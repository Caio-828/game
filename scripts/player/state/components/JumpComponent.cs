using Godot;
using System;

public partial class JumpComponent : StatePlayer
{
	[Export] private float JumpForce = -350.0f; // Força do pulo
	[Export] private float CoyoteTime = 0.1f; // Tempo de tolerância para pulo

	private float _coyoteTimer; // Contador do tempo de tolerância para pulo

	private Vector2 _velocity;

    public override void Update(float delta)
    {
		if (IsDashing || IsOnWall || IsWallJumping)
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
		bool canJump = PressedKeyJump &&
		_coyoteTimer < CoyoteTime &&
		!IsJumping;

		if (canJump)
		{
			IsJumping = true;
			_velocity.Y = JumpForce;
			_coyoteTimer = CoyoteTime;
		}

		// Mecânica de salto curto
		if (ReleasedKeyJump && _velocity.Y < 0 && IsJumping) _velocity.Y *= 0.5f;
	}
}
