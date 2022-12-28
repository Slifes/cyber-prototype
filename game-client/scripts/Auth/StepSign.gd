extends Node2D

var _wallet

@onready var step_manager = get_parent()
@onready var button = $Control/Button


func active(wallet):
	_wallet = wallet
	button.connect("pressed", _sign_pressed)	

func _sign_pressed():
	_wallet.RequestSignature()
