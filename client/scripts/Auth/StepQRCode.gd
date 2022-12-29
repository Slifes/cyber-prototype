extends Node2D

var _wallet

@onready var _qrcode = $Control/CenterContainer/QRCode
@onready var step_manager = get_parent()

func active(wallet):
	_wallet = wallet
	_wallet.ConnectToWallet()
	_wallet.connect("WalletQRCode", _print_qrcode)
	_wallet.connect("WalletConnected", _step_qrcode_connected);

func _step_qrcode_connected():
	_wallet.disconnect("WalletConnected", _step_qrcode_connected)
	step_manager.step_finished()

func _print_qrcode(qrcode):
	_qrcode.visible = true
	_qrcode.texture.content = qrcode
