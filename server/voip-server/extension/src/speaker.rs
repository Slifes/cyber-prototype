use godot::prelude::*;
use godot::engine::{Engine, Time, Sprite3D, Node3DVirtual, Node3D, AudioStreamGeneratorPlayback, AudioStreamPlayer3D};
use opus::{Decoder, Channels};

use super::manager::VoipManager;

const OPUS_FRAME_TIME: usize = 2880;

#[derive(GodotClass)]
#[class(base=Node3D)]
pub struct VoipSpeaker {
  id: i64,
  opus_packet: Vec<Vec<u8>>,
  decoded_frame: Vec<Vec<f32>>,
  audio_stream: Option<Gd<AudioStreamPlayer3D>>,
  sprite: Option<Gd<Sprite3D>>,
  decoder: Decoder,

  #[base]
  base: Base<Node3D>,
}

#[godot_api]
impl VoipSpeaker {
  #[func]
  pub fn set_id(&mut self, id: i64) {
    self.id = id;
  }

  #[func]
  pub fn on_finished_audio(&mut self) {
    self.sprite.as_mut().unwrap().set_visible(false);
  }

  pub fn add_opus_packet(&mut self, frame: Vec<u8>) {
    self.opus_packet.push(frame);
  }

  fn decode(&mut self, data: &Vec<u8>) {
    let output: &mut [f32] = &mut [0f32; OPUS_FRAME_TIME];

    let data = self.decoder.decode_float(data, output, false);

    godot_print!("Decoded frame: {:?}", data);

    if let Ok(output_size) = data {
      self.decoded_frame.push(output[..output_size].to_vec());
    }
  }

  fn play_audio(&mut self)  {
    let audio_stream: &mut Gd<AudioStreamPlayer3D> = self.audio_stream.as_mut().unwrap();

    self.sprite.as_mut().unwrap().set_visible(true);

    if !audio_stream.is_playing() {
      audio_stream.set("playing".into(), Variant::from(true));
    }

    let mut playback: Gd<AudioStreamGeneratorPlayback> = audio_stream
      .get_stream_playback().unwrap()
      .try_cast().unwrap();

    if playback.can_push_buffer(i64::try_from(OPUS_FRAME_TIME).unwrap()) {
      let mut frames = PackedVector2Array::new();

      let len = self.decoded_frame.len().min(2);

      for _ in 0..len {
        let frame_f32 = self.decoded_frame.remove(0);

        for frame in frame_f32 {
          frames.push(Vector2::new(frame, frame));
        }
      }      

      playback.push_buffer(frames);
    }
  }
}

#[godot_api]
impl Node3DVirtual for VoipSpeaker {
  fn init(base: Base<Node3D>) -> Self {
    let decode = Decoder::new(48000, Channels::Mono).unwrap();
    VoipSpeaker {
      id: 0,
      base,
      audio_stream: None,
      sprite: None,
      decoder: decode,
      opus_packet: Vec::new(),
      decoded_frame: Vec::new(),
    }
  }

  fn ready(&mut self) {
    if Engine::singleton().is_editor_hint() {
      return;
    }

    let mut audio_stream = self.base.get_node_as::<AudioStreamPlayer3D>("AudioStreamPlayer3D");

    audio_stream.connect("finished".into(), Callable::from_object_method(self.base.get_node_as::<VoipSpeaker>("."), "on_finished_audio"), 0);

    self.audio_stream = Some(audio_stream);

    self.sprite = Some(self.base.get_node_as("Speaker"));

    let name = self.base.get_parent().unwrap().get_parent().unwrap().get_parent().unwrap().get_name();

    godot_print!("Speaker ready: {}", name);

    self.set_id(name.to_string().parse().unwrap());

    self.base.get_node_as::<VoipManager>("/root/GlobalVoipManager")
      .bind_mut()
      .add_speaker(self.id, self.base.get_node_as::<VoipSpeaker>("."));
  }

  fn process(&mut self, _delta: f64) {
    // let time = Time::singleton().get_ticks_msec();

    if Engine::singleton().is_editor_hint() {
      return;
    }

    if self.opus_packet.len() > 0 {
      let packet = self.opus_packet.remove(0);
      
      self.decode(&packet);
    }

    if self.decoded_frame.len() > 2 {
      self.play_audio();
    }

    // godot_print!("Time playing audio: {:?}", Time::singleton().get_ticks_msec() - time);
  }
}

impl Drop for VoipSpeaker {
  fn drop(&mut self) {
    // self.base.get_node_as::<VoipManager>("/root/GlobalVoipManager")
    //   .bind_mut()
    //   .remove_speaker(self.id);
  }
}