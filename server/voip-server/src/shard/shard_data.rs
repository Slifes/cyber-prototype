use log::info;
use serde::Deserialize;
use async_trait::async_trait;

use crate::types::Peers;

use packets::shard::{
  ShardAuthentication,
  ShardPlayerConnect,
  ShardPlayerDisconnect,
  ShardPlayerAddCloser,
  ShardPlayerRemovePlayer
};

#[derive(Debug)]
pub struct Shard {
  id: u32,
  is_authenticated: bool,
  peers_id: Vec<i32>,
  peers: Peers,
}

impl Shard {
  pub fn new(peers: Peers) -> Self {
    Self {
      id: u32::MAX,
      is_authenticated: false,
      peers,
      peers_id: Vec::new(),
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
    self.peers_id.push(packet.player_id);

    info!("Received player connect packet: {:?}", packet);
    info!("Shard peers: {:?}", self.peers)
  }
}

#[async_trait]
impl ReceivePacket<ShardPlayerDisconnect> for Shard {
  async fn receive_packet(&mut self, packet: ShardPlayerDisconnect) {
    let index = self.peers_id
      .iter()
      .position(|x| x == &packet.player_id);

    if let Some(idx) = index {
      self.peers_id.remove(idx);
    }

    info!("Received player disconnect packet: {:?}", packet);
  }
}

#[async_trait]
impl ReceivePacket<ShardPlayerAddCloser> for Shard {
  async fn receive_packet(&mut self, packet: ShardPlayerAddCloser) {
    let mut clients = self.peers.write().await;

    let closer_peer = clients.get(&packet.closer_id);

    if closer_peer.is_none() {
      return;
    }

    let addr = closer_peer.unwrap().read().await.sck_addr;

    if let Some(peer) = clients.get_mut(&packet.player_id) {
      peer
        .write().await
        .add_player(packet.closer_id, addr).await;
    }

    info!("Received player add closer packet: {:?}", packet);
  }
}

#[async_trait]
impl ReceivePacket<ShardPlayerRemovePlayer> for Shard {
  async fn receive_packet(&mut self, packet: ShardPlayerRemovePlayer) {
    let mut clients = self.peers.write().await;

    if let Some(peer) = clients.get_mut(&packet.player_id) {
      peer
        .write().await
        .remove_player(packet.closer_id);
    }

    info!("Received player remove player packet: {:?}", packet);
  }
}