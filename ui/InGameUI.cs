using System;
using Godot;

public class InGameUI : Control
{
    public float TimeLeft = 300;
    public int Score = 0;


    public override void _Ready()
    {

    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        var pc = GetTree().CurrentScene.FindChildByType<Buggy2>();

        this.FindChildByName<Label>("Label").Text = $"Time Left: {Mathf.RoundToInt(TimeLeft)}\nScore: {Score}\nHas Cargo: {pc.HasCargo}\nDest: {pc.Destination}";

        TimeLeft -= delta;
    }
}
