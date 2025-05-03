using Godot;
using System;

public partial class CollisionPlayer : CollisionShape2D
{
	private Player _player; // Node do player
	public override void _Ready()
	{
		_player = GetParent<Player>();
		_player.ToggleCollision += OnToggleCollision;
	}

	// Sinais
	private void OnToggleCollision()
	{
		CallDeferred(nameof(UpdateCollisionState));
	}

	private void UpdateCollisionState()
	{
        if (Name == StatePlayer.CurrentCollision)
            Disabled = false;
        else
            Disabled = true;
	}
}
