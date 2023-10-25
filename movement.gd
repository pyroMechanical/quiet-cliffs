extends CharacterBody3D


const SPEED = 3
const RUN_SPEED = 5
const JUMP_VELOCITY = 4.5
var player

# Get the gravity from the project settings to be synced with RigidBody nodes.
var gravity = ProjectSettings.get_setting("physics/3d/default_gravity")
func _ready():
	player = get_node("AnimationPlayer")

func _physics_process(delta):
	if position.y < -50:
		position = Vector3(0, 2, 0)
	# Add the gravity.
	if not is_on_floor():
		velocity.y -= gravity * delta
		if velocity.y > 2:
			player.play("Jump(Pose)", 1)
		else:
			player.play("Fall(Pose)", 1)
	# Handle Jump.
	if Input.is_action_just_pressed("jump") and is_on_floor():
		velocity.y = JUMP_VELOCITY
		

	# Get the input direction and handle the movement/deceleration.
	# As good practice, you should replace UI actions with custom gameplay actions.
	var input_dir = Input.get_axis("move_forward", "move_back")
	var current_speed;
	if Input.is_action_pressed("run"):
		current_speed = RUN_SPEED
	else:
		current_speed = SPEED
	if is_on_floor():
		if input_dir == 0:
			player.play("Idle", 1)
		elif abs(current_speed * input_dir) <= SPEED:
			player.play("Walk", 1, abs(input_dir))
		else:
			player.play("SlowRun", 1, abs(input_dir))
	var direction = (transform.basis * Vector3(0, 0, input_dir))
	var rotation_dir = Input.get_axis("strafe_right", "strafe_left")
	rotation.y += rotation_dir * 2 * delta
	if direction:
		velocity.x = direction.x * current_speed
		velocity.z = direction.z * current_speed
	else:
		velocity.x = move_toward(velocity.x, 0, current_speed)
		velocity.z = move_toward(velocity.z, 0, current_speed)

	move_and_slide()
