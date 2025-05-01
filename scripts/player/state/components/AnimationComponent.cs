using Godot;
using System;

public partial class AnimationComponent : StatePlayer
{
	private AnimatedSprite2D _texture;

    public override void InitComponent()
    {
		_texture = GetNode<AnimatedSprite2D>("texture");
		_texture.FlipH = FlipH;
    }

    public override void UpdateComponent(float delta)
    {
		_texture.FlipH = FlipH;

		if (IsOnLadder)
		{
			_texture.Play("climp_ladder");
			if (!IsMovingV)_texture.Pause();
			return;
		}

		if (IsDashing)
		{
			_texture.Play("dash");
			return;
		}

		if (IsOnWall)
		{
			_texture.Play("wall");
			return;
		}

		if (IsSwimming)
		{
			_texture.Play("swim");
			return;
		}

		if (IsJumping)
		{
			_texture.Play("jump");
			return;
		}

		if (WasInAir)
		{
			_texture.Play("falling");
			return;
		}

		if (IsMovingH)
		{
			_texture.Play("run");
			return;
		}

		_texture.Play("idle");
    }
}
