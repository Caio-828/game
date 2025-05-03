using Godot;
using System;

public partial class ClimbLaddersComponent : StatePlayer
{
	[ExportGroup("Component settings")]
	[Export] private float MoveSpeed = 100.0f; // Velocidade de movimento subindo/descendo a escada

	private Map _map; // Node do mapa

    public override void InitComponent()
    {
        _map = Global.GetMap();
    }

    public override void UpdateComponent(float delta)
    {
		if (!OnLadderArea)
		{
			IsOnLadder = false;
			return;
		}

		if (Direction.Y != 0)
		{
			IsOnLadder = true;
			FlipH = false;
		}

		if (!IsOnLadder)
			return;
		
		Vector2 velocity = Player.Velocity;

		if (IsMovingV)
		{
			// Centraliza a posição do player no eixo x para alinhar a escada
			Vector2 centerPositionTile = TouchTileData.GlobalPosition;
			Player.GlobalPosition = new Vector2(centerPositionTile.X, Player.GlobalPosition.Y);	
		}

		// Adiciona o movimento do player subindo/descendo a escada
		velocity.Y = MoveSpeed * Direction.Y;

		Player.Velocity = velocity;
    }
}
