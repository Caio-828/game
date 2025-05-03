using Godot;
using System;
using System.Linq;

public partial class InteractiveTileDetectorComponent : StatePlayer
{
	private Map _map; // Node do mapa
	private Area2D _area; // Node da área
	private Node2D _markersParent; // Node que contém todos os markers

	private string _typeTile = null; // Tipo do tile a ser tocado

	public override void InitComponent()
	{
		_map = Global.GetMap();
		_area = GetNode<Area2D>("area");

		_area.BodyEntered += OnAreaBodyEntered;
		_area.BodyExited += OnAreaBodyExited;

		_markersParent = GetNode<Node2D>("markers");
	}

	// Sinais
	private void OnAreaBodyEntered(Node2D body)
	{	
		if (body is not TileMapLayer tileMapLayer)
			return;
			
		_typeTile = null;

		CheckMarkers(tileMapLayer);

		if (_typeTile != null)
			Player.EmitSignal("TileAreaEntered", _typeTile); // Emitindo sinal que entrou na área de um tile interativo
	}

	private void OnAreaBodyExited(Node2D body)
	{
		if (body is not TileMapLayer)
			return;

		if (TouchTileData == TouchTileData.Empty)
			return;

		Player.EmitSignal("TileAreaExited", TouchTileData.Type); // Emitindo sinal que saiu da área de um tile interativo

		// Resetando dados do tile interativo a ser tocado no momento
		TouchTileData = TouchTileData.Empty;
	}

	private void CheckMarkers(TileMapLayer tileMapLayer)
	{
		// Verifica a posição de cada marker para descobrir onde está o tile interativo
		foreach (Marker2D marker in _markersParent.GetChildren().Cast<Marker2D>())
		{
			string type = (string)_map.CheckTileCustomData("typeTile", marker.GlobalPosition, tileMapLayer.Name);

			if (type == null)
				continue;
			
			// Ao encontrar a posição do tile interativo, atualiza o _typeTile
			_typeTile = type;
			// Obtém o global position do ponto central do tile interativo
			Vector2 globalPositionTile = _map.CenterPositinTile(marker.GlobalPosition, tileMapLayer.Name);

			// Salva os dados do tile interativo para permitir a utilização em outros componentes do player
			TouchTileData = new(tileMapLayer.Name, _typeTile, globalPositionTile);
			break;
		}
	}
}
