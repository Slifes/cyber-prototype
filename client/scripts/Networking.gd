extends Node

@onready var auth = $"../AuthClient"

var test_map: bool = true

# Called when the node enters the scene tree for the first time.
func _enter_tree():
	start_network()

func start_network() -> void:
	var multiplayer_api = MultiplayerAPI.create_default_interface()
	
	var peer = ENetMultiplayerPeer.new()
	peer.create_client("localhost", 4242)
	
	multiplayer_api.connected_to_server.connect(_connected)
	multiplayer_api.server_disconnected.connect(_disconnected)
	multiplayer_api.set_multiplayer_peer(peer)

	get_tree().set_multiplayer(multiplayer_api)

func _connected():
	var auth_token = "pGmmzfP3tYZybrYbFLr6SVJKVA4"

	if not test_map:
		auth_token = auth.SessionToken()
		
	print("Sending session token: ", auth_token)
	rpc_id(1, "onSessionMap", auth_token)# auth.SessionToken())

@rpc(any_peer)
func onSessionMap(_auth_token: String):
	print("auth_token: ", _auth_token)

func _disconnected():
	print("Finished")
