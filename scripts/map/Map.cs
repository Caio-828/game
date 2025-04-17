using Godot;
using System;

public partial class Map : Node2D
{
	public TileMapLayer Terrain { get; private set; } // Layer principal (Terreno do mapa)
	public override void _Ready()
	{
		Global.Manager.SetMap(this); // Atribuindo este mapa a variável global

		if (HasNode("terrain"))
			Terrain = GetNode<TileMapLayer>("terrain");
	}

	public bool CheckTileCanWallJump(Vector2 globalPosition)
	{
		if (Terrain == null) return false;

        // Converte para cordenada de tile
        Vector2 tileCoords = Terrain.LocalToMap(Terrain.ToLocal(globalPosition));

        // Verifica se o terrain tem a propriedade can_wall_jump
        TileData tileData = Terrain.GetCellTileData((Vector2I)tileCoords);
        // Retorna pois a propriedade não existe
        if (tileData == null) return false;
        // Também retorna se for false
        if (!(bool)tileData.GetCustomData("can_wall_jump")) return false;

		// Retorna true por que a propriedade existe e está ativada
		return true;
	}
}
