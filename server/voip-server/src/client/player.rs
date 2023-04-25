use tokio::net::UdpSocket;
use std::collections::HashMap;
use std::sync::{Arc, RwLock};

pub struct Client {
  id: i32,
  shard_id: u32,
  socket: UdpSocket,
  near_players: HashMap<i32, Box<Client>>,
}

impl Client {
  pub fn new(socket: UdpSocket) -> Self {
    Self {
      id: i32::MAX,
      shard_id: u32::MAX,
      socket,
      near_players: HashMap::new(),
    }
  }

  pub fn set_client_id(&mut self, id: i32) {
    self.id = id;
  }

  pub fn add_player(&mut self, player: Box<Client>) {
    self.near_players.insert(player.id, player);
  }

  pub fn remove_player(&mut self, player_id: i32) {
    self.near_players.remove(&player_id);
  }

  pub fn send_packet_to_near_players(&self, packet: &[u8]) {
    for (_, player) in self.near_players.iter() {
      player.socket.send(packet);
    }
  }
}