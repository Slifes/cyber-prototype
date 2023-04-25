mod shard;
mod client;

#[tokio::main]
async fn main() {
    let mut shard = shard::ShardServer::new();

    println!("Hello, world!");

    shard.run().await.unwrap();
}