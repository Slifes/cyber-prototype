use serde::Deserialize;

use packets::shard::{
  ShardAuthentication,
  ShardPlayerConnect,
  ShardPlayerDisconnect,
};

#[derive(Debug)]
pub struct Shard {
  id: u32,
  is_authenticated: bool,
  peers: Vec<i32>,
}

unsafe impl Send for Shard {}
unsafe impl Sync for Shard {}

impl Shard {
  pub fn new() -> Self {
    Self {
      id: u32::MAX,
      is_authenticated: false,
      peers: Vec::new(),
    }
  }

  pub fn set_shard_id(&mut self, id: u32) {
    self.id = id;
  }
}

pub trait ReceivePacket<T> where T: Deserialize<'static> {
  fn receive_packet(&mut self, packet: T);
}

impl ReceivePacket<ShardAuthentication> for Shard {
  fn receive_packet(&mut self, packet: ShardAuthentication) {
    self.is_authenticated = true;

    println!("Received authentication packet: {:?}", packet);
  }
}

impl ReceivePacket<ShardPlayerConnect> for Shard {
  fn receive_packet(&mut self, packet: ShardPlayerConnect) {
    self.peers.push(packet.player_id);

    println!("Received player connect packet: {:?}", packet);
    println!("Shard peers: {:?}", self.peers)
  }
}

impl ReceivePacket<ShardPlayerDisconnect> for Shard {
  fn receive_packet(&mut self, packet: ShardPlayerDisconnect) {
    let index = self.peers
      .iter()
      .position(|x| x == &packet.player_id);

    if let Some(idx) = index {
      self.peers.remove(idx);
    }

    println!("Received player disconnect packet: {:?}", packet);
    println!("Shard peers: {:?}", self.peers);
  }
}