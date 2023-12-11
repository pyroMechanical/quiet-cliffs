using Godot;
using System;

public partial class zombie : CharacterBody3D
{
	[Export]
	float Speed = 1.5f;
	public const float Acceleration = 2.0f;
	public float AttackTimer = 0.0f;
	public float AttackThreshold = 2.0f;
	bool IsAttacking = false;
	public AnimationPlayer player;
	NavigationAgent3D nav;
	[Export]
	Node3D target;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();

	public override void _Ready() {
		player = GetNode<AnimationPlayer>("AnimationPlayer");
		nav = GetNode<NavigationAgent3D>("NavigationAgent3D");
		player.AnimationFinished += OnAnimationFinished;
	}

	private void OnAnimationFinished(StringName animationName) {
		if(animationName == "Attack1") {
			IsAttacking = false;
			AttackTimer = 0;
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		
		Vector3 velocity = Velocity;
		nav.TargetPosition = target.GlobalPosition;
		Vector3 direction = (nav.GetNextPathPosition() - GlobalPosition);
		LookAt(nav.GetNextPathPosition());
		if(direction.Length() > .03) {
			direction = direction.Normalized();
		}
		if(Position.DistanceTo(target.GlobalPosition) > nav.TargetDesiredDistance) {
			AttackTimer = 0.0f;
			velocity = direction * Speed;
			Velocity = velocity;
		}
		else {
			Velocity = new Vector3(0, 0, 0);
			AttackTimer += (float) delta;
		}
		MoveAndSlide();

		if (!IsAttacking) {
			if(AttackTimer >= AttackThreshold) {
				player.Play("Attack1", 1);
				IsAttacking = true;
			}
			else if(Velocity.Length() > 0.05){
				player.Play("Walk", 1, 2.0f);
			}
			else {
				Velocity = new Vector3(0, 0, 0);
				player.Play("Idle", 1);
			}
		}
	}
}
