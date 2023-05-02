use log::info;

use tokio::sync::RwLock;
use std::sync::Arc;
use std::net::SocketAddr;

pub struct Peer {
  pub id: i64,
  pub sck_addr: SocketAddr,
  near_players: Vec<Arc<RwLock<Peer>>>,
}

impl Peer {
  pub fn new(sck_addr: SocketAddr) -> Self {
    Self {
      id: i64::MAX,
      sck_addr,
      near_players: Vec::new(),
    }
  }

  pub fn set_client_id(&mut self, id: i64) {
    self.id = id;
  }

  pub async fn add_player(&mut self, player: Arc<RwLock<Peer>>) {
    let player_id = {
      player.read().await.id
    };
    self.near_players.push(player);
  }

  pub fn remove_player(&mut self, player_id: i32) {
    //self.near_players.remove(&player_id);
  }

  pub async fn send_packet_to_near_players(&self, packet: &[u8]) {
    // let write = self.near_players
    //   .iter()
    //   .map(|(_, player)| async { player.write().await.socket.send(packet) })
    //   .collect::<Vec<_>>();
  }
}

impl Drop for Peer {
  fn drop(&mut self) {
    info!("Dropping peer: {:?}", self.sck_addr);
  }
}