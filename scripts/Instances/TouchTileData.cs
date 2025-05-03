using Godot;
using System;

public class TouchTileData(string layer, string type, Vector2 globalPosition)
{
    public string Layer { get; private set; } = layer;
    public string Type { get; private set; } = type;
    public Vector2 GlobalPosition { get; private set; } = globalPosition;

    private static readonly TouchTileData _empty = new(null, null, Vector2.Zero);

    public static TouchTileData Empty => _empty;
}
