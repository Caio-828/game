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
		string animation = "idle";

		if (IsOnLadder)
		{
			animation = "climp_ladder";
			if (!IsMovingV)
				_texture.Pause();
			else
				_texture.Play();
		}
		 else if (IsDashing)
		{
			animation = "dash";
		}
		else if (IsOnWall)
		{
			animation = "wall";
		}
		else if (IsJumping)
		{
			animation = "jump";
		}
		else if (IsSwimming)
		{
			animation = "swim";
		}
		else if (WasInAir)
		{
			animation = "falling";
		}
		else if (IsMovingH)
		{
			animation = "run";
		}

		_texture.Play(animation);
    }
}
