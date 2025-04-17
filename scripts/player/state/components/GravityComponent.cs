using Godot;
using System;

public partial class GravityComponent : StatePlayer
{
    public override void Update(float delta)
    {
		if (IsDashing) return;

		// Aplicando gravidade
		if (!Player.IsOnFloor())
		{
			Player.Velocity += Player.GetGravity() * delta;
			WasInAir = Player.StartInAir;
			return;
		}
		
		if (WasInAir)
		{
			Player.EmitSignal("Landed");
			WasInAir = false;
			return;
		}

		Player.StartInAir = true;
    }
}
