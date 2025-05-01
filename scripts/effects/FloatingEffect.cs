using Godot;
using System;

public partial class FloatingEffect : Node2D
{
	[Export] private float Amplitude = 2.0f;
	[Export] private float Frequency = 0.5f;
	private Vector2 _initialPosition;

    public override void _Ready()
    {
        _initialPosition = Position;
    }

	public override void _PhysicsProcess(double delta)
	{
        float time = Time.GetTicksMsec() / 1000.0f;
        float offset = Amplitude * Mathf.Sin(time * Mathf.Pi * 2.0f * Frequency);

		Position = new(_initialPosition.X, _initialPosition.Y + offset);
	}
}
