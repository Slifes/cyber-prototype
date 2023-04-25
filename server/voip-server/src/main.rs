mod shard;
mod client;

#[tokio::main]
async fn main() {
    let mut shard = shard::SHARD_SERVER.write().unwrap();

    println!("Starting shard server...");

    shard.run().await.unwrap();
}