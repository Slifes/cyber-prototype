extends Node


# Called when the node enters the scene tree for the first time.
func _ready():
	var args = OS.get_cmdline_args()
	
	if "shard" in args:
		var zone = args[1]
		get_tree().change_scene_to_file("res://zones/%s.tscn" % zone)
	else:
		get_tree().change_scene_to_file("res://scenes/world_sharded.tscn")
