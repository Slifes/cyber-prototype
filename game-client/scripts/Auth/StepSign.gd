extends Node2D

var _wallet

@onready var step_manager = get_parent()
@onready var button = $Control/Button

signal _on_authenticated()

func active(wallet):
	_wallet = wallet
	button.connect("pressed", _sign_pressed)	
	_on_authenticated.connect(_authenticated)

func _sign_pressed():
	_wallet.RequestSignature(self)

func _authenticated():
	step_manager.step_finished()
