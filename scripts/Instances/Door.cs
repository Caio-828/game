using Godot;
using System;

public partial class Door : Node2D
{
	private Sprite2D _texture; // Textura da porta
	private CollisionShape2D _collision; // Node da colisão da porta fechada
	private Area2D _area; // Node da área que detecta o player ao se aproximar

	public override void _Ready()
	{
		_texture = GetNode<Sprite2D>("texture");
		_collision = GetNode<CollisionShape2D>("static_body/collision");
		_area = GetNode<Area2D>("area");

		_area.BodyEntered += OnBodyEntered;
		_area.BodyExited += OnBodyExited;
	}

	public override void _PhysicsProcess(double delta)
	{
	}

	// Sinais
	private void OnBodyEntered(Node2D body)
	{
		if (body is not Player)
			return;

		_texture.Frame = 1;
		_collision.SetDeferred("disabled", true);
	}

	private void OnBodyExited(Node2D body)
	{
		if (body is not Player)
			return;
		
		_texture.Frame = 0;
		_collision.SetDeferred("disabled", false);
	}
}
