using Godot;
using System;

public partial class SwimComponent : StatePlayer
{
	[ExportGroup("Component settings")]
	[Export] private float MovementSpeed = 100.0f; // Velocidade de movimento
	[Export] private float SwimmingForce = 200.0f; // Força da natação

	private bool _wasSwimming = false; // Controle para detectar a mudança de IsSwimming

	private Vector2 _velocity;
	public override void UpdateComponent(float delta)
	{
		if (!IsOnWater || IsDashing)
		{
			IsSwimming = false;
			UpdateWasSwimming();
			return;
		}

		_velocity = Player.Velocity;

		HandleSwimming();
		HandleMovement();
		UpdateWasSwimming();

		Player.Velocity = _velocity;
	}

	private void HandleSwimming()
	{
		if (Player.IsOnFloor())
			IsSwimming = false;
		else
			IsSwimming = true;

		if (PressedKeyJump)
			_velocity.Y = -SwimmingForce;
	}

	private void HandleMovement()
	{
		if (Direction.X != 0)
		{
			_velocity.X = MovementSpeed * Direction.X;
		}
	}

	private void UpdateWasSwimming()
	{
		if (_wasSwimming != IsSwimming)
		{
			_wasSwimming = IsSwimming;
			Player.EmitSignal("ToggleCollision");
		}
	}
}
