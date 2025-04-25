using Godot;
using System;
using System.Collections.Generic;

public partial class Map : Node2D
{
	public Dictionary<string, TileMapLayer> Layers = []; // Dicionário com todos as layer do mapa

	public override void _Ready()
	{
		Global.Manager.SetMap(this); // Atribuindo este mapa a variável global

		InitializeDictionaryLayers();
	}

	public Vector2 ConvertGlobalPosition(Vector2 globalPosition, string layer = "terrain")
	{
		TileMapLayer tileMapLayer = Layers[layer];

		// Converte global position em posição de tile
		return tileMapLayer.LocalToMap(tileMapLayer.ToLocal(globalPosition));
	}

	public Vector2 CenterPositinTile(Vector2 globalPosition, string layer = "terrain")
	{
		TileMapLayer tileMapLayer = Layers[layer];

		// Retorna a cordenada central do tile da cordenada de entrada
		return tileMapLayer.MapToLocal(tileMapLayer.LocalToMap(globalPosition));
	}

	public Variant? CheckTileCustomData(string nameData, Vector2 globalPosition, string layer = "terrain")
	{
		TileMapLayer tileMapLayer = Layers[layer];

        Vector2 tileCoords = ConvertGlobalPosition(globalPosition);

        // Verifica se o terrain tem propriedades customizadas
        TileData tileData = tileMapLayer.GetCellTileData((Vector2I)tileCoords);
        // Retorna nulo pois o tile não tem propriedade customizadas
        if (tileData == null) return null;

		// Retorna o valor da propriedade
		if (tileData.HasCustomData(nameData))
			return (bool)tileData.GetCustomData(nameData);

		// Retorna nulo pois a propriedade não existe
		return null;
	}

	private void InitializeDictionaryLayers()
	{
		foreach (Node node in GetChildren())
		{
			if (node is TileMapLayer layer)
				Layers.Add(node.Name, layer);
		}
	}
}
