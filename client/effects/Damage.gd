extends Node3D

func _ready():
	$AnimationPlayer.animation_finished.connect(_animation_finished)

func run(damage: int, color: String):
	$Label3D.text = str(damage)
	$Label3D.modulate = color
	$AnimationPlayer.play("damage")

func _animation_finished(name):
	queue_free()
