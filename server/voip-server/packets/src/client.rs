use serde::{Serialize, Deserialize};
use super::parser::PacketParser;

#[derive(Debug, Serialize, Deserialize)]
pub struct CMAuth {
  pub client_id: i64,
  pub client_key: String,
}

#[derive(Debug, Serialize, Deserialize)]
pub struct CMStreaming {
  pub audio_frame: Vec<Vec<u8>>,
}

#[derive(Debug, Serialize, Deserialize)]
#[serde(tag = "id", content = "data")]
pub enum ClientPacket {
  #[serde(rename="1")]
  Auth(CMAuth),
  #[serde(rename="2")]
  Streaming(CMStreaming),
}

#[derive(Debug, Serialize, Deserialize)]
pub struct SMAuth {
  pub status: bool,
}

#[derive(Debug, Serialize, Deserialize)]
pub struct SMStreaming {
  pub id: i64,
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

impl PacketParser<ClientPacket> for ClientPacket { }
impl PacketParser<SMPackets> for SMPackets { }
