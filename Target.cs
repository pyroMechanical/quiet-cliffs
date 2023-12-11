using Godot;
using System;

public partial class Target : Marker3D
{
	public bool updatePos = false;
	public Vector2 ClickPos;
	
	public override void _Ready()
	{
	}
	
	public override void _Input(InputEvent @event) {
		if(@event is InputEventMouseButton mouseEvent && mouseEvent.Pressed) {
			if (mouseEvent.ButtonIndex == MouseButton.Left) {
				updatePos = true;
				ClickPos = mouseEvent.Position;
			}
		}
	}
	public override void _PhysicsProcess(double delta)
	{
		if (updatePos) {
			updatePos = false;
			Camera3D camera = GetViewport().GetCamera3D();
			Vector3 from = camera.ProjectRayOrigin(ClickPos);
			Vector3 to = from + camera.ProjectRayNormal(ClickPos) * 1000.0f;
			var space = GetWorld3D().DirectSpaceState;
			var query = PhysicsRayQueryParameters3D.Create(from, to);
			var result = space.IntersectRay(query);
			Position = (Vector3) result["position"];
		}
	}
}
