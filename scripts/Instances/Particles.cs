using Godot;
using System;

public partial class Particles : GpuParticles2D
{
	public override void _Ready()
	{
		Finished += QueueFree;
	}

	public void FlipDirection(bool flipH = false, bool flipV = false)
	{
		Scale *= new Vector2(flipH ? -1 : 1, flipV ? -1 : 1);
	}
}
