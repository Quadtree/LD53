using System;
using System.Collections.Generic;
using Godot;
using Godot.Collections;

public class InGameUI : Control
{
    [Export]
    public int Minutes = 3;

    public float TimeLeft;
    public int Score = 0;
    public bool EndScreenShown = false;

    public bool ShowDebugData = true;

    public List<Tuple<Label, Buggy2Wheel>> BuggyWheelMapping = new List<Tuple<Label, Buggy2Wheel>>();


    public override void _Ready()
    {
        TimeLeft = Minutes * 60;

        CallDeferred(nameof(Setup));
    }

    void Setup()
    {
        var dbg = this.FindChildByName<Container>("WheelDebug");

        foreach (var it in GetTree().CurrentScene.FindChildByType<Buggy2>().FindChildrenByType<Buggy2Wheel>())
        {
            var label = new Label();
            dbg.AddChild(label);

            BuggyWheelMapping.Add(Tuple.Create(label, it));

            break;
        }
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        var pc = GetTree().CurrentScene.FindChildByType<Buggy2>();

        this.FindChildByName<Label>("Label").Text = $"Time Left: {Mathf.RoundToInt(TimeLeft) / 60}:{(Mathf.RoundToInt(TimeLeft) % 60).ToString().PadLeft(2, '0')}\nDeliveries: {Score}";//\nHas Cargo: {pc.HasCargo}\nDest: {pc.Destination}";

        TimeLeft = Mathf.Max(TimeLeft - delta, 0);

        if (ShowDebugData)
        {
            foreach (var it in BuggyWheelMapping)
            {
                it.Item1.Visible = true;
                it.Item1.Text = it.Item2.DebugInfo;
            }
        }
        else
        {
            foreach (var it in BuggyWheelMapping) it.Item1.Visible = false;
        }

        if (TimeLeft <= 0 && !EndScreenShown)
        {
            AddChild(GD.Load<PackedScene>("res://ui/EndOverlay.tscn").Instance());
            GetTree().Paused = true;
            EndScreenShown = true;
        }
    }

    public override void _Input(InputEvent @event)
    {
        base._Input(@event);

        if (OS.IsDebugBuild())
        {
            if (@event.IsActionPressed("cheat_little_time_left"))
            {
                TimeLeft = 3;
            }
        }
    }
}
