use serde::{Serialize, Deserialize};
use super::parser::deserialize; 
use std::io;

#[derive(Debug, Serialize, Deserialize)]
pub struct ShardAuthentication {
  pub packet_id: u32,
  pub shard_id: u32,
  pub shard_key: String,
}

#[derive(Debug, Serialize, Deserialize)]
pub struct ShardPlayerConnect {
  pub packet_id: u32,
  pub player_id: i32,
}

#[derive(Debug, Serialize, Deserialize)]
pub struct ShardPlayerDisconnect {
  pub packet_id: u32,
  pub player_id: i32,
}

#[derive(Debug, Serialize, Deserialize)]
pub struct ShardPlayerAddCloser {
  pub packet_id: u32,
  pub player_id: i32,
  pub closer_id: i32,
}

#[derive(Debug, Serialize, Deserialize)]
pub struct ShardPlayerRemovePlayer {
  pub packet_id: u32,
  pub player_id: i32,
  pub closer_id: i32,
}

#[derive(Debug, Serialize, Deserialize)]
#[serde(tag = "id", content = "data")]
pub enum ShardPacket {
  #[serde(rename = "1")]
  Authentication(ShardAuthentication),
  #[serde(rename = "2")]
  PlayerConnect(ShardPlayerConnect),
  #[serde(rename = "3")]
  PlayerDisconnect(ShardPlayerDisconnect),
  #[serde(rename = "4")]
  PlayerAddCloser(ShardPlayerAddCloser),
  #[serde(rename = "5")]
  PlayerRemovePlayer(ShardPlayerRemovePlayer),
}

#[derive(Debug)]
pub enum ShardPackets {
  Authentication(ShardAuthentication),
  PlayerConnect(ShardPlayerConnect),
  PlayerDisconnect(ShardPlayerDisconnect),
  PlayerAddCloser(ShardPlayerAddCloser),
  PlayerRemoveCloser(ShardPlayerRemovePlayer)
}

impl ShardPackets {
  pub fn parser(value: &[u8]) -> Result<Self, io::Error> {
    match value[1] {
      1 => Ok(ShardPackets::Authentication(deserialize(value)?)),
      2 => Ok(ShardPackets::PlayerConnect(deserialize(value)?)),
      3 => Ok(ShardPackets::PlayerDisconnect(deserialize(value)?)),
      4 => Ok(ShardPackets::PlayerAddCloser(deserialize(value)?)),
      5 => Ok(ShardPackets::PlayerRemoveCloser(deserialize(value)?)),
      _ => Err(io::Error::new(io::ErrorKind::Other, "Invalid packet id")),
    }
  }
}