use tokio::net::UdpSocket;
use tokio::sync::RwLock;
use std::collections::HashMap;
use std::future::Future;

pub struct Client {
  id: i32,
  shard_id: u32,
  socket: UdpSocket,
  near_players: HashMap<i32, RwLock<Client>>,
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

  pub async fn add_player(&mut self, player: RwLock<Client>) {
    let player_id = {
      player.read().await.id
    };
    self.near_players.insert(player_id, player);
  }

  pub fn remove_player(&mut self, player_id: i32) {
    self.near_players.remove(&player_id);
  }

  pub async fn send_packet_to_near_players(&self, packet: &[u8]) {
    // let write = self.near_players
    //   .iter()
    //   .map(|(_, player)| async { player.write().await.socket.send(packet) })
    //   .collect::<Vec<_>>();

  }
}