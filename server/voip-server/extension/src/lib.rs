use godot::prelude::*;

mod speaker;

struct VoipLibrary;

#[gdextension]
unsafe impl ExtensionLibrary for VoipLibrary {}