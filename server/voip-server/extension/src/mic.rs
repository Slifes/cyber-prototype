use godot::prelude::*;
use godot::engine::{
  Engine, Time, AudioEffectCapture, AudioServer, InputEvent,
  AudioStreamPlayer
};
use opus::{Encoder, Channels, Application};

const OPUS_FRAME_TIME: usize = 2880;
 
fn packed_vector_to_mono_pcm(samples: &Vec<Vector2>) -> Vec<f32> {
  samples.iter()
    .map(|x| (x.x + x.y) / 2.0)
    .collect::<Vec<f32>>()
}

#[derive(GodotClass)]
#[class(base=AudioStreamPlayer)]
pub struct VoipMicrophone {
  recording: bool,
  prev_recording: bool,
  encoder: Encoder,
  opus_packets: Vec<Vec<u8>>,
  record_effect: Option<Gd<AudioEffectCapture>>,
  frame: Vec<Vector2>,

  #[base]
  base: Base<AudioStreamPlayer>,
}

#[godot_api]
impl VoipMicrophone {
  pub fn get_opus_packet_count(&self) -> usize {
    self.opus_packets.len()
  }

  pub fn get_latest_opus_packet(&mut self) -> Vec<Vec<u8>> {
    let len = self.opus_packets.len().min(2);
    let mut packets = Vec::with_capacity(len);

    for _ in 0..len {
      packets.push(self.opus_packets.remove(0));
    }

    packets
  }

  fn process_frame(&mut self, buffer: &[Vector2]) {
    let frame_len = self.frame.len();
    let frame_missing = OPUS_FRAME_TIME - frame_len;
    let buffer_to_collect = frame_missing.min(buffer.len());

    if frame_missing > 0 {
      self.frame.extend(buffer[..buffer_to_collect].iter());
    }

    if self.frame.len() == OPUS_FRAME_TIME {
      let mono_pcm = packed_vector_to_mono_pcm(&self.frame);

      self.encode(mono_pcm);

      self.frame.clear();
    }

    if (buffer.len() - buffer_to_collect) > 0 {
      self.frame.extend(buffer[buffer_to_collect..].iter());
    }
  }

  fn recording(&mut self) {
    if self.recording {
      let capture = self.record_effect.as_mut().unwrap();

      if self.prev_recording {
        let frames = capture.get_frames_available();
    
        if frames > 0 {
          let buffer: Vec<Vector2> = capture.get_buffer(frames).to_vec();

          for buff in buffer.chunks(OPUS_FRAME_TIME) {
            self.process_frame(buff);
          }
        }
  
      } else {
        self.prev_recording = true;
        
        capture.clear_buffer();
      }
    }
  }

  fn encode(&mut self, data: Vec<f32>) {
    godot_print!("Data: {:?}", data.len());

    let result = self.encoder.encode_vec_float(&data, 1276);

    match result {
      Ok(result_data) => {
        godot_print!("Encoded: {:?}", result_data.len());
  
        self.opus_packets.push(result_data);
      },
      Err(x) => {
        godot_print!("Error: {:?}", x);
      }
    }
  }
}  

#[godot_api]
impl IAudioStreamPlayer for VoipMicrophone {
  fn init(base: Base<AudioStreamPlayer>) -> Self {
    let encoder = Encoder::new(48000, Channels::Mono, Application::Voip).unwrap();
    Self {
      recording: false,
      prev_recording: false,
      record_effect: None,
      frame: Vec::with_capacity(OPUS_FRAME_TIME),
      opus_packets: Vec::new(),
      encoder,
      base,
    }
  }

  fn ready(&mut self) {
    if Engine::singleton().is_editor_hint() {
      return;
    }

    godot_print!("Speaker is ready!");

    let audio_server = &mut AudioServer::singleton();
    let record_index = audio_server.get_bus_index("Record".into());
    self.record_effect = match audio_server.get_bus_effect(record_index, 0).unwrap().try_cast() {
      Ok(effect) => Some(effect),
      Err => None
    };
  }

  fn input(&mut self, event: Gd<InputEvent>) {
    if event.is_action_pressed("voip_active".into()) {
      godot_print!("Pressed voip_active");
      self.recording = true;

    } else if event.is_action_released("voip_active".into()) {
      godot_print!("Released voip_active");

      self.recording = false;
      self.prev_recording = false;
      self.frame.clear();
    }
  }

  fn process(&mut self, _delta: f64) {
    // let time = Time::singleton().get_ticks_msec();

    if Engine::singleton().is_editor_hint() {
      return;
    }

    self.recording();

    // godot_print!("Time recording audio: {:?}", Time::singleton().get_ticks_msec() - time);
  }
}