using Godot;
using System;

public partial class ClimbLaddersComponent : StatePlayer
{
	[Export] private float MoveSpeed = 100.0f; // Velocidade de movimento subindo/descendo a escada

	private Map _map; // Node do mapa

    public override void Init()
    {
        _map = Global.Manager.GetMap();
    }

    public override void Update(float delta)
    {
		Variant? isLadder = _map.CheckTileCustomData("is_ladder", Player.GlobalPosition);
		OnLadderArea = isLadder != null && (bool)isLadder;

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
			Vector2 centerPositionTile = _map.CenterPositinTile(Player.GlobalPosition);
			Player.GlobalPosition = new Vector2(centerPositionTile.X, Player.GlobalPosition.Y);	
		}

		// Adiciona o movimento do player subindo/descendo a escada
		velocity.Y = MoveSpeed * Direction.Y;

		Player.Velocity = velocity;
    }
}
