use serde::{Serialize, Deserialize};

#[derive(Debug, Serialize, Deserialize)]
pub struct ClientAuth {
  client_id: u32,
  client_key: String,
}

#[derive(Debug, Serialize, Deserialize)]
pub struct ClientStreming {
  client_id: u32,
  audio_data: Vec<u8>,
}

#[derive(Debug, Serialize, Deserialize)]
pub enum ClientPacket {
  Auth(ClientAuth),
  Streaming(ClientStreming),
}
