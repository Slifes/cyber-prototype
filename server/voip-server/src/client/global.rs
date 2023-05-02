use super::player::Client;

use tokio::sync::RwLock;
use std::sync::Arc;
use std::collections::HashMap;

use lazy_static::lazy_static;

lazy_static! {
  pub static ref CLIENTS: RwLock<HashMap<i32, Arc<RwLock<Client>>>> = RwLock::new(HashMap::new());
}
