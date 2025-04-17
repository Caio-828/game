using Godot;
using System;

public partial class ConfigScene : Node2D
{
	[Export] public string SceneFolder {get; private set;} // Pasta geral para todos ToggleScene da cena

	private Player _player; // Node do player

	private PackedScene _mainCheckPointScene; // Cena do check point
	private CheckPoint _mainCheckPoint; // Referência para cena do check point

	public override void _Ready()
	{
		_player = Global.Manager.GetPlayer();

		UpdatePlayerState();

		// Sistema de check point e respanw só adicionado caso exista o node check_points
		if (HasNode("check_points"))
		{
			// Adiciona um novo check point na cena para quardar a posição que o player entrou
			_mainCheckPointScene = ResourceLoader.Load<PackedScene>("res://scenes/Interactable/check_point.tscn");
			_mainCheckPoint = (CheckPoint)_mainCheckPointScene.Instantiate();
			GetTree().CurrentScene.GetNode<Node2D>("check_points").AddChild(_mainCheckPoint);

			_mainCheckPoint.GlobalPosition = _player.GlobalPosition; // Guarda a posição que o player iniciou
			
			Global.Manager.CheckPoint = _mainCheckPoint; // Reseta o ultimo check point pois mudou de cena

			_player.Died += PlayerRespawn; // Ação quando o player morrer
		}
	}
	private void UpdatePlayerState()
	{
		if (string.IsNullOrEmpty(Global.Manager.FromScene)) return; // Retorna sem fazer nada caso FromScene esteja vazio
		
		Marker2D marker = GetNode<Marker2D>($"scene_points/{Global.Manager.FromScene}_pos"); // Pega a posição que o player deve spawnar

		// Atualiza variáveis de estaso do player definido pelo toggleScene
		ToggleScene toggleScene = GetNode<ToggleScene>($"scene_points/{Global.Manager.FromScene}");
        _player.StartFlipH = toggleScene.StartFlipH;
		_player.StartInAir = toggleScene.StartInAir;

		_player.Position = marker.GlobalPosition; // Mandando player para a posição de spawn correta
	}

	private void PlayerRespawn()
	{
		_player.GlobalPosition = Global.Manager.CheckPoint.GlobalPosition; // Atualiza a posição do player para o último check point
	}
}
