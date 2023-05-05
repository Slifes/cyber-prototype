use log::info;

mod shard;
mod peers;
mod server;
mod types;

#[tokio::main]
async fn main() {
  env_logger::init();

  info!("Starting voip server...");

  let server = server::Server::new();

  server.run().await;

  info!("Server closed");
} 