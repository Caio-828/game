using Godot;
using System;

public partial class Global : Node
{
    public static Transition Transition {get; private set;} // Referência para node da transição

    private static Player Player { get; set; } // Node do player
    private static Map Map { get; set; } // Tile set com o terreno do mapa

    public static string FromScene { get; internal set; } // Indica de qual cena o player vem ao mudar
    public static CheckPoint CheckPoint { get; internal set; } // Ultimo check point

    public override void _Ready()
    {
        Transition = GetNode<Transition>("/root/Transition");
    }

    private static Node GetRoot()
    {
        return Engine.GetMainLoop() is SceneTree tree ? tree.Root : null;
    }

    public static Player GetPlayer() => Player;
    public static void SetPlayerPath(NodePath path)
    {
        Node root = GetRoot();
        
        if (root?.HasNode(path) ?? false)
        {
            Player = root.GetNode<Player>(path);
        }
    }

    public static Map GetMap() => Map;
    public static void SetMap(Map terrain) => Map = terrain;
}
