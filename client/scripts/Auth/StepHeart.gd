extends Control

var _wallet

@onready var step_manager = get_parent()
@onready var auth = $"/root/AuthClient"
@onready var scene_manager = $"../../../SceneManager"
@onready var button = $AspectRatioContainer/Control/Button
@onready var char_container = $AspectRatioContainer/Control/ScrollContainer/VBoxContainer

signal _mint_txsended()
signal _receive_characters(characters: Array)
signal _session_map_created(token: String)

func active(wallet):
	_wallet = wallet
	button.connect("pressed", _mint_pressed)
	
	_mint_txsended.connect(_mint_tx)
	_receive_characters.connect(_characters)
	_session_map_created.connect(_session_map)
	
	auth.GetCharacters(self)

func _mint_pressed():
	_wallet.MintHeart(self)

func _mint_tx(tx_id):
	print("arrived")
	print(tx_id)
	goto_map()

func _characters(characters: Array):
	print(characters)
	
	for char in characters:
		print("token_id: " + char["token_id"])
		
		var d = func ():
			print(char["token_id"])
			_select_character(char["id"])
		
		var c = BoxContainer.new()
		var t = TextureButton.new()
		t.texture_normal = load("res://icon.svg")
		c.add_child(t)
		t.pressed.connect(d)
		
		char_container.add_child(c)

func _select_character(id):
	auth.CreateSessionMap(id, self);

func _session_map(token):
	print(token)
	goto_map()

func goto_map():
	scene_manager.ChangeState("world")
