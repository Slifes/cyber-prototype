use godot::prelude::*;
use godot::engine::Engine;

use std::sync::{Arc, mpsc};
use std::net::UdpSocket;
use std::thread;

use packets::{
  PacketParser,
  client::{ClientPacket, CMStreaming, CMAuth, SMPackets}
};

use super::mic::VoipMicrophone;
use super::manager::VoipManager;

#[derive(GodotClass)]
#[class(base=Node)]
struct VoipClient {
  #[export]
  address: GString,

  socket:  Option<Arc<UdpSocket>>,
  microphone: Option<Gd<VoipMicrophone>>,
  manager: Option<Gd<VoipManager>>,
  record_tx: Option<mpsc::Receiver<SMPackets>>,

  #[base]
  base: Base<Node>,
}

#[godot_api]
impl VoipClient {
  pub fn send(&mut self, packet: ClientPacket) -> Result<(), std::io::Error>{
    if let Some(socket) = &self.socket {
      let data = ClientPacket::serialize(packet)?;
      godot_print!("Sending packet: {:?}", data.len());
      socket.send(&data)?;
    } 

    Ok(())
  }

  pub fn start(&mut self) -> Result<(), std::io::Error> {
    let socket = UdpSocket::bind("0.0.0.0:0").expect("Failed to bind socket");

    socket.connect(self.address.to_string()).expect("Failed to connect to server");

    let sck = Arc::new(socket);

    let sck_handler = sck.clone();

    self.socket = Some(sck);

    let client_id: i32 = self.base().get_multiplayer()
      .unwrap().get_unique_id().try_into().unwrap();

    self.send(ClientPacket::Auth(CMAuth {
      client_id,
      client_key: "asjduioasjdoiasjd".to_string(),
    }));

    self.run_handler(sck_handler);

    Ok(())
  }

  pub fn run_handler(&mut self, socket: Arc<UdpSocket>) {
    let (record_tx, record_rx) = mpsc::channel();
 
    self.record_tx = Some(record_rx);

    thread::spawn(move || {
      loop {
        let mut buffer = [0u8; 2048];

        match socket.recv(&mut buffer) {
          Ok(size) => {
            if size == 0 {
              continue;
            }

            let pck = SMPackets::deserialize(&buffer[..size]).unwrap();

            let _ = record_tx.send(pck);
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
impl INode for VoipClient {
  fn init(base: Base<Node>) -> Self {
    Self {
      address: GodotString::from("127.0.0.1:8081"),
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

    self.microphone = Some(self.base().get_node_as::<VoipMicrophone>("VoipMicrophone"));

    self.manager = Some(self.base().get_node_as::<VoipManager>("/root/GlobalVoipManager"));
  
    self.start();
  }

  fn process(&mut self, _delta: f64) {
    if Engine::singleton().is_editor_hint() {
      return;
    }

    let microphone = self.microphone.as_mut().unwrap();

    let packet_count = {
      let mic_mut = microphone.bind();
      mic_mut.get_opus_packet_count()
    };

    if packet_count > 1 {
      godot_print!("Running packet send");
      let packets = {
        let mut mic_mut = microphone.bind_mut();
        mic_mut.get_latest_opus_packet()
      };

      let packet = ClientPacket::Streaming(CMStreaming {
        audio_frame: packets,
      });
      
      let result = self.send(packet);

      godot_print!("Result send streaming: {:?}", result);
    }
    
    if let Some(record_tx) = self.record_tx.as_mut() {
      let pck = record_tx.try_recv();

      if let Ok(packet) = pck {
        match packet {
          SMPackets::Streaming(streaming) => {
            godot_print!("Streaming id: {:?}", streaming.id);
            if let Some(manager) = &mut self.manager {
              godot_print!("Running manager");
              manager.bind_mut()
                .on_speak_data(streaming.id, streaming.audio_frame);
            } 
          }
          SMPackets::Auth(auth) => {
            godot_print!("Auth: {:?}", auth)
            // if let Some(manager) = &self.manager {
            //   manager.bind_mut().on_auth(auth.id);
            // }
          }
          SMPackets::Pong { ping_time } => {
            godot_print!("Pong: {:?}", ping_time);
          }
        }
      } 
    }
  }
}