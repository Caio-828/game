using Godot;
using System;

public partial class Camera : Camera2D
{
	private bool positionSmoothin; // Controle se o smoothing da câmera está ativado

	public override void _Ready()
	{
		positionSmoothin = PositionSmoothingEnabled;
		Global.Transition.Finished += OnAnimationFinished;

		if (string.IsNullOrEmpty(Global.Manager.FromScene)) return;
		
		PositionSmoothingEnabled = false;
	}

	private void OnAnimationFinished()
	{
		if (!IsInstanceValid(this) || !positionSmoothin) return;
		
		PositionSmoothingEnabled = true;
	}
}
