use log::info;

use std::collections::HashMap;
use std::sync::Arc;

use tokio::sync::RwLock;

use super::peers::server::PeerServer;
use super::shard::server::ShardServer;
use super::types::Peers;

pub struct Server {
  peers: Peers,
}

impl Server {
  pub fn new() -> Self {
    Self {
      peers: Arc::new(RwLock::new(HashMap::new())),
    }
  }

  pub async fn run(&self) {
    let mut shard = ShardServer::new();
    let mut peer_server = PeerServer::new();

    let result = tokio::join!(
      shard.run(self.peers.clone()),
      peer_server.run(self.peers.clone())
    );

    info!("Server closed: {:?}", result);
  }
}