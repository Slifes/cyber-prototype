extends Node

@export
var PlayerScene = load("res://Player.tscn")

# Called when the node enters the scene tree for the first time.
func _enter_tree():
	start_network()

func start_network() -> void:
	var multiplayer_api = MultiplayerAPI.create_default_interface()
	
	var peer = ENetMultiplayerPeer.new()
	peer.create_client("localhost", 4242)
	
	multiplayer_api.peer_disconnected.connect(_disconnected)
	multiplayer_api.set_multiplayer_peer(peer)

	get_tree().set_multiplayer(multiplayer_api)

func _disconnected():
	print("Finished")
