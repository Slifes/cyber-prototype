extends Node

var token: int = 0

@onready var viewport: SubViewport = $SubViewport

var time = 0

func _ready():
	load_data(3, {})

func _process(delta):
	time += delta
	
	if time > 1:
		_on_process_image()

func _on_process_image():
	save()
	get_tree().quit()

func load_data(token_id: int, player_equipment: Dictionary):
	token = token_id

# Called when the node enters the scene tree for the first time.
func save():
	viewport.get_texture().get_image().save_png(str(token) + ".png")
