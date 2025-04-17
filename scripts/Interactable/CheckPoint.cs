using Godot;
using System;

public partial class CheckPoint : Node2D
{
	private Area2D _area; // Node da área 2d
	public override void _Ready()
	{
		_area = GetNode<Area2D>("area");
	}

	// Sinais
	private void _on_area_body_entered(Node body)
	{
		if (body is Player) Global.Manager.CheckPoint = this;
	}
}
