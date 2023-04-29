using System;
using Godot;

public class EndOverlay : Control
{
    float HoldTime = 1;

    public override void _Ready()
    {
        base._Ready();

        PauseMode = PauseModeEnum.Process;
    }

    public override void _Process(float delta)
    {
        base._Process(delta);

        HoldTime -= delta;
    }

    public override void _Input(InputEvent @event)
    {
        base._Input(@event);

        if (@event is InputEventKey && HoldTime <= 0)
        {
            GetTree().Paused = false;

            if (@event.IsActionPressed("restart_game"))
            {
                GetTree().ChangeScene("res://maps/Moon1.tscn");
            }
            else
            {
                QueueFree();
            }
        }
    }
}
