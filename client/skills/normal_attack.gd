extends Node

@onready var animation: AnimationPlayer = $Animation
# Called when the node enters the scene tree for the first time.
func _ready():
	animation.animation_finished.connect(_animation_finished)

func _animation_finished(name):
	queue_free()
	
func play_animation():
	animation.play()
