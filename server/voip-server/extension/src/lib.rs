use godot::prelude::*;

mod mic;
mod client;
mod speaker;

struct VoipLibrary;

#[gdextension]
unsafe impl ExtensionLibrary for VoipLibrary {}