extends Node

@onready var auth = $"../AuthClient"

# Called when the node enters the scene tree for the first time.
func _enter_tree():
	start_network()

func start_network() -> void:
	var multiplayer_api = MultiplayerAPI.create_default_interface()
	
	var peer = ENetMultiplayerPeer.new()
	peer.create_client("localhost", 4242)
	
	multiplayer_api.peer_connected.connect(_connected)
	multiplayer_api.peer_disconnected.connect(_disconnected)
	multiplayer_api.set_multiplayer_peer(peer)

	get_tree().set_multiplayer(multiplayer_api)

func _connected(id):
	print("Connected ", auth.SessionToken())

	rpc_id(1, "onSessionMap", auth.SessionToken())

@rpc(any_peer)
func onSessionMap(auth_token):
	pass

func _disconnected():
	print("Finished")
