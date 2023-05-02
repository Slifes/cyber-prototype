use godot::prelude::*;
use godot::engine::{
  Engine, AudioEffectCapture, AudioServer, InputEvent,
  AudioStreamPlayer, AudioStreamPlayerVirtual
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
  frames: Vec<Vec<Vector2>>, 
  opus_packets: Vec<Vec<u8>>,
  record_effect: Option<Gd<AudioEffectCapture>>,

  #[base]
  base: Base<AudioStreamPlayer>,
}

#[godot_api]
impl VoipMicrophone {
  #[signal]
  fn broadcast();

  pub fn get_latest_opus_packet(&mut self) -> Vec<Vec<u8>> {
    let mut packets = Vec::new();

    let len = self.opus_packets.len().min(3);

    for _ in 0..len {
      packets.push(self.opus_packets.remove(0));
    }

    packets
  }

  fn recording(&mut self) {
    if self.recording {
      let capture = self.record_effect.as_mut().unwrap();

      if self.prev_recording {

        let frames = capture.get_frames_available();
    
        if frames > 0 {
          let buffer: Vec<Vector2> = capture.get_buffer(frames).to_vec();
    
          self.add_frame(buffer);
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

    if let Err(x) = result {
      godot_print!("Error: {:?}", x);
    } else {
      let result_data = result.unwrap();

      godot_print!("Encoded: {:?}", result_data.len());

      self.opus_packets.push(result_data);
    }
  }

  fn add_frame(&mut self, buffer: Vec<Vector2>) {
    for buff in buffer.chunks(OPUS_FRAME_TIME) {
      let frame = self.frames.last_mut();

      if let Some(last_frame) = frame {
        if last_frame.len() != OPUS_FRAME_TIME {
          let frame_missing = OPUS_FRAME_TIME - last_frame.len();

          let frame_difference = if buff.len() <= frame_missing {
            buffer.len()
          } else {
            frame_missing
          };
        
          last_frame.append(buffer[..frame_difference].to_vec().as_mut());
          
          if frame_difference != buffer.len() {
            self.frames.push(buff[(buff.len() - frame_missing)..].to_vec());
          }
        } else {
          self.frames.push(buff.to_vec());
        }
      } else {
        self.frames.push(buff.to_vec());
      }
    }
  }
}  

#[godot_api]
impl AudioStreamPlayerVirtual for VoipMicrophone {
  fn init(base: Base<AudioStreamPlayer>) -> Self {
    let encoder = Encoder::new(48000, Channels::Mono, Application::Voip).unwrap();
    Self {
      recording: false,
      prev_recording: false,
      record_effect: None,
      frames: Vec::new(),
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
    self.record_effect = audio_server.get_bus_effect(record_index, 0).unwrap().try_cast();
  }

  fn input(&mut self, event: Gd<InputEvent>) {
    if event.is_action_pressed("voip_active".into(), false, true) {
      godot_print!("Pressed voip_active");
      self.recording = true;

    } else if event.is_action_released("voip_active".into(), true) {
      godot_print!("Released voip_active");

      self.recording = false;
      self.prev_recording = false;
    }
  }

  fn process(&mut self, delta: f64) {
    if Engine::singleton().is_editor_hint() {
      return;
    }

    self.recording();
    
    let last_frame = self.frames.first_mut();

    if let Some(frame) = last_frame {
      if frame.len() == OPUS_FRAME_TIME {

        let mono_pcm = packed_vector_to_mono_pcm(frame);

        self.encode(mono_pcm);

        self.frames.remove(0);
      }
    }

    if self.opus_packets.len() > 2 {
      self.emit_signal("broadcast".into(), &[]);
    }
  }
}