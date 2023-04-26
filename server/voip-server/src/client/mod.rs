pub mod player;
pub mod server;

use tokio::sync::RwLock;
use std::sync::Arc;
use std::collections::HashMap;

use lazy_static::lazy_static;

lazy_static! {
  pub static ref CLIENTS: RwLock<HashMap<i32, Arc<RwLock<player::Client>>>> = RwLock::new(HashMap::new());
}