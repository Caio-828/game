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
	private void OnToggleCollision(string collision)
	{
		if (Name == collision)
			Disabled = false;
		else
			Disabled = true;
	}
}
