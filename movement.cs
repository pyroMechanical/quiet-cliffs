using Godot;
using System;

public partial class movement : CharacterBody3D
{
	public const float Speed = 3.0f;
	public const float RunSpeed = 5.0f;
	public const float JumpVelocity = 4.5f;
	public AnimationPlayer player;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();
	public override void _Ready() {
		player = GetNode<AnimationPlayer>("AnimationPlayer");
	}
	

	public override void _PhysicsProcess(double delta)
	{
		Vector3 velocity = Velocity;
		Vector3 rotation = Rotation;

		if (Position.Y < -50) {
			Position = new Vector3(0, 2, 0);
		}
		// Add the gravity.
		if (!IsOnFloor()) {
			velocity.Y -= gravity * (float)delta;
			if (velocity.Y > 2) {
				player.Play("Jump(Pose)", 1);
			}
			else {
				player.Play("Fall(Pose)", 1);
			}
		}

		// Handle Jump.
		if (Input.IsActionJustPressed("jump") && IsOnFloor())
			velocity.Y = JumpVelocity;

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		float inputDir = Input.GetAxis("move_forward", "move_back");
		float currentSpeed;
		if (Input.IsActionPressed("run")) {
			currentSpeed = RunSpeed;
		}
		else {
			currentSpeed = Speed;
		}
		if(IsOnFloor()) {
			if (inputDir == 0f) {
				player.Play("Idle", 1);
			}
			else if (Mathf.Abs(currentSpeed * inputDir) <= Speed) {
				player.Play("Walk", 1);
			}
			else {
				player.Play("SlowRun", 1);
			}
		}
		Vector3 direction = (Transform.Basis * new Vector3(0, 0, inputDir));
		float rotationDir = Input.GetAxis("strafe_right", "strafe_left");
		rotation.Y += rotationDir * 2 * (float) delta;
		if (direction != Vector3.Zero)
		{
			velocity.X = direction.X * currentSpeed;
			velocity.Z = direction.Z * currentSpeed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, currentSpeed);
			velocity.Z = Mathf.MoveToward(Velocity.Z, 0, currentSpeed);
		}

		Velocity = velocity;
		Rotation = rotation;
		MoveAndSlide();
	}
}
