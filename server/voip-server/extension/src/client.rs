use tokio::net::UdpSocket;
use packets::client::{ClientPacket, ClientAuth};

use super::mic::VoipMicrophone;

struct VoipClient {
  socket: UdpSocket,
  microphone: Option<VoipMicrophone>,
}

impl VoipClient {
  pub async fn new() -> Result<Self, std::io::Error> {
    Ok(Self {
      socket: UdpSocket::bind("127.0.0.1:8081").await?,
      microphone: None
    })
  }

  pub async fn send(&mut self, packet: ClientPacket) -> Result<(), std::io::Error>{
    let data = ClientPacket::parser(packet)?;
    self.socket.send(&data).await?;

    Ok(())
  }

  pub fn run_handler(&mut self) {
    tokio::spawn(async move {
      
    });
  }
}