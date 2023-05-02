use godot::prelude::*;
use godot::engine::{Engine, NodeVirtual, Node, AudioStreamGeneratorPlayback, AudioStreamPlayer3D};
use opus::{Decoder, Channels};

use super::manager::VoipManager;

const OPUS_FRAME_TIME: usize = 2880;

#[derive(GodotClass)]
#[class(base=Node)]
pub struct VoipSpeaker {
  id: i64,
  opus_packet: Vec<Vec<u8>>,
  decoded_frame: Vec<Vec<f32>>,
  audio_stream: Option<Gd<AudioStreamPlayer3D>>,

  #[base]
  base: Base<Node>,
}

#[godot_api]
impl VoipSpeaker {
  #[func]
  pub fn set_id(&mut self, id: i64) {
    self.id = id;
  }

  pub fn add_opus_packet(&mut self, frame: Vec<u8>) {
    self.opus_packet.push(frame);
  }

  fn decoder(&mut self, data: &Vec<u8>) {
    let output: &mut [f32] = &mut [0f32; OPUS_FRAME_TIME];

    let data = Decoder::new(48000, Channels::Mono)
      .unwrap()
      .decode_float(data, output, false);

    godot_print!("Decoded frame: {:?}", data);

    if let Ok(output_size) = data {
      self.decoded_frame.push(output[..output_size].to_vec());
    }
  }

  fn play_audio(&mut self)  {
    let audio_stream: &mut Gd<AudioStreamPlayer3D> = self.audio_stream.as_mut().unwrap();

    audio_stream.set("playing".into(), Variant::from(true));

    let mut playback: Gd<AudioStreamGeneratorPlayback> = audio_stream
      .get_stream_playback().unwrap()
      .try_cast().unwrap();

    godot_print!("Playing audio");

    for frame in &self.decoded_frame {
      for data in frame {
        playback.push_frame(Vector2::new(*data, *data));
      }
    }

    self.decoded_frame.clear();
  }
}

#[godot_api]
impl NodeVirtual for VoipSpeaker {
  fn init(base: Base<Node>) -> Self {
    VoipSpeaker {
      id: 0,
      base,
      audio_stream: None,
      opus_packet: Vec::new(),
      decoded_frame: Vec::new()
    }
  }

  fn ready(&mut self) {
    if Engine::singleton().is_editor_hint() {
      return;
    }

    self.audio_stream = Some(self.base.get_node_as("AudioStreamPlayer3D"));

    let name = self.base.get_parent().unwrap().get_parent().unwrap().get_parent().unwrap().get_name();

    godot_print!("Speaker ready: {}", name);

    self.set_id(name.to_string().parse().unwrap());

    self.base.get_node_as::<VoipManager>("/root/GlobalVoipManager")
      .bind_mut()
      .add_speaker(self.id, self.base.get_node_as::<VoipSpeaker>("."));
  }

  fn process(&mut self, _delta: f64) {
    if Engine::singleton().is_editor_hint() {
      return;
    }

    if self.opus_packet.len() > 0 {
      let packet = self.opus_packet.remove(0);
      
      self.decoder(&packet);
    }

    if self.decoded_frame.len() > 1 {
      self.play_audio();
    }
  }
}

impl Drop for VoipSpeaker {
  fn drop(&mut self) {
    godot_print!("Speaker dropped!");
  }
}