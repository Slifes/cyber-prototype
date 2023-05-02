use std::io;

use serde::{Deserialize, Serialize};
use rmp_serde::{Deserializer, Serializer};

pub fn deserialize<T> (buf: &[u8]) -> Result<T, io::Error> where for<'de> T: Deserialize<'de> {
  let mut de = Deserializer::new(buf);
  Deserialize::deserialize(&mut de)
    .map_err(|x| io::Error::new(io::ErrorKind::Other, x))
}

fn serialize<T> (packet: T) -> Result<Vec<u8>, io::Error> where for<'de> T: Serialize {
  let mut buf = Vec::new();
  packet.serialize(&mut Serializer::new(&mut buf))
    .map_err(|x| io::Error::new(io::ErrorKind::Other, x))?;
  Ok(buf)
}

pub trait PacketParser<T> where for<'de> T: Serialize + Deserialize<'de> {
  fn serialize(value: T) -> Result<Vec<u8>, io::Error> {
    serialize(value)
  }

  fn deserialize(value: &[u8]) -> Result<T, io::Error> {
    deserialize(value)
  }
}