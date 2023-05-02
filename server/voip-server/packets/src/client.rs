use serde::{Serialize, Deserialize};
use super::parser::{serialize, deserialize};
use std::io;

#[derive(Debug, Serialize, Deserialize)]
pub struct CMAuth {
  client_id: u32,
  client_key: String,
}

#[derive(Debug, Serialize, Deserialize)]
pub struct CMStreaming {
  frame_length: u16,
  audio_frame: Vec<Vec<u8>>,
}

#[derive(Debug, Serialize, Deserialize)]
#[serde(tag = "id", content = "data")]
pub enum ClientPacket {
  #[serde(rename="1")]
  Auth(CMAuth),
  #[serde(rename="2")]
  Streaming(CMStreaming),
}

impl ClientPacket {
  pub fn parser(packet: ClientPacket) -> Result<Vec<u8>, io::Error> {
    serialize(packet)
  }
}

#[derive(Debug, Serialize, Deserialize)]
pub struct SMAuth {
  pub status: bool,
}

#[derive(Debug, Serialize, Deserialize)]
pub struct SMStreaming {
  pub id: u32,
  pub audio_frame: Vec<Vec<u8>>,
}

#[derive(Debug, Serialize, Deserialize)]
#[serde(tag = "id", content = "data")]
pub enum SMPackets {
  #[serde(rename="1")]
  Auth(SMAuth),
  #[serde(rename="2")]
  Streaming(SMStreaming),
}

impl SMPackets {
  pub fn parser(value: &[u8]) -> Result<Self, io::Error> {
    deserialize(value)
  }
}
