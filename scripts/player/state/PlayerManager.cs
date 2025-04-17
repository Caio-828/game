using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class PlayerManager : StatePlayer
{
	public Dictionary<string, StatePlayer> Components {get; private set; } = []; // Dicionário com todos os componentes

    public void Init(Player player)
    {
		InitState(player, this);
		InitializeComponents();
    }

	public override void Update(float delta)
	{
		StateUpdate();
		UpdatingComponents(delta);
	}

	private void InitializeComponents()
	{
		// Iniciando todos os componentes
		foreach (StatePlayer component in GetChildren().Cast<StatePlayer>())
		{
			component.Init();
			Components.Add(component.Name, component);
		}
	}

	private void UpdatingComponents(float delta)
	{
		foreach (StatePlayer component in Components.Values) component.Update(delta);
	}

	public async void Die()
	{
		CanInput = false; // Impede o player de se mover
		Global.Transition.Start("circle_player"); // Inicia uma transição

		await ToSignal(Global.Transition, "Peak"); // Espera a transição chegar no pico para continuar
		
		Player.EmitSignal("Died"); // Emite o sinal que o player morreu
		CanInput = true; // Permite que o player se mova novamente
	}
}
