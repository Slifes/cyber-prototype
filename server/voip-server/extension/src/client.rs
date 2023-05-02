use godot::prelude::*;
use godot::engine::Engine;

use std::sync::Arc;

use tokio::net::UdpSocket;
use tokio::sync::mpsc;

use packets::client::{ClientPacket, SMPackets};

use super::mic::VoipMicrophone;
use super::manager::VoipManager;

#[derive(GodotClass)]
#[class(base=Node)]
struct VoipClient {
  socket:  Option<Arc<UdpSocket>>,
  microphone: Option<Gd<VoipMicrophone>>,
  manager: Option<Gd<VoipManager>>,
  record_tx: Option<mpsc::Receiver<SMPackets>>,

  #[base]
  base: Base<Node>,
}

impl VoipClient {
  pub async fn send(&mut self, packet: ClientPacket) -> Result<(), std::io::Error>{
    if let Some(socket) = &self.socket {
      let data = ClientPacket::parser(packet)?;
      socket.send(&data).await?;
    } 

    Ok(())
  }

  pub async fn start(&mut self) -> Result<(), std::io::Error> {
    let mut socket = UdpSocket::bind("0.0.0.0:0").await?;

    socket.connect("127.0.0.1:8081").await?;

    let sck = Arc::new(socket);

    self.run_handler(sck.clone());

    self.socket = Some(sck);

    Ok(())
  }

  pub fn run_handler(&mut self, socket: Arc<UdpSocket>) {
    let (record_tx, record_rx) = mpsc::channel(1024);

    self.record_tx = Some(record_rx);

    tokio::spawn(async move {
      loop {
        let mut buffer = [0u8; 1024];

        match socket.recv(&mut buffer).await {
          Ok(size) => {
            if size == 0 {
              continue;
            }

            let pck = SMPackets::parser(&buffer[..size]).unwrap();

            let _ = record_tx.send(pck).await;
          }
          Err(e) => {
            godot_print!("Error: {}", e);
          }
        }
      }
    });
  }

}

#[godot_api]
impl NodeVirtual for VoipClient {
  fn init(base: Base<Node>) -> Self {
    Self {
      socket: None,
      microphone: None,
      manager: None,
      record_tx: None,
      base
    }
  }

  fn ready(&mut self) {
    if Engine::singleton().is_editor_hint() {
      return;
    }

    self.microphone = Some(self.base.get_node_as::<VoipMicrophone>("VoipMicrophone"));

    self.manager = Some(self.base.get_node_as::<VoipManager>("/root/GlobalVoipManager"));
  }

  fn process(&mut self, _delta: f64) {
    if Engine::singleton().is_editor_hint() {
      return;
    }

    if let Some(record_tx) = self.record_tx.as_mut() {
      let pck = record_tx.try_recv();
      
      if let Ok(packet) = pck {
        match packet {
          SMPackets::Streaming(streaming) => {
            if let Some(manager) = &mut self.manager {
              manager.bind_mut().on_speak_data(streaming.id, streaming.audio_frame);
            }
          }
          SMPackets::Auth(auth) => {
            godot_print!("Auth: {:?}", auth)
            // if let Some(manager) = &self.manager {
            //   manager.bind_mut().on_auth(auth.id);
            // }
          }
        }
      } 
    }
  }
}