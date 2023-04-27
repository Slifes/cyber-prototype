use godot::engine::audio_stream_wav::Format;
use godot::prelude::*;
use godot::engine::{Engine, AudioEffectCapture, AudioServer, InputEvent, AudioStreamPlayer, Node, NodeVirtual, AudioEffectRecord};
use opus::{Decoder, Encoder, Channels, Application};

#[derive(GodotClass)]
#[class(base=Node)]
pub struct Speaker {
  volume: f32,
  pitch_scale: f32,
  bus: String,
  time: f64,
  
  stream: Option<Gd<AudioStreamPlayer>>,
  record_effect: Option<Gd<AudioEffectRecord>>,
  //generator: Option<Gd<AudioStreamGeneratorPlayback>>,

  #[base]
  base: Base<Node>,
}

#[godot_api]
impl Speaker {
  fn pcm8_to_pcm16(pcm8: &[u8]) -> Vec<i16> {
    let mut pcm16 = Vec::with_capacity(pcm8.len() * 2);
    for &sample8 in pcm8.iter() {
      let sample16 = (sample8 as i16 - 128) << 8;
      pcm16.push(sample16);
    }
    pcm16
  }

  fn packed_vector_to_mono_pcm(&self, samples: &Vec<Vector2>) -> Vec<i16> {
    let mut pcm_data = Vec::new();

    for i in 0..samples.len() / 2 {
      let left = samples[i * 2];
      let right = samples[i * 2 + 1];
      let mono_sample = ((left.x + right.x) / 2.0 * i16::MAX as f32) as i16;
      pcm_data.push(mono_sample);
    }

    pcm_data
  }

  pub fn _encode(&mut self, data: Vec<f32>) {
    let mut encoder = Encoder::new(48000, Channels::Mono, Application::Voip).unwrap();

    godot_print!("Decoded: {:?}", data.len());

    let result = encoder.encode_vec_float(&data, 1024);

    if let Err(x) = result {
      godot_print!("Error: {:?}", x);
    } else {
      let result_data = result.unwrap();
      godot_print!("Encoded: {:?}", result_data.len());
      
      // self._decoder(&result_data);
    }
  }

  fn _decoder(&mut self, data: &Vec<u8>) {
    let mut streamer: Gd<AudioStreamPlayer> = self.base
      .get_node_as("AudioStreamPlayer2");

    // streamer.set_stream(AudioStreamGenerator::new().try_cast().unwrap());
    
    // let mut playback: Gd<AudioStreamGeneratorPlayback> = streamer.get_stream_playback().unwrap().try_cast().unwrap();

    let output: &mut [i16] = &mut [0; 8000];

    let data = Decoder::new(48000, Channels::Mono)
      .unwrap()
      .decode(data, output, false);

    let output_size = data.unwrap();
    
    godot_print!("Decoded: {:?}", output_size);

    streamer.set("playing".into(), Variant::from(true));

    // let mut playback: Gd<AudioStreamGeneratorPlayback> = streamer.get_stream_playback().unwrap().try_cast().unwrap(); 
  }
}

fn pcm16_to_float(pcm_data: &[u8]) -> Vec<f32> {
  let mut float_data = Vec::with_capacity(pcm_data.len() / 2);
  for i in (0..pcm_data.len()).step_by(2) {
      let sample = i16::from_le_bytes([pcm_data[i], pcm_data[i + 1]]);
      float_data.push(sample as f32 / 32767.0);
  }
  float_data
}

#[godot_api]
impl NodeVirtual for Speaker {
  fn init(base: Base<Node>) -> Self {
    Self {
      time: 0f64,
      volume: 1.0,
      pitch_scale: 1.0,
      bus: String::from("Master"),
      stream: None,
      record_effect: None,
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

    godot_print!("Effect: {:?}", self.record_effect);

    self.record_effect.as_mut().unwrap().set_format(Format::FORMAT_8_BITS);
  }

  fn input(&mut self, event: Gd<InputEvent>) {
    godot_print!("Speaker received input: {:?}", event);

    if event.is_action_pressed("voip_active".into(), false, true) {
      godot_print!("Pressed voip_active");

      self.record_effect.as_mut().unwrap().set_recording_active(true);


      // self.stream.as_mut().unwrap().play(0.0);
    } else if event.is_action_released("voip_active".into(), true) {
      godot_print!("Released voip_active");
      let capture = self.record_effect.as_mut().unwrap();

      capture.set_recording_active(false);

      if !capture.is_recording_active() {
        
        let buffer: Vec<u8> = capture.get_recording().unwrap().get_data().to_vec();
      
        // godot_print!("HEUHEUEHUE: {:?}", pcm16_to_float(&buffer[..]));

        let pcm_data = pcm16_to_float(&buffer[..]);

        self._encode(pcm_data);
      }
    }
  }

  // fn process(&mut self, delta: f64) {
  //   if Engine::singleton().is_editor_hint() {
  //     return;
  //   }

  //   let capture = self.record_effect.as_mut().unwrap();

  //   if !capture.is_recording_active() {
      
  //     let buffer: Vec<u8> = capture.get_recording().unwrap().get_data().to_vec();

  //     let pcm_data = buffer.iter().map(|&sample| (sample as i16 - 128) << 8).collect::<Vec<i16>>();

  //     self._encode(pcm_data);
  //   }
  // }
}