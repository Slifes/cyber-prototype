extends Node

@onready var wallet = $"../../Wallet"
@onready var step_sequence = [
	$STEP_QRCODE,
	$STEP_SIGN,
	$STEP_HEART
]

var current_step = -1
var current_node

# Called when the node enters the scene tree for the first time.
func _ready():
	_next_step()

func _next_step():
	if current_step != -1:
		current_node.hide()

	current_step += 1
	current_node = step_sequence[current_step]
	current_node.active(wallet)
	current_node.show()

func step_finished():
	_next_step()
