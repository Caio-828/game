using Godot;
using System;

public partial class ToggleScene : Area2D
{
	[ExportGroup("Basic settings")]
	[Export] private string _sceneFolder; // Pasta que contém a cena de destino
	[Export] private bool _needPressKey = false; // Controle se prescisa pressionar pra interagir
	private enum Directions {Left = -1, Every = 0, Right = 1};
	[Export] private Directions _diretionToInteract = Directions.Every; // Lado específico para interagir

	[ExportGroup("Dialog")]
	[Export] private PackedScene _dialogScene = null; // Caixa de diálogo
	[Export] private float _dialogScale = 2.5f; // Escala da caixa de diálogo

	[ExportGroup("State player at enter")]
	[Export] public bool StartInAir {get; private set;} = false; // Controle se começa no ar ao chegar
	[Export] public bool StartFlipH {get; private set;} = false; // Controle se deve espelhar ao chegar

	private ConfigScene _configScene; // Node pai da cena
	private Marker2D _dialogPos; // Posição para caixa de diálogo
	private Dialog _dialog = null; // Referência para o dialog

	private bool _playerInArea = false; // Controle se o player esta na área

	public override void _Ready()
	{
		_configScene = (ConfigScene)GetTree().CurrentScene;

		if (_dialogScene != null) _dialogPos = _configScene.GetNode<Marker2D>($"scene_points/{Name}_dialog_pos");

		_sceneFolder ??= _configScene.SceneFolder;
	}
	public override void _Process(double delta)
	{
		if (!_playerInArea)
		{
			RemoveDialog();
			return;
		}

		bool ?flipH = _diretionToInteract != 0 ? _diretionToInteract < 0 : StatePlayer.FlipH;
		bool isCorrectFlip = StatePlayer.FlipH == flipH;

		bool canChangeScene = (
			Input.IsActionJustPressed("interact") || !_needPressKey) &&
			isCorrectFlip &&
			_playerInArea &&
			!Global.Transition.IsRunning();

		if (canChangeScene)
		{
			Global.Transition.Start("fade");
			ChangeScene();
		}		

		DrawDialog(isCorrectFlip);
	}

	private async void ChangeScene()
	{ 
		try
		{
			await ToSignal(Global.Transition, "Peak"); // Espera a transição chegar ao pico

			string path = GetScenePath(); // Pega o caminho da cena de destino
			if (path == "") return;

			Global.FromScene = _configScene.Name; // Atualiza o FromScene

			_configScene.QueueFree(); // Liberando a memória da cena atual
			GetTree().ChangeSceneToFile(path);
		}
		catch (Exception e)
		{
			GD.PrintErr($"Erro ao mudar de cena: {e.Message}");
		}
	}
	private void DrawDialog(bool isCorrectDirection)
	{
		if (_dialogScene == null) return;

		if (!isCorrectDirection)
		{
			RemoveDialog();
			return;
		}

		if (_dialog != null) return;

		_dialog = (Dialog)_dialogScene.Instantiate();
		_configScene.AddChild(_dialog);
		_dialog.Init(_dialogPos.GlobalPosition, _dialogScale);
	}

	private void RemoveDialog()
	{
		if (_dialog == null) return;

		_dialog.RemoveFromQueue();
		_dialog = null;
	}

	private string GetScenePath()
	{
		string path = $"res://scenes/places/{_sceneFolder}/{Name}.tscn";

		if (!ResourceLoader.Exists(path))
		{
			GD.PrintErr("Cena de destino não foi encontrada");
			return "";
		}
		return path;
	}

	// Sinais
	private void _on_body_entered(Node2D body)
	{ // Detecta quando o player entra na área
		if (body is Player) _playerInArea = true;
	}
	private void _on_body_exited(Node2D body)
	{ // Detecta quando o player sai da área
		if (body is Player) _playerInArea = false;
	}
}
