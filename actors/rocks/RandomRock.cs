using System;
using Godot;

public class RandomRock : Spatial
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
        this.ClearChildren();

        var rock = GD.Load<PackedScene>($"res://actors/rocks/Rock{Util.RandInt(1, 5)}.tscn").Instance<Spatial>();

        AddChild(rock);
        rock.GlobalTranslation += new Vector3(0, 3, 0);
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //
    //  }
}
