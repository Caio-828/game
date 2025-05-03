using Godot;
using System;
using System.Collections.Generic;

public partial class Map : Node2D
{
	public Dictionary<string, TileMapLayer> Layers = []; // Dicionário com todos as layer do mapa

	public override void _Ready()
	{
		Global.SetMap(this); // Atribuindo este mapa a variável global

		InitializeDictionaryLayers();
	}

	public Vector2I ConvertGlobalPosition(Vector2 globalPosition, string layerName = "terrain")
	{
		TileMapLayer tileMapLayer = Layers[layerName];

		// Converte global position em posição de tile
		Vector2 position = tileMapLayer.ToLocal(globalPosition);
		return tileMapLayer.LocalToMap(position);
	}

	public Vector2 CenterPositinTile(Vector2 globalPosition, string layerName = "terrain")
	{
		TileMapLayer tileMapLayer = Layers[layerName];

		// Retorna a cordenada central do tile da cordenada de entrada
		return tileMapLayer.MapToLocal(tileMapLayer.LocalToMap(globalPosition));
	}

	public Variant? CheckTileCustomData(string nameData, Vector2 globalPosition, string layerName = "terrain")
	{
		TileMapLayer tileMapLayer = Layers[layerName];

        Vector2I tileCoords = ConvertGlobalPosition(globalPosition, layerName);

        // Verifica se o tile tem propriedades customizadas
        TileData tileData = tileMapLayer.GetCellTileData(tileCoords);
        // Retorna nulo se o tile não tem propriedades customizadas
        if (tileData == null)
			return null;

        // Retorna o valor da propriedade
        if (tileData.HasCustomData(nameData))
			return tileData.GetCustomData(nameData);

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
