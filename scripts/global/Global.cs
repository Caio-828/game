using Godot;
using System;

public partial class Global : Node
{
    static public Transition Transition {get; private set;} // Referência para node da transição

    static private Player Player; // Node do player
    static private Map Map; // Tile set com o terreno do mapa

    static public string FromScene { get; internal set; } // Indica de qual cena o player vem ao mudar
    static public CheckPoint CheckPoint { get; internal set; } // Ultimo check point

    public override void _Ready()
    {
        Transition = GetNode<Transition>("/root/Transition");
    }

    static public Player GetPlayer() => Player;
    static public void SetPlayer(Player player) => Player = player;

    static public Map GetMap() => Map;
    static public void SetMap(Map terrain) => Map = terrain;
}
