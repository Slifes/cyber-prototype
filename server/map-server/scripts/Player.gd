extends CharacterBody3D
class_name Player

@onready var aabb: Area3D = $AABB

@export
var near_players = {}

func _ready():
	global_position = $Network.position
	$Network/MultiplayerSynchronizer.set_multiplayer_authority(str(name).to_int())
	
	aabb.body_entered.connect(_body_entered)
	aabb.body_exited.connect(_body_exited)

@rpc(any_peer, unreliable)
func ReceiveState(position):
	if name == str(multiplayer.get_remote_sender_id()):
		global_position = position

func _body_entered(body):
	if body.name != name:
		near_players[body.name] = body

func _body_exited(body):
	near_players.erase(body.name)
