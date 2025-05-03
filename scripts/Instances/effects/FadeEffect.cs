using Godot;
using System;

public partial class FadeEffect : Node2D
{
	[Signal] public delegate void FinishedEventHandler(); // Sinal ao acabar a transição
	private AnimationPlayer _animation; // Node da animação
	public override void _Ready()
	{
		_animation = GetNode<AnimationPlayer>("animation");	
		_animation.AnimationFinished += OnAnimationFinished;
	}
	
	public void FadeOut()
	{
		_animation.Play("fade_out");
	}

	private void OnAnimationFinished(StringName animName)
	{
		if (animName == "fade_out") EmitSignal("Finished");
	}

}
