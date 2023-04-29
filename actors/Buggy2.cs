using System;
using Godot;

public class Buggy2 : Spatial
{
    float EnginePower = 7f;

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

        float leftWheelPower = 0;
        float rightWheelPower = 0;

        if (Input.IsActionPressed("turn_left"))
        {
            leftWheelPower = -1;
            rightWheelPower = 1;
        }
        else if (Input.IsActionPressed("turn_right"))
        {
            leftWheelPower = 1;
            rightWheelPower = -1;
        }
        else if (Input.IsActionPressed("accelerate"))
        {
            leftWheelPower = 1;
            rightWheelPower = 1;
        }
        else if (Input.IsActionPressed("reverse"))
        {
            leftWheelPower = -1;
            rightWheelPower = -1;
        }

        foreach (var it in this.FindChildrenByType<Buggy2Wheel>())
        {
            //GD.Print(it.Transform.origin.x);
            if (it.Transform.origin.x < 0)
                it.AngularVelocity = new Vector3(leftWheelPower * -EnginePower, 0, 0);
            else
                it.AngularVelocity = new Vector3(rightWheelPower * -EnginePower, 0, 0);
        }
    }
}
