use std::io;
use std::sync::{Arc, RwLock};
use std::marker::{Send, Sync};

use serde::{Serialize, Deserialize};
use rmp_serde::{Deserializer, Serializer};

use tokio::io::AsyncReadExt;
use tokio::net::{TcpListener, TcpStream};

use packets::shard::{ShardPacket, ShardAuthentication};

async fn handler_connect(stream: &mut TcpStream) -> Result<(), io::Error>{
  loop {
    let mut buf = [0; 1024];
    let size = stream.read(&mut buf).await?;
    
    println!("Received buf: {:?}", buf[..size].to_vec());

    let mut buff_serializer = Vec::new();
    let mut serializer = Serializer::new(&mut buff_serializer);

    let pck = ShardAuthentication {
      packet_id: 1,
      shard_id: 20,
      shard_key: String::from("test"),
    };

    ShardPacket::Authentication(pck).serialize(&mut serializer).unwrap();

    println!("Serialize buf: {:?}", buff_serializer);

    // let packet: ShardPacket = bincode::deserialize(&buf[..size])
    //   .map_err(|x| io::Error::new(io::ErrorKind::Other, x))?;

    let mut de = Deserializer::new(&buf[..size]);
    
    match buf[1] {
      1 => {
        let packet: ShardAuthentication = Deserialize::deserialize(&mut de)
        .map_err(|x| io::Error::new(io::ErrorKind::Other, x))?;

        let shard_id = packet.shard_id;
        // shard.is_authenticated = true;
        // shard.set_shard_id(shard_id);
        println!("Received authentication packet: {:?}", shard_id);
      }
      _ => {
        println!("Received unknown packet");
      }
    }
  }
}

#[derive(Debug)]
struct Shard {
  id: u32,
  is_authenticated: bool,
}

impl Shard {
  pub fn new() -> Self {
    Self {
      id: u32::MAX,
      is_authenticated: false,
    }
  }

  pub fn set_shard_id(&mut self, id: u32) {
    self.id = id;
  }
}

pub struct ShardServer {
  shards: Vec<Arc<Shard>>,
}

impl ShardServer {
  pub fn new() -> Self {
    Self {
      shards: Vec::new(),
    }
  }

  pub async fn run(&mut self) -> Result<(), io::Error> {
    let listener = TcpListener::bind("127.0.0.1:8080").await?;

    loop {
      let (mut stream, _) = listener.accept().await?;
      let shard = Arc::new(Shard::new());
      let shard_thread = shard.clone();

      self.shards.push(shard);

      println!("New connection: {:?}", stream);

      tokio::spawn(async move {
        handler_connect(&mut stream).await.unwrap();
      });
    }
}
}