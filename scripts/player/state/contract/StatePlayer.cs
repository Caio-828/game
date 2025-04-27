using Godot;
using System;
using System.Collections.Generic;

public abstract partial class StatePlayer : Node2D
{
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

	public static bool IsOnWall { get; protected set; } = false; // Controle se está na parede

	public static bool OnLadderArea { get; protected set; } = false; // Controle se está na área de uma escada
	public static bool IsOnLadder { get; protected set; } = false; // Controle se está subindo uma escada
	
	// Com set privado
	public static bool IsMovingH { get; private set; } = false; // Controle se está movendo na horizontal
	public static bool IsMovingV { get; private set; } = false; // Controle se está movendo no vertical

	// Outros ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
	public static string CurrentCollision { get; private set; } = "default_collision"; // Indicador da colisão atual do player

	// Dados do tile interativo a ser tocado no momento (Valor null caso não esteja em contato com nenhum)
	public static Dictionary<string, Variant?> InteractiveTilesTouched { get; protected set; } = new Dictionary<string, Variant?>
	{
		{"type", null},
		{"GlobalPosition", null}
	};

	// ==========================================================================================================================================================================================

	// Init de cada componente. Chamado ao iniciar a classe
	public virtual void Init() {}

	// Update de cada componente. Chamado a cada loop
	public virtual void Update(float delta) {}

	// ==========================================================================================================================================================================================

	// Chamada pelo Gerênciador ao iniciar a classe
	protected static void InitState(Player player, PlayerManager manager)
	{
		Player = player;
		Manager = manager;

		// Atualiza variáveis do estado inicial do player
		WasInAir = Player.StartInAir;
		FlipH = Player.StartFlipH;

		// Sinais para a alteração da área de colisão do player
		Player.DashStarted += OnDashStarted;
		Player.DashFinished += OnDashFinished;

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
	private static void OnDashStarted()
	{
		Player.EmitSignal("ToggleCollision", "dash_collision");
		CurrentCollision = "dash_collision";
	}

	private static void OnDashFinished()
	{
		Player.EmitSignal("ToggleCollision", "default_collision");
		CurrentCollision = "default_collision";
	}

	private static void OnTileAreaEntered(string type)
	{
        switch (type)
        {
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
			case "ladder":
				OnLadderArea = false;
				break;
		}
	}
}
