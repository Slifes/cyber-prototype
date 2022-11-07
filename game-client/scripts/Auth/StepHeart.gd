extends Node2D

var _wallet

@onready var step_manager = get_parent()
@onready var button = $Control/Button

signal _mint_txsended()

func active(wallet):
	_wallet = wallet
	
	button.connect("pressed", _mint_pressed)	
	_mint_txsended.connect(_mint_tx)

func _mint_pressed():
	_wallet.MintHeart(self)

func _mint_tx(tx_id):
	print("arrived")
	print(tx_id)
	
	get_tree().change_scene_to_file("res://scenes/node.tscn")
