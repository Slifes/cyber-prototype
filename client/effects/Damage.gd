extends Node3D

func _ready():
	$AnimationPlayer.animation_finished.connect(_animation_finished)

func run(damage: int):
	$Label3D.text = str(damage)
	$AnimationPlayer.play("damage")

func _animation_finished(name):
	queue_free()
