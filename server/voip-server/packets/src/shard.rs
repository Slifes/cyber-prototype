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
  pub shard_id: u32,
  pub player_id: u32,
  pub closer_id: u32,
}

#[derive(Debug, Serialize, Deserialize)]
pub struct ShardPlayerRemovePlayer {
  pub shard_id: u32,
  pub player_id: u32,
  pub closer_id: u32,
}

#[derive(Debug, Serialize, Deserialize)]
#[serde(tag = "id", content = "data")]
pub enum ShardPacket {
  #[serde(rename = "1")]
  Authentication(ShardAuthentication),
  #[serde(rename = "2")]
  PlayerConnect(ShardPlayerConnect),
  // #[serde(rename = "3")]
  // PlayerDisconnect(ShardPlayerDisconnect),
  // #[serde(rename = "4")]
  // PlayerAddCloser(ShardPlayerAddCloser),
  // #[serde(rename = "5")]
  // PlayerRemovePlayer(ShardPlayerRemovePlayer),
}

#[derive(Debug)]
pub enum ShardPackets {
  Authentication(ShardAuthentication),
  PlayerConnect(ShardPlayerConnect),
  PlayerDisconnect(ShardPlayerDisconnect),
}

impl ShardPackets {
  pub fn parser(value: &[u8]) -> Result<Self, io::Error> {
    Ok(match value[1] {
      1 => ShardPackets::Authentication(deserialize(value)?),
      2 => ShardPackets::PlayerConnect(deserialize(value)?),
      3 => ShardPackets::PlayerDisconnect(deserialize(value)?),
      _ => panic!("Invalid packet id: {}", value[1]),
    })
  }
}