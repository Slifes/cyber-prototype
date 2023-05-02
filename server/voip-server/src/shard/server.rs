use log::info;

use std::io;
use std::sync::Arc;

use tokio::sync::{Mutex, RwLock};
use tokio::io::AsyncReadExt;
use tokio::net::{TcpListener, TcpStream};

use packets::shard::ShardPackets;

use super::shard_data::{Shard, ReceivePacket};

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

  pub async fn run(&mut self) -> Result<(), io::Error> {
    let listener = TcpListener::bind("127.0.0.1:8080").await?;

    loop {
      let (mut stream, _) = listener.accept().await?;

      let shard_vector = self.shards.clone();

      let task = tokio::spawn(async move {
        let mut shards = shard_vector.lock().await;
        let shard = Arc::new(RwLock::new(Shard::new()));
        let shard_threaded = shard.clone();

        shards.push(shard);

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

    let mut shard_data = shard.write().await;

    info!("Received packet: {:?}", buf[..size].to_vec());

    let packet_type: ShardPackets = ShardPackets::parser(&buf[..size])?;

    match packet_type {
      ShardPackets::Authentication(packet) => shard_data.receive_packet(packet).await,
      ShardPackets::PlayerConnect(packet) => shard_data.receive_packet(packet).await,
      ShardPackets::PlayerDisconnect(packet) => shard_data.receive_packet(packet).await
    }
  }
}
