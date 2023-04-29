using System;
using Godot;

public class RockField : Spatial
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        CallDeferred(nameof(Setup));
    }

    void Setup()
    {
        for (var i = 0; i < 40; ++i)
        {
            var rock = GD.Load<PackedScene>($"res://actors/rocks/Rock{Util.RandInt(1, 5)}.tscn").Instance<Spatial>();

            AddChild(rock);
            rock.GlobalTranslation = GlobalTranslation + new Vector3(Util.RandF(-50, 50), 3, Util.RandF(-50, 50));
        }
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //
    //  }
}
