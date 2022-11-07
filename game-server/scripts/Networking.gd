extends Node

@onready var spawner = $MultiplayerSpawner;

# Called when the node enters the scene tree for the first time.
func _enter_tree():
	# Start the server if Godot is passed the "--server" argument,
	# and start a client otherwise.
	if "--server" in OS.get_cmdline_args():
		start_network(true)
	else:
		start_network(false)

func start_network(server: bool) -> void:
	var peer = ENetMultiplayerPeer.new()

	# Listen to peer connections, and create new player for them
	multiplayer.peer_connected.connect(self.create_player)
	# Listen to peer disconnections, and destroy their players
	multiplayer.peer_disconnected.connect(self.destroy_player)
	
	peer.create_server(4242)
	print('server listening on localhost 4242')

	multiplayer.set_multiplayer_peer(peer)

func create_player(id : int) -> void:
	# Instantiate a new player for this client.
	spawner.spawn(id)

func destroy_player(id : int) -> void:
	# Delete this peer's node.
	$Players.get_node(str(id)).queue_free()
