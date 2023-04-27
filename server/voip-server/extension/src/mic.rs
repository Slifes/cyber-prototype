use godot::prelude::*;
use godot::engine::{
  Engine, AudioEffectCapture, AudioServer, InputEvent,
  AudioStreamPlayer, Node, NodeVirtual
};
use opus::{Encoder, Channels, Application};

const OPUS_FRAME_TIME: usize = 2880;

fn packed_vector_to_mono_pcm(samples: &Vec<Vector2>) -> Vec<f32> {
  let mut pcm_data = Vec::new();

  for i in 0..samples.len() {
    let left = samples[i];
    let mono_sample = (left.x + left.y) / 2.0;
    pcm_data.push(mono_sample);
  }

  pcm_data
}

#[derive(GodotClass)]
#[class(base=Node)]
pub struct VoipMicrophone {
  recording: bool,
  prev_recording: bool,

  frames: Vec<Vec<Vector2>>, 
  opus_packets: Vec<Vec<u8>>,
  
  stream: Option<Gd<AudioStreamPlayer>>,
  record_effect: Option<Gd<AudioEffectCapture>>,

  #[base]
  base: Base<Node>,
}

#[godot_api]
impl VoipMicrophone {
  pub fn get_latest_opus_packet(&mut self) -> Vec<Vec<u8>> {
    let mut packets = Vec::new();

    let len = self.opus_packets.len().min(3);

    for i in 0..len {
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

  pub fn _encode(&mut self, data: Vec<f32>) {
    let mut encoder = Encoder::new(48000, Channels::Mono, Application::Voip).unwrap();

    godot_print!("Data: {:?}", data.len());

    let result = encoder.encode_vec_float(&data, 1276);

    if let Err(x) = result {
      godot_print!("Error: {:?}", x);
    } else {
      let result_data = result.unwrap();
      godot_print!("Encoded: {:?}", result_data.len());

      self.opus_packets.push(result_data.clone());
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
impl NodeVirtual for VoipMicrophone {
  fn init(base: Base<Node>) -> Self {
    Self {
      recording: false,
      prev_recording: false,
      stream: None,
      record_effect: None,
      frames: Vec::new(),
      opus_packets: Vec::new(),
      base,
    }
  }

  fn ready(&mut self) {
    if Engine::singleton().is_editor_hint() {
      return;
    }

    godot_print!("Speaker is ready!");

    self.stream = Some(self.base
      .get_node_as("AudioStreamPlayer"));

    let audio_server = &mut AudioServer::singleton();
    let record_index = audio_server.get_bus_index("Record".into());

    self.record_effect = audio_server.get_bus_effect(record_index, 0).unwrap().try_cast();
  }

  fn input(&mut self, event: Gd<InputEvent>) {
    godot_print!("Speaker received input: {:?}", event);

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

        self._encode(mono_pcm);

        self.frames.remove(0);
      }
    }
  }
}