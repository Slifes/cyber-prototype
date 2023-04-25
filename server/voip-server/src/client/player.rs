use tokio::net::UdpSocket;
use std::collections::HashMap;
use std::sync::{Arc, RwLock};

struct Client {
  id: u32,
  shard_id: u32,
  socket: UdpSocket,
  near_players: Arc<RwLock<HashMap<u32, Arc<Client>>>>,
}

impl Client {
  fn new(id: u32, socket: UdpSocket) -> Self {
    Self {
      id,
      shard_id: u32::MAX,
      socket,
      near_players: Arc::new(RwLock::new(HashMap::new())),
    }
  } 

  pub fn add_player(&self, player: Arc<Client>) {
    self.near_players.write().unwrap().insert(player.id, player);
  }

  pub fn remove_player(&self, player_id: u32) {
    self.near_players.write().unwrap().remove(&player_id);
  }

  pub fn send_packet_to_near_players(&self, packet: &[u8]) {
    for (_, player) in self.near_players.read().unwrap().iter() {
      player.socket.send_to(packet, player.socket.peer_addr().unwrap());
    }
  }
}