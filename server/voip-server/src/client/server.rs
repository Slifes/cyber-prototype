use tokio::net::UdpSocket;
use std::io;

use super::global::CLIENTS;

struct ClientServer {
  socket: Option<UdpSocket>,
}

impl ClientServer {
  pub fn new() -> Self {
    Self {
      socket: None,
    }
  }

  async fn run(&mut self) -> Result<(), io::Error> {
    self.socket = Some(UdpSocket::bind("127.0.0.1:8081").await?);

    tokio::spawn(async move {
      let mut buf = [0; 1024];

      // loop {
      //   let (len, addr) = self.socket.as_ref().unwrap().recv_from(&mut buf).await;

      //   println!("Received {} bytes from {:?}", len, addr);
      // }
    });

    loop {
      let mut buf = [0; 1024];

      let (len, addr) = self.socket.as_ref().unwrap().recv_from(&mut buf).await?;

      println!("Received {} bytes from {:?}", len, addr);
    }
  }
}