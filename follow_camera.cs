using Godot;
using System;

public partial class follow_camera : Node3D
{
	[Export]
	public const float VSpeed = 2;
	[Export]
	public const float HSpeed = 2;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Vector3 rotation = Rotation;
		float vertical = Input.GetAxis("camera_down", "camera_up");
		float horizontal = Input.GetAxis("camera_left", "camera_right");
		rotation.Z += vertical * VSpeed * (float) delta;
		if (rotation.Z <= Mathf.DegToRad(-90)) rotation.Z = Mathf.DegToRad(-90);
		if (rotation.Z >= Mathf.DegToRad(0)) rotation.Z = Mathf.DegToRad(0);
		rotation.Y += horizontal * HSpeed * (float) delta;
		Rotation = rotation;
	}
}
