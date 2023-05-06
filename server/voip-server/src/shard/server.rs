use log::info;

use std::io;
use std::sync::Arc;

use tokio::sync::{Mutex, RwLock};
use tokio::io::{AsyncReadExt, AsyncWriteExt};
use tokio::net::{TcpListener, TcpStream};

use packets::shard::ShardPackets;

use super::shard_data::{Shard, ReceivePacket};
use crate::types::Peers;

pub struct ShardServer {
  shards: Arc<Mutex<Vec<Arc<RwLock<Shard>>>>>,
  tasks: Vec<tokio::task::JoinHandle<()>>,
}

impl ShardServer {
  pub fn new() -> Self {
    ShardServer {
      shards: Arc::new(Mutex::new(Vec::new())),
      tasks: Vec::new()
    }
  }

  pub async fn run(&mut self, peers: Peers) -> Result<(), io::Error> {
    info!("Shard server starting...");
    
    let listener = TcpListener::bind("0.0.0.0:8080").await?;

    loop {
      info!("Waiting for connection...");

      let (mut stream, _) = listener.accept().await?;

      let shard_vector = self.shards.clone();
      let shard_peers = peers.clone();

      let task = tokio::spawn(async move {

        let shard_threaded = {
          let mut shards = shard_vector.lock().await;
          let shard = Arc::new(RwLock::new(Shard::new(shard_peers)));
          let shard_threaded = shard.clone();
          shards.push(shard);
          shard_threaded
        };

        let result = handler_connect(shard_threaded, &mut stream).await;

        info!("Connection closed: {:?}", result);
      });

      self.tasks.push(task);
    }
  }
}

async fn handler_connect(shard: Arc<RwLock<Shard>>, stream: &mut TcpStream) -> Result<(), io::Error> {
  loop {
    let mut buf = [0; 1024];
    let size = stream.read(&mut buf).await?;
    stream.flush().await?;

    info!("Before shard write");
    info!("Received packet: {:?}", buf[..size].to_vec());

    let mut shard_data = shard.write().await;

    let packet_type: ShardPackets = match ShardPackets::parser(&buf[..size]) {
      Ok(packet) => packet,
      Err(e) => {
        info!("Error: {}", e);
        continue;
      }
    };

    match packet_type {
      ShardPackets::Authentication(packet) => shard_data.receive_packet(packet, stream).await,
      ShardPackets::PlayerConnect(packet) => shard_data.receive_packet(packet, stream).await,
      ShardPackets::PlayerDisconnect(packet) => shard_data.receive_packet(packet, stream).await,
      ShardPackets::PlayerAddCloser(packet) => shard_data.receive_packet(packet, stream).await,
      ShardPackets::PlayerRemoveCloser(packet) => shard_data.receive_packet(packet, stream).await,
    }
  }
}
