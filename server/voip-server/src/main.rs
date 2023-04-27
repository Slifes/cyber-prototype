use log::info;

mod shard;
mod client;

#[tokio::main]
async fn main() {
  env_logger::init();

  info!("Starting shard server...");

  let mut shard = shard::SHARD_SERVER.write().await;

  shard.run().await.unwrap();
}