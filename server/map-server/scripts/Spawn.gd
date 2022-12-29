extends MultiplayerSpawner

var player = preload("res://actors/Player.tscn")

func _spawn_custom(id):
	var p = player.instantiate()
	
	p.name = str(id)
	
	return p
