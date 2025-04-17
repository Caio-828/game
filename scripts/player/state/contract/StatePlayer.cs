using Godot;
using System;

public abstract partial class StatePlayer : Node2D
{
	static protected Player Player { get; private set; } // Node do player
	static protected StatePlayer Manager { get; private set; } // Gerênciador dos componentes 

	// Input ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
	static public Vector2 Direction { get; protected set; } // Vector com entrada do teclado (Cima, Baixo, Direita, Esquerda)

	static public bool PressedKeyJump { get; protected set; } // Controle de quando a tecla de jump é pressionada
	static public bool ReleasedKeyJump { get; protected set; } // Controle de quando a tecla de jump é solta

	static public bool PressedKeyDash { get; protected set; } // Controle de quando a tecla de dash é pressionada

	// Variáveis de controle -----------------------------------------------------------------------------------------------------------------------------------------------------------------
	static public bool CanInput { get; protected set; } = true; // Cotrole aceita comandos de inputs

	static public bool FlipH { get; protected set; } = false; // Controle se está flipado
	static public bool WasInAir { get; protected set; } = false; // Controle se está no ar

	static public bool IsJumping { get; protected set; } = false; // Controle se está pulando
	static public bool IsWallJumping { get; protected set; } = false; // Controle se está pulando apartir da parede
	static public bool IsDashing { get; protected set; } = false; // Controle se está dashando

	static public bool IsOnWall { get; protected set; } = false; // Controle se está na parede
	
	// Com set privado
	static public bool IsMovingH { get; private set; } = false; // Controle se está movendo na horizontal
	static public bool IsMovingV { get; private set; } = false; // Controle se está movendo no vertical

	// Outros ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
	static public string CurrentCollision { get; private set; } = "default_collision"; // Indicador da colisão atual do player

	// ==========================================================================================================================================================================================

	// Init de cada componente. Chamada ao iniciar a classe
	public virtual void Init() {}

	// Update de cada componente. Chamada a cada loop
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
	static private void OnDashStarted()
	{
		Player.EmitSignal("ToggleCollision", "dash_collision");
		CurrentCollision = "dash_collision";
	}

	static private void OnDashFinished()
	{
		Player.EmitSignal("ToggleCollision", "default_collision");
		CurrentCollision = "default_collision";
	}
}
