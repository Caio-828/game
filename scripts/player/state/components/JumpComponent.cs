using Godot;
using System;

public partial class JumpComponent : StatePlayer
{
	[ExportGroup("Component settings")]
	[Export] private float JumpForce = 350.0f; // Força do pulo
	[Export] private float CoyoteTime = 0.1f; // Tempo de tolerância para pulo

	private float _coyoteTimer; // Contador do tempo de tolerância para pulo

	private Vector2 _velocity;

    public override void UpdateComponent(float delta)
    {
		if (IsDashing || IsOnWall || IsWallJumping || IsOnLadder)
		{
			IsJumping = false;
			return;
		}
		
		_velocity = Player.Velocity;
		
		if (Player.IsOnFloor() || IsOnWall)
		{
			IsJumping = false;
			_coyoteTimer = 0;
		}
		else
		{
			_coyoteTimer += delta;
		}

		MainLogic();

		Player.Velocity = _velocity;
    }

	private void MainLogic()
	{
		if (CanJump())
		{
			IsJumping = true;
			_velocity.Y = -JumpForce;
			_coyoteTimer = CoyoteTime;
		}

		// Mecânica de salto curto
		if (ReleasedKeyJump && _velocity.Y < 0 && IsJumping) _velocity.Y *= 0.5f;
	}

	private bool CanJump()
	{
		if (!PressedKeyJump)
			return false;
		
		if (IsJumping)
			return false;

		if (_coyoteTimer >= CoyoteTime)
			return false;

		return true;
	}
}
