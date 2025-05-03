using Godot;
using System;

public partial class WorldLimt : Node2D
{
	private Player _player; // Node do player

	private Marker2D _top_left; // Canto superior esquerdo
	private Marker2D _bottom_right; // Canto inferior direito

	public override void _Ready()
	{
		_player = Global.GetPlayer();

		_top_left = GetNode<Marker2D>("top_left");
		_bottom_right = GetNode<Marker2D>("bottom_right");
	}

	public override void _PhysicsProcess(double delta)
	{
		if (_player.GlobalPosition.X < _top_left.GlobalPosition.X ||
			_player.GlobalPosition.Y < _top_left.GlobalPosition.Y)
		{
			_player.Die();
		}
		else if (_player.GlobalPosition.X > _bottom_right.GlobalPosition.X ||
				_player.GlobalPosition.Y > _bottom_right.GlobalPosition.Y)
		{
			_player.Die();
		}
	}
}
