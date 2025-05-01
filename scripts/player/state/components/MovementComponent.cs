using Godot;
using System;

public partial class MovementComponent : StatePlayer
{
	[ExportGroup("Component settings")]
	[Export] private float MovementSpeed = 150.0f; // Velocidade de movimento

	public override void UpdateComponent(float delta)
	{
		if (IsDashing || IsWallJumping) return;

		Vector2 velocity = Player.Velocity;

		// Lógica de movimentação
		if (Direction.X != 0)
		{
			velocity.X = Direction.X * MovementSpeed; // Move o player
		}
		else
		{
			velocity.X = Mathf.MoveToward(velocity.X, 0, MovementSpeed); // Freia de forma gradual
		}

		Player.Velocity = velocity;
	}
}
