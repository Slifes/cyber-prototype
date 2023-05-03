use log::info;
use serde::Deserialize;
use async_trait::async_trait;

use crate::peers::CLIENTS;

use packets::shard::{
  ShardAuthentication,
  ShardPlayerConnect,
  ShardPlayerDisconnect,
  ShardPlayerAddCloser
};

#[derive(Debug)]
pub struct Shard {
  id: u32,
  is_authenticated: bool,
  peers: Vec<i32>,
}

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

#[async_trait]
pub trait ReceivePacket<T> where T: Deserialize<'static> {
  async fn receive_packet(&mut self, packet: T);
}

#[async_trait]
impl ReceivePacket<ShardAuthentication> for Shard {
  async fn receive_packet(&mut self, packet: ShardAuthentication) {
    self.is_authenticated = true;

    info!("Received authentication packet: {:?}", packet);
  }
}

#[async_trait]
impl ReceivePacket<ShardPlayerConnect> for Shard {
  async fn receive_packet(&mut self, packet: ShardPlayerConnect) {
    self.peers.push(packet.player_id);

    info!("Received player connect packet: {:?}", packet);
    info!("Shard peers: {:?}", self.peers)
  }
}

#[async_trait]
impl ReceivePacket<ShardPlayerDisconnect> for Shard {
  async fn receive_packet(&mut self, packet: ShardPlayerDisconnect) {
    let index = self.peers
      .iter()
      .position(|x| x == &packet.player_id);

    if let Some(idx) = index {
      self.peers.remove(idx);
    }

    info!("Received player disconnect packet: {:?}", packet);
    info!("Shard peers: {:?}", self.peers);
  }
}

#[async_trait]
impl ReceivePacket<ShardPlayerAddCloser> for Shard {
  async fn receive_packet(&mut self, packet: ShardPlayerAddCloser) {
    let clients = CLIENTS.read().await;

    let client = clients.get(&packet.player_id).unwrap();
    let closer = clients.get(&packet.closer_id).unwrap();

    // client.write().await.add_player(closer.clone());
    // closer.write().await.add_player(client.clone());

    info!("Received player add closer packet: {:?}", packet);
  }
}