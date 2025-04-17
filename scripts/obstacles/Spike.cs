using Godot;
using System;

public partial class Spike : Area2D
{
	// Sinais
	private void _on_body_entered(Node2D body)
	{
		if (body is Player player) player.Manager.Die();
	}
}
