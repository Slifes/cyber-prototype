use godot::prelude::*;
use godot::engine::{Node, NodeVirtual};

use std::collections::HashMap;

use crate::speaker::VoipSpeaker;

#[derive(GodotClass)]
#[class(base=Node)]
pub struct VoipManager {
  #[base]
  base: Base<Node>,
  speakers: HashMap<i64, Gd<VoipSpeaker>>,
}

#[godot_api]
impl VoipManager {
  pub fn add_speaker(&mut self, id: i64, speaker: Gd<VoipSpeaker>) {
    self.speakers.insert(id, speaker);
    godot_print!("Added speaker: {}", id);
    godot_print!("Speakers: {:?}", self.speakers);
  }

  pub fn remove_speaker(&mut self, id: i64) {
    self.speakers.remove(&id);
  }

  pub fn on_speak_data(&mut self, id: i64, data: Vec<Vec<u8>>) {
    godot_print!("Received data: {:?}, {:?}", id, data.len());
    if let Some(speaker) = self.speakers.get_mut(&id) {
      godot_print!("Found speaker: {}", id);
      let mut speaker_mut: GdMut<VoipSpeaker> = speaker.bind_mut();
      for d in data {
        speaker_mut.add_opus_packet(d);
      }
    }
  }
}

#[godot_api]
impl NodeVirtual for VoipManager {
  fn init(base: Base<Node>) -> Self {
    Self {
      base,
      speakers: HashMap::new(),
    }
  }
}