using Godot;
using System;

public partial class Player : CharacterBody2D
{
	[Export] public bool StartInAir {get; set;} = false; // Controle se inicia no ar
	[Export] public bool StartFlipH {get; set;} = false; // Controle se inicia flipado

	[Signal] public delegate void LandedEventHandler(); // Sinal emitido ao aterrissar no chão
	[Signal] public delegate void DashStartedEventHandler(); // Sinal emitido ao iniciar o dash
	[Signal] public delegate void DashFinishedEventHandler(); // Sinal emitido ao finalizar o dash
	[Signal] public delegate void DiedEventHandler(); // Sinal emitido quando o player morre
	[Signal] public delegate void ToggleCollisionEventHandler(); // Sinal emitido ao trocar a colisão do player
	[Signal] public delegate void TileAreaEnteredEventHandler(string type); // Sinal emitido ao entrar na área de colisão de um tile interativo
	[Signal] public delegate void TileAreaExitedEventHandler(string type); // Sinal emitido ao sair da área de colisão de um tile interativo


	public PlayerManager Manager { get; private set; } // Gerenciador de estado do player

	public RectangleShape2D Collision { get; internal set; } // Node da colisão do player

    public override void _EnterTree()
    {
        Global.SetPlayerPath(GetPath()); // Atribuindo caminho do player para referência global
    }

    public override void _Ready()
    {
		Manager = GetNode<PlayerManager>("state");
		Manager.Init(this);
    }
    public override void _PhysicsProcess(double delta)
	{
		Manager.Update((float)delta);
		MoveAndSlide();
	}

	public void Die()
	{
		Manager.Die();
	}
}	