use std::io;

use serde::Deserialize;
use rmp_serde::Deserializer;

pub fn deserialize<T> (buf: &[u8]) -> Result<T, io::Error> where for<'de> T: Deserialize<'de> {
  let mut de = Deserializer::new(buf);
  Deserialize::deserialize(&mut de)
    .map_err(|x| io::Error::new(io::ErrorKind::Other, x))
}

