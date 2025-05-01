using Godot;
using System;

public partial class DashComponent : StatePlayer
{
	[ExportGroup("Component settings")]
	[Export] private float DashSpeed = 300.0f; // Velocidade do dash
	[Export] private float DashDuration = 0.3f; // Duração do dash
	[Export] private float DashCooldown = 0.5f; // Tempo de recarga entre dashes
	[Export] private float DashInSequence = 1; // Quantos dash em sequência podem ser lançados

	private bool _canDash = true; // Controle se pode realizar o dash
 	private float _dashTimer = 0f; // Contador de tempo de dash
	private float _dashCooldownTimer = 0f; // Contador de tempo de recarga
	private float _dashCounter = 0; // Contador de dash em sequência

	Vector2 _velocity;

    public override void UpdateComponent(float delta)
    {
		if (IsOnLadder)
		{
			IsDashing = false;
			_dashTimer = 0f;
			return;
		}

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
		IsDashing = true;
		Player.EmitSignal("DashStarted");
		_canDash = false;
		_dashTimer = 0f;
		
		_velocity = Vector2.Zero;
	}
	private void EndDash()
	{
		IsDashing = false;
		Player.EmitSignal("DashFinished");
		_dashTimer = 0f;
	}
}
