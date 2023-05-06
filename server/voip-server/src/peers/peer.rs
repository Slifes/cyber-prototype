use log::info;

use std::sync::Arc;
use std::net::SocketAddr;

use tokio::net::UdpSocket;

use packets::PacketParser;
use packets::client::SMPackets;

#[derive(Debug)]
struct NearPeer {
  id: i32,
  addr: SocketAddr,
}

#[derive(Debug)]
pub struct Peer {
  pub id: i32,
  pub sck_addr: SocketAddr,
  pub ping_time: u64,
  near_players: Vec<NearPeer>,
}

impl Peer {
  pub fn new(sck_addr: SocketAddr) -> Self {
    Self {
      id: i32::MAX,
      sck_addr,
      ping_time: 0,
      near_players: Vec::new(),
    }
  }

  pub fn set_client_id(&mut self, id: i32) {
    self.id = id;
  }

  pub async fn add_player(&mut self, id: i32, addr: SocketAddr) {
    self.near_players.push(NearPeer { id, addr });
  }

  pub fn remove_player(&mut self, player_id: i32) {
    if let Some(position) = self.near_players.iter().position(|x| x.id == player_id) {
      self.near_players.remove(position);
    }
  }

  pub async fn send_packet_to_near_players(&self, socket: Arc<UdpSocket>, packet: SMPackets) {
    let data = SMPackets::serialize(packet).unwrap();

    for peer in self.near_players.iter() {
      if let Err(x) = socket.send_to(&data, peer.addr).await {
        info!("Error sending packet to peer: {}", x);
      }
    }
  }
}