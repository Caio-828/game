using Godot;
using System;
using System.Collections.Generic;

public abstract partial class StatePlayer : Node2D
{
	[Export] public bool Disable { get; set; } = false; // Controle se o componente está desabilitado
	
	// statics ----------------------------------------------------------------------------------------------------------------------------------------------------------------------------
	protected static Player Player { get; private set; } // Node do player
	protected static PlayerManager Manager { get; private set; } // Gerênciador dos componentes

	// Input ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
	public static Vector2 Direction { get; protected set; } // Vector com entrada do teclado (Cima, Baixo, Direita, Esquerda)

	public static bool PressedKeyJump { get; protected set; } // Controle de quando a tecla de jump é pressionada
	public static bool ReleasedKeyJump { get; protected set; } // Controle de quando a tecla de jump é solta

	public static bool PressedKeyDash { get; protected set; } // Controle de quando a tecla de dash é pressionada

	// Variáveis de controle -----------------------------------------------------------------------------------------------------------------------------------------------------------------
	public static bool CanInput { get; protected set; } = true; // Cotrole aceita comandos de inputs

	public static bool FlipH { get; protected set; } = false; // Controle se está flipado
	public static bool WasInAir { get; protected set; } = false; // Controle se está no ar

	public static bool IsJumping { get; protected set; } = false; // Controle se está pulando
	public static bool IsWallJumping { get; protected set; } = false; // Controle se está pulando apartir da parede
	public static bool IsDashing { get; protected set; } = false; // Controle se está dashando
	public static bool IsSwimming { get; protected set; } = false; // Controle se está nadando

	public static bool IsOnWater { get; protected set; } = false; // Controle se está na água

	public static bool IsOnWall { get; protected set; } = false; // Controle se está na parede

	public static bool OnLadderArea { get; protected set; } = false; // Controle se está na área de uma escada
	public static bool IsOnLadder { get; protected set; } = false; // Controle se está subindo uma escada
	
	// Com set privado
	public static bool IsMovingH { get; private set; } = false; // Controle se está movendo na horizontal
	public static bool IsMovingV { get; private set; } = false; // Controle se está movendo no vertical

	// Outros ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
	public static string CurrentCollision { get; private set; } = "default_collision"; // Indicador da colisão atual do player

	public static TouchTileData TouchTileData { get; protected set; } = TouchTileData.Empty; // Dados do tile interativo a ser tocado no momento

	// ==========================================================================================================================================================================================

	// Chamada do gerenciador para iniciar o componente
	// Também pode ser usado no lugar de InitComponent para ignorar o Disable
	// Obs*: Não funciona caso o node do manager estaja com o Disable como true
	public virtual void Init()
	{
		if (Disable)
			return;
		
		InitComponent();
	}

	// Chamada do gerenciador para atualizar o componente
	// (Também pode ser usado no lugar de UpdateComponent para ignorar o Disable)
	// Obs*: Não funciona caso o node do manager estaja com o Disable como true
	public virtual void Update(float delta)
	{
		if (Disable)
			return;

		UpdateComponent(delta);
	}

	// Init de cada componente. Chamado ao iniciar a classe
	public virtual void InitComponent() {}

	// Update de cada componente. Chamado a cada loop
	public virtual void UpdateComponent(float delta) {}

	// ==========================================================================================================================================================================================

	// Chamada pelo Gerênciador ao iniciar a classe
	protected static void InitState(Player player, PlayerManager manager)
	{
		Player = player;
		Manager = manager;

		// Atualiza variáveis do estado inicial do player
		WasInAir = Player.StartInAir;
		FlipH = Player.StartFlipH;

		// Sinais para trocar a colisão do player no dash
		Player.DashStarted += () => Player.EmitSignal("ToggleCollision");
		Player.DashFinished += () => Player.EmitSignal("ToggleCollision");

		// Sinal ao trocar de colisão
		Player.ToggleCollision += OnToggleCollision;

		// Reset no estado do player ao morrer
		Player.Died += ResetStatePlayerOnDeat;

		// Sinais emitidos ao entrar/sair da área de um tile interativo
		Player.TileAreaEntered += OnTileAreaEntered;
		Player.TileAreaExited += OnTileAreaExited;
	}

	// Chamada pelo Gerênciador a cada loop
	protected static void StateUpdate()
	{
		// Atualiza os inputs do player
		Direction = CanInput ? Input.GetVector("move_left", "move_right", "move_up", "move_down") : Vector2.Zero;

		PressedKeyJump = Input.IsActionJustPressed("jump") && CanInput;
		ReleasedKeyJump = Input.IsActionJustReleased("jump") && CanInput;

		PressedKeyDash = Input.IsActionJustPressed("dash") && CanInput;

		// Atualiza variáveis de movimento
		IsMovingH = Player.Velocity.X != 0;
		IsMovingV = Player.Velocity.Y != 0;

		if (IsDashing) return;

		// Atualiza o flip no eixo horizontal do player
		FlipH = Direction.X != 0 ? Direction.X < 0 : FlipH;
	}

	// Sinais ====================================================================================================================================================================================
	private static void OnToggleCollision()
	{
		string collision = "default_collision";

		if (IsDashing)
			collision = "dash_collision";
		else if (IsSwimming)
			collision = "swim_collision";

		CurrentCollision = collision;
	}

	private static void OnTileAreaEntered(string type)
	{
        switch (type)
        {
			case "water":
				IsOnWater = true;
				break;

            case "spike":
                Player.Die();
                break;
				
            case "ladder":
                OnLadderArea = true;
                break;
        }
    }

	private static void OnTileAreaExited(string type)
	{
		switch (type)
		{
			case "water":
				IsOnWater = false;
				break;

			case "ladder":
				OnLadderArea = false;
				break;
		}
	}

	private static void ResetStatePlayerOnDeat()
	{
		WasInAir = false;
		IsJumping = false;
		IsWallJumping = false;
		IsDashing = false;
		IsOnWater = false;
		OnLadderArea = false;
	}
}
