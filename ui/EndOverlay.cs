using System;
using Godot;

public class EndOverlay : Control
{
    public override void _Input(InputEvent @event)
    {
        base._Input(@event);

        if (@event is InputEventKey)
        {
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
