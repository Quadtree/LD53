using System;
using Godot;

public class Buggy : VehicleBody
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {

    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //
    //  }

    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);

        EngineForce = 0;

        if (Input.IsActionPressed("turn_left"))
        {

        }
        else if (Input.IsActionPressed("turn_right"))
        {
            // leftWheelPower = 1;
            // rightWheelPower = -1;
        }
        else if (Input.IsActionPressed("accelerate"))
        {
            EngineForce = -100;
        }
        else if (Input.IsActionPressed("reverse"))
        {
            EngineForce = 100;
        }
    }
}
