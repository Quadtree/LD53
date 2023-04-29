using System;
using Godot;

public class ChaseCamera : Camera
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {

    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _PhysicsProcess(float delta)
    {
        var trg = GetTree().CurrentScene.FindChildByName<Spatial>("CameraTarget");

        GlobalRotation = trg.GlobalRotation;

        GlobalTranslation = (GlobalTranslation * 0.95f + trg.GlobalTranslation * 0.05f);
    }
}
