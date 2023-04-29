using System;
using Godot;

public class EndScreen : Spatial
{
    public override void _Input(InputEvent @event)
    {
        base._Input(@event);

        if (@event is InputEventKey)
        {
            GetTree().ChangeScene("res://maps/Moon1.tscn");
        }
    }
}
