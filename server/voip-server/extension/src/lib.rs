use godot::prelude::*;

mod mic;
mod client;
mod speaker;
mod manager;
struct VoipLibrary;

#[gdextension]
unsafe impl ExtensionLibrary for VoipLibrary {}