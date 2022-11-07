extends CharacterBody3D
class_name Player

func _ready():
	$Network/MultiplayerSynchronizer.set_multiplayer_authority(str(name).to_int())

@rpc
func Hello():
	print("Heelo")

@rpc(any_peer)
func Pong():
	print("ID: ", $Network/MultiplayerSynchronizer.get_multiplayer_authority())
	print("Pong: ", multiplayer.get_remote_sender_id())

var f = 0

func _process(delta):
	f = f + delta
	
	if f > 1:
		rpc("Hello")
		f = 0
