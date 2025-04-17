using Godot;
using System;

public partial class Dialog : Node2D
{
	private FadeEffect fadeEffect;

	public override void _Ready()
	{
		if (HasNode("fade_effect"))
		{
			fadeEffect = GetNode<FadeEffect>("fade_effect");
			fadeEffect.Finished += QueueFree;
		}
	}

	public void Init(Vector2 GlobalPosition, float Scale)
	{
		this.GlobalPosition = GlobalPosition;

		Camera2D camera = GetViewport().GetCamera2D();
		this.Scale = new Vector2(1f / camera.Zoom.X, 1f / camera.Zoom.Y) * Scale;
	}

	public void RemoveFromQueue()
	{
		if (fadeEffect != null)
		{
			fadeEffect.FadeOut();
			return;
		};

		QueueFree();
	}
}
