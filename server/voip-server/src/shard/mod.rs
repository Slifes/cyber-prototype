use std::sync::RwLock;
use lazy_static::lazy_static;

mod server;
mod shard_data;

lazy_static! {
  pub static ref SHARD_SERVER: RwLock<server::ShardServer> = RwLock::new(server::ShardServer::new());
}
