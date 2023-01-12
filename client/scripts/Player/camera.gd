extends Camera3D

@export
var noise: FastNoiseLite

@export
var trauma: float = 0.0

@export var max_x = 5
@export var max_y = 5
@export var max_r = 25
@export var time_scale = 150

var time: float

# Called when the node enters the scene tree for the first time.
func _ready():
	pass # Replace with function body.


func add_trauma(value):
	trauma = value


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	time += delta

	var shake = pow(trauma, 2)
	var offsetX = noise.get_noise_3d(time * time_scale, 0, 0) * max_x * shake
	var offsetY = noise.get_noise_3d(0, time * time_scale, 0) * max_y * shake
	
	self.h_offset = offsetX
	self.v_offset = offsetY
	# rotation_degrees = (noise.get_noise_3d(0, 0, time * time_scale) * max_r * shake).
	# self.position = Vector3(offsetX, offsetY, self.position.z)
	
	if trauma > 0:
		trauma = clamp(trauma - (delta * 0.6), 0, 1)
