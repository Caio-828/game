using Godot;
using System;

public partial class MovementComponent : StatePlayer
{
	[Export] private float MoveSpeed = 150.0f; // Velocidade de movimento

	public override void Update(float delta)
	{
		if (IsDashing || IsWallJumping) return;

		Vector2 velocity = Player.Velocity;

		// Lógica da movimentação
		if (Direction.X != 0)
		{
			velocity.X = Direction.X * MoveSpeed; // Move o player
		}
		else
		{
			velocity.X = Mathf.MoveToward(velocity.X, 0, MoveSpeed); // Freia de forma gradual
		}

		Player.Velocity = velocity;
	}
}
