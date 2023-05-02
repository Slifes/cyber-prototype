use log::info;

mod shard;
mod client;

#[tokio::main]
async fn main() {
  env_logger::init();

  info!("Starting shard server...");

  let mut shard = shard::server::ShardServer::new();

  let mut client_server = client::server::ClientServer::new();

  let result = tokio::join!(
    shard.run(),
    client_server.run()
  );

  info!("Server closed: {:?}", result);
} 