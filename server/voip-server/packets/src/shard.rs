use serde::{Serialize, Deserialize};

#[derive(Debug, Serialize, Deserialize)]
pub struct ShardAuthentication {
  pub packet_id: u32,
  pub shard_id: u32,
  pub shard_key: String,
}

#[derive(Debug, Serialize, Deserialize)]
pub struct ShardPlayerConnect {
  pub shard_id: u32,
  pub player_id: u32,
}

#[derive(Debug, Serialize, Deserialize)]
pub struct ShardPlayerDisconnect {
  pub shard_id: u32,
  pub player_id: u32,
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