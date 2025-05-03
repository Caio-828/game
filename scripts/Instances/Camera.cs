using Godot;
using System;

public partial class Camera : Camera2D
{
	private Player _player; // Node do player
	private bool _positionSmoothin; // Controle se o smoothing da câmera está ativado

	public override void _Ready()
	{
		_positionSmoothin = PositionSmoothingEnabled;

		_player = Global.GetPlayer();
		_player.Died += OnPlayerDied;
		Global.Transition.Finished += OnAnimationFinished;

		if (!string.IsNullOrEmpty(Global.FromScene))
			PositionSmoothingEnabled = false;	
	}

	// Sinais
	private void OnPlayerDied()
	{
		if (IsInstanceValid(this) && _positionSmoothin)
			PositionSmoothingEnabled = false;
	}

	private void OnAnimationFinished()
	{
		if (IsInstanceValid(this) && _positionSmoothin)
			PositionSmoothingEnabled = true;
	}
}
