using Godot;
using System;

public partial class GravityComponent : StatePlayer
{
	private Vector2 _velocity;
    public override void UpdateComponent(float delta)
    {
		if (IsDashing || IsOnLadder) return;

		_velocity = Player.Velocity;

		if (!Player.IsOnFloor())
		{
			ApplyGravity(delta);
			WasInAir = Player.StartInAir;
		}
		else if (WasInAir)
		{
			Player.EmitSignal("Landed");
			WasInAir = false;
		}
		else
		{
			Player.StartInAir = true;
		}

		Player.Velocity = _velocity;
    }

	private void ApplyGravity(float delta)
	{
		float variation = IsOnWater ? 1.5f : 1.0f;
		_velocity += Player.GetGravity() / variation * delta;
	}
}
