use tokio::net::UdpSocket;
use std::io;

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

    loop {
      let mut buf = [0; 1024];

      let (len, addr) = self.socket.as_ref().unwrap().recv_from(&mut buf).await?;

      println!("Received {} bytes from {:?}", len, addr);
    }
  }
}