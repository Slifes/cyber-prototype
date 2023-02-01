extends Node

@onready var auth = $"../AuthClient"
@onready var latency_text: Label = $UI/Network/Ping
@onready var packet_loss_text: Label = $UI/Network/Loss

var test_map: bool = true
var client_clock: int = 0
var delta_latency: float = 0
var decimal_collector: float = 0
var server_peer: ENetPacketPeer

# Called when the node enters the scene tree for the first time.
func _enter_tree():
	start_network()

func start_network() -> void:
	var multiplayer_api = MultiplayerAPI.create_default_interface()
	
	var peer = ENetMultiplayerPeer.new()
	peer.create_client("44.211.197.19", 4242) #44.211.197.19
	
	multiplayer_api.connected_to_server.connect(_connected)
	multiplayer_api.server_disconnected.connect(_disconnected)
	multiplayer_api.set_multiplayer_peer(peer)

	get_tree().set_multiplayer(multiplayer_api)

func _connected():
	server_peer = multiplayer.multiplayer_peer.get_peer(1)
	
	var auth_token = "pGmmzfP3tYZybrYbFLr6SVJKVA4"
	var now = Time.get_unix_time_from_system() * 1000.0
	
	sync_server_time()
	
	if not test_map:
		auth_token = auth.SessionToken()
		
	print("Sending session token: ", auth_token)
	rpc_id(1, "onSessionMap", auth_token)# auth.SessionToken())


func sync_server_time():
	var now = Time.get_unix_time_from_system() * 1000.0
	rpc_id(1, "FetchServerTime", now)
	
	# var timer: Timer = Timer.new()
	# timer.wait_time = 0.5
	# timer.autostart = true
	# timer.timeout.connect(determine_latency)
	
	# add_child(timer)

@rpc("any_peer")
func onSessionMap(_auth_token: String):
	print("auth_token: ", _auth_token)

@rpc("any_peer", "reliable")
func FetchServerTime(client_time):
	pass

@rpc("reliable")
func ReturnServerTime(server_time: float, client_time: float):
	var now = Time.get_unix_time_from_system() * 1000.0
	var latency = (now - client_time) / 2
	client_clock = server_time + latency

	# print("latency: ", latency)
	print("click_clock: ", client_clock)
	pass

func determine_latency():
	# rpc_id(1, "CheckLatency", Time.get_unix_time_from_system() * 1000)
	pass

@rpc("any_peer", "reliable")
func CheckLatency(client_time):
	pass
	
@rpc("reliable")
func ReturnLatency(client_time: float):
	# var now = Time.get_unix_time_from_system() * 1000
	# latency_array.append((now - client_time) / 2)
	
	# if latency_array.size() == 9:
	# 	var total_latency = 0
	# 	latency_array.sort()
	# 	var mid_point = latency_array[4]
	# 	for i in range(latency_array.size() -1, -1, -1):
	# 		if latency_array[i] > (2 * mid_point) and latency_array[i] > 20:
	# 			latency_array.remove_at(i)
	# 		else:
	# 			total_latency += latency_array[i]
	# 	latency = total_latency / latency_array.size()
	# 	latency_array.clear()
		
	# 	latency_text.text = "Latencia: %.02f" % latency
	pass

func _disconnected():
	print("Finished")
	get_tree().quit()

func _physics_process(delta):
	if server_peer:
		latency_text.text = "Ping: %s ms" % str(server_peer.get_statistic(ENetPacketPeer.PEER_ROUND_TRIP_TIME))
		packet_loss_text.text = "Loss: %s" % str(server_peer.get_statistic(ENetPacketPeer.PEER_PACKET_LOSS))

	client_clock += int(delta * 1000) + delta_latency
	delta_latency -= delta_latency
	decimal_collector += (delta * 1000) - int(delta * 1000)
	if decimal_collector >= 1.0:
		client_clock += 1
		decimal_collector -= 1.00
