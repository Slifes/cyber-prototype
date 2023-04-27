use serde::{Serialize, Deserialize};
use super::parser::serialize;
use std::io;

#[derive(Debug, Serialize, Deserialize)]
pub struct ClientAuth {
  client_id: u32,
  client_key: String,
}

#[derive(Debug, Serialize, Deserialize)]
pub struct ClientStreming {
  frame_length: u16,
  audio_frame: Vec<Vec<u8>>,
}

#[derive(Debug, Serialize, Deserialize)]
#[serde(tag = "id", content = "data")]
pub enum ClientPacket {
  #[serde(rename="1")]
  Auth(ClientAuth),
  #[serde(rename="2")]
  Streaming(ClientStreming),
}

impl ClientPacket {
  pub fn parser(packet: ClientPacket) -> Result<Vec<u8>, io::Error> {
    serialize(packet)
  }
}