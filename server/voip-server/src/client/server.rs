use log::{error, info};

use std::collections::HashMap;
use std::net::SocketAddr;
use std::sync::Arc;
use std::io;

use tokio::net::UdpSocket;
use tokio::sync::RwLock;

use packets::PacketParser;
use packets::client::{SMPackets, SMStreaming, ClientPacket, SMAuth};

use super::global::CLIENTS;
use super::peer::Peer;

pub struct ClientServer {
  socket: Option<Arc<UdpSocket>>,
  peers: Arc<RwLock<HashMap<SocketAddr, Peer>>>
}

async fn send_bulk(socket: Arc<UdpSocket>, packet: SMPackets, peers: Vec<SocketAddr>) -> Result<(), io::Error> {

  let data = SMPackets::serialize(packet)?;

  for peer_id in peers {
    socket.send_to(&data, peer_id).await?;
  }

  Ok(())
}

impl ClientServer {
  pub fn new() -> Self {
    Self {
      socket: None,
      peers: Arc::new(RwLock::new(HashMap::new())),
    }
  }

  pub async fn run(&mut self) -> Result<(), io::Error> {
    info!("Starting client server...");

    let socket = UdpSocket::bind("127.0.0.1:8081").await?;

    let sck = Arc::new(socket);

    let sck_handler = sck.clone();

    let peers_handler = self.peers.clone();

    self.socket = Some(sck);

    tokio::spawn(async move {
      let mut buf = [0; 1024]; 
      loop {
        let received = sck_handler.recv_from(&mut buf).await;

        match received {
          Ok((len, addr)) => {
            let packet = ClientPacket::deserialize(&buf[..len]).unwrap();

            info!("Received client packet");
            let mut peers = peers_handler.write().await;

            if peers.contains_key(&addr) {
              let peer = peers.get(&addr).unwrap();

              match packet {
                ClientPacket::Streaming(stream) => {
                  info!("Received streaming packet");
                  let packet = SMPackets::Streaming(SMStreaming { id: peer.id, audio_frame: stream.audio_frame });

                  let result = send_bulk(sck_handler.clone(), packet, peers.keys().into_iter().map(|x| x.clone()).filter(|x| *x != addr).collect()).await;
                
                  info!("Result: {:?}", result);
                }
                _ => {
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
                  peers.insert(addr, peer);

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