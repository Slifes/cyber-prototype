use std::collections::HashMap;
use std::sync::Arc;

use tokio::sync::RwLock;

use super::peers::peer::Peer;

pub type Peers = Arc<RwLock<HashMap<i32, Arc<RwLock<Peer>>>>>;