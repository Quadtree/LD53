using System;
using Godot;

public class Destination : Spatial
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {

    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        var pc = GetTree().CurrentScene.FindChildByType<Buggy2>();

        Rotate(Vector3.Up, delta * 2);

        if (pc.Destination != null)
        {
            this.GlobalTranslation = pc.Destination.GlobalTranslation;
            Visible = true;
        }
        else
        {
            Visible = false;
        }
    }
}
