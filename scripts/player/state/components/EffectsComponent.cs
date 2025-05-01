using Godot;
using System;

public partial class EffectsComponent : StatePlayer
{
	private PackedScene _jumpDustScene; // Poeira do pulo
	private PackedScene _dashImpactScene; // Impacto ao ativar o dash

	private Marker2D _jumpDustPos; // Posição da poeira do pulo
	private Marker2D _dashImpactPos; // Posição do impacto do dash
	private float _dashImpactPosOffset; // Desvio da posição do impacto do dash quando espelhado

    public override void InitComponent()
    {
		_jumpDustScene = ResourceLoader.Load<PackedScene>("res://scenes/player/effects_player/jump_dust.tscn");
		_dashImpactScene = ResourceLoader.Load<PackedScene>("res://scenes/player/effects_player/dash_impact.tscn");

		_jumpDustPos = GetNode<Marker2D>("markers/jump_dust_pos");
		_dashImpactPos = GetNode<Marker2D>("markers/dash_impact_pos");
		_dashImpactPosOffset = 2 * (GlobalPosition.X - _dashImpactPos.GlobalPosition.X);

		Player.Landed += EmitJumpDust;
		Player.DashStarted += EmitDashImpact;
    }

	private void EmitJumpDust()
	{
		Particles jumpDust = (Particles)_jumpDustScene.Instantiate();

		jumpDust.GlobalPosition = _jumpDustPos.GlobalPosition;
		Player.AddSibling(jumpDust);
		jumpDust.Emitting = true;
	}

	private void EmitDashImpact()
	{
		Particles dashImpact = (Particles)_dashImpactScene.Instantiate();
		Vector2 position = _dashImpactPos.GlobalPosition;
	
		if (FlipH)
		{
			position.X += _dashImpactPosOffset;
			dashImpact.FlipDirection(true);
		};

		dashImpact.GlobalPosition = position;
		Player.AddSibling(dashImpact);
		dashImpact.Emitting = true;
	}
}
