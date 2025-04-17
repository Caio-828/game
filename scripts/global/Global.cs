using Godot;
using System;

public partial class Global : Node
{

    static public Global Manager {get; private set;} // Referência para este mesmo script
    static public Transition Transition {get; private set;} // Referência para node da transição

    private Player Player; // Node do player
    private Map Map; // Tile set com o terreno do mapa

    public string FromScene { get; internal set; } // Indica de qual cena o player vem ao mudar
    public CheckPoint CheckPoint { get; internal set; } // Ultimo check point

    public override void _Ready()
    {
        Manager = this;
        Transition = GetNode<Transition>("/root/Transition");
    }

    public Player GetPlayer() => Player;
    public void SetPlayer(Player player) => Player = player;

    public Map GetMap() => Map;
    public void SetMap(Map terrain) => Map = terrain;
}
