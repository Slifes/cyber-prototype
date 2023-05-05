use log::{error, info};

use std::collections::HashMap;
use std::net::SocketAddr;
use std::sync::Arc;
use std::io;

use tokio::net::UdpSocket;
use tokio::sync::RwLock;

use packets::PacketParser;
use packets::client::{SMPackets, SMStreaming, ClientPacket, SMAuth};

use super::peer::Peer;
use crate::types::Peers;

pub struct PeerServer {
  socket: Option<Arc<UdpSocket>>,
  peers_by_addr: Arc<RwLock<HashMap<SocketAddr, i32>>>
}

impl PeerServer {
  pub fn new() -> Self {
    Self {
      socket: None,
      peers_by_addr: Arc::new(RwLock::new(HashMap::new())),
    }
  }

  pub async fn run(&mut self, peers: Peers) -> Result<(), io::Error> {
    info!("Starting client server...");

    let socket = UdpSocket::bind("0.0.0.0:8081").await?;

    let sck = Arc::new(socket);

    let sck_handler = sck.clone();

    let peers_handler = self.peers_by_addr.clone();

    self.socket = Some(sck);

    tokio::spawn(async move {
      let mut buf = [0; 2048]; 
      loop {
        let received = sck_handler.recv_from(&mut buf).await;

        let mut peers_id = peers_handler.write().await;
        let mut peers = peers.write().await;

        match received {
          Ok((len, addr)) => {
            info!("Received client packet");

            let packet = match ClientPacket::deserialize(&buf[..len]) {
              Ok(packet) => packet,
              Err(e) => {
                error!("Error: {}", e);
                peers_id.remove(&addr);
                continue;
              }
            };

            if peers_id.contains_key(&addr) {
              let peer_id = peers_id.get(&addr).unwrap();
              let peer = peers.get(peer_id).unwrap().read().await;

              match packet {
                ClientPacket::Streaming(stream) => {
                  info!("Received streaming packet");
                  
                  let packet = SMPackets::Streaming(SMStreaming { id: peer.id, audio_frame: stream.audio_frame });

                  peer.send_packet_to_near_players(sck_handler.clone(), packet).await;
                }
                ClientPacket::Ping {
                  ping_time
                } => {
                  info!("Received ping packet");

                  let packet = SMPackets::Pong {
                    ping_time
                  };

                  let result = sck_handler.send_to(&SMPackets::serialize(packet).unwrap(), addr).await;
                
                  info!("Result: {:?}", result);
                }
                _ => {
                  // peers_id.remove(&addr);
                  // peers.remove(peer_id);
                  error!("Client not authenticated");
                }
              }
            } else {
              let mut peer = Peer::new(addr);

              let packet = ClientPacket::deserialize(&buf[..len]).unwrap();

              match packet {
                ClientPacket::Auth(auth) => {
                  info!("Client authenticated {:?}", auth);
                  peer.set_client_id(auth.client_id);

                  peers_id.insert(addr, auth.client_id);
                  peers.insert(auth.client_id, Arc::new(RwLock::new(peer)));

                  let packet = SMPackets::Auth(SMAuth { status: true });
                  
                  let result = sck_handler.send_to(&SMPackets::serialize(packet).unwrap(), addr).await;
                
                  info!("Result: {:?}", result);
                }
                _ => {
                  error!("Client not authenticated");
                }
              };
            }
          }
          Err(e) => {
            error!("Error: {}", e);
          }
        }
      }
    });

    Ok(())
  }
}