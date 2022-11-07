extends Node2D

var _wallet

@onready var _qrcode = $QRCode
@onready var step_manager = get_parent()

func active(wallet):
	_wallet = wallet
	_qrcode.texture.content = wallet.URI()
	_wallet.connect("WalletConnected", _step_qrcode_connected);

func _step_qrcode_connected():
	_wallet.disconnect("WalletConnected", _step_qrcode_connected)
	
	step_manager.step_finished()
