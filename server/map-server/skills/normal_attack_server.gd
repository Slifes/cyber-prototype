extends Node3D

@onready var area: Area3D = $Pivot/Body/Area3D

# Called when the node enters the scene tree for the first time.
func _ready():
	area.body_entered.connect(_on_hit)

func _on_hit(body: Node3D):
	print("Player hinted: ", body.name)

func _on_animation_animation_finished(anim_name):
	queue_free()
