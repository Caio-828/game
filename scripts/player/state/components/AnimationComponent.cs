using Godot;
using System;

public partial class AnimationComponent : StatePlayer
{
	private AnimatedSprite2D _texture;

    public override void Init()
    {
		_texture = GetNode<AnimatedSprite2D>("texture");
		_texture.FlipH = FlipH;
    }

    public override void Update(float delta)
    {
		_texture.FlipH = FlipH;

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

		if (IsJumping)
		{
			_texture.Play("jump");
			return;
		}

		if (!Player.IsOnFloor())
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
