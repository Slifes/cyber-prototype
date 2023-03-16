extends Control

var _wallet

@onready var step_manager = get_parent()
@onready var auth = $"/root/AuthClient"
@onready var scene_manager = $"/root/SceneManager"
@onready var char_container = $MarginContainer/Control/ScrollContainer/VBoxContainer
@onready var selector = $Selector
@onready var empty_character = $EMPTY_CHARACTER

# Buttons
@onready var mint_btn = $MarginContainer/HBoxContainer/Mint
@onready var create_btn = $MarginContainer/HBoxContainer/Create
@onready var enter_world_btn = $MarginContainer/HBoxContainer/Enter

var character_list: Array = []
var character_selected: int

signal _receive_characters(characters: Array)
signal _session_map_created(token: String)

func active(wallet):
	_wallet = wallet

	enter_world_btn.pressed.connect(_enter_world)
	create_btn.pressed.connect(_character_creator)
	
	_receive_characters.connect(_characters)
	_session_map_created.connect(_session_map)
	
	selector.Load(self)


func _characters(characters: Array):
	print(characters)
	
	character_list = characters
	
	for char in characters:
		var on_click = func ():
			print(char["id"])
			_select_character(char["id"])
		
		var container: TextureButton = empty_character.duplicate()
		container.visible = true
		container.pressed.connect(on_click)

		char_container.add_child(container)

func _select_character(id: int):
	character_selected = id
	
	auth.SetTokenSelected(id)
	
	for char in character_list:
		if id == char["id"]:
			if "character" in char:
				create_btn.visible = false
				enter_world_btn.visible = true
			else:
				enter_world_btn.visible = false
				create_btn.visible = true
			break
	

func _enter_world():
	selector.CreateSession(character_selected, self)


func _character_creator():
	goto_character_creator()


func _session_map(token):
	print(token)
	goto_map()


func goto_map():
	scene_manager.ChangeState("world")


func goto_character_creator():
	scene_manager.ChangeState("character_creator")
