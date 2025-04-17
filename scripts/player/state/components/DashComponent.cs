using Godot;
using System;

public partial class DashComponent : StatePlayer
{
	[Export] private float DashSpeed = 300.0f; // Velocidade do dash
	[Export] private float DashDuration = 0.3f; // Duração do dash
	[Export] private float DashCooldown = 0.5f; // Tempo de recarga entre dashes
	[Export] private float DashInSequence = 1; // Quantos dash em sequência podem ser lançados

	private bool _canDash = true; // Controle se pode realizar o dash
 	private float _dashTimer = 0f; // Contador de tempo de dash
	private float _dashCooldownTimer = 0f; // Contador de tempo de recarga
	private float _dashCounter = 0; // Contador de dash em sequência

	Vector2 _velocity;


    public override void Update(float delta)
    {
		_velocity = Player.Velocity;

		if (Player.IsOnFloor()) _dashCounter = 0;

		// Lògica do dash
		if (
			PressedKeyDash &&
			_canDash &&
			_dashCounter < DashInSequence) 
		{
			_dashCounter++;
			StartDash();
		}

		if (_dashCooldownTimer >= DashCooldown)
		{
			_canDash = true;
			_dashCooldownTimer = 0f;
		}

		if (IsDashing)
		{
			float direction = FlipH ? -1 : 1;
			_dashTimer += delta;
			_velocity.X = direction * DashSpeed;

		}

		if (!_canDash) _dashCooldownTimer += delta;

		if (_dashTimer >= DashDuration) EndDash();

		Player.Velocity = _velocity;
    }

	private void StartDash()
	{
		Player.EmitSignal("DashStarted");
		IsDashing = true;
		_canDash = false;
		_dashTimer = 0f;
		
		_velocity = Vector2.Zero;
	}
	private void EndDash()
	{
		Player.EmitSignal("DashFinished");
		IsDashing = false;
		_dashTimer = 0f;
	}
}
