using System;
using System.Collections.Generic;
using Godot;

public class Buggy2 : Spatial
{
    float EnginePower = 7f;

    List<Node> LeftWheels;
    List<Node> RightWheels;

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

        if (LeftWheels == null)
        {
            foreach (var it in this.FindChildrenByType<Buggy2Wheel>())
            {
                if (it.Joint == null) continue;

                if (LeftWheels == null)
                {
                    LeftWheels = new List<Node>();
                    RightWheels = new List<Node>();
                }

                if (it.Transform.origin.x < 0)
                {
                    LeftWheels.Add(it);
                }
                else
                {
                    RightWheels.Add(it);
                }
            }
        }

        var baseRotation = new Vector3(1, 0, 0);

        foreach (var it in this.FindChildrenByType<Buggy2Wheel>())
        {
            if (it.Joint == null) continue;
            //GD.Print(it.Transform.origin.x);
            if (LeftWheels.Contains(it))
            {
                it.AngularVelocity = baseRotation * leftWheelPower * -EnginePower;
            }
            else
            {
                it.AngularVelocity = baseRotation * rightWheelPower * -EnginePower;
            }

            // if (it.Transform.origin.x < 0)
            // {
            //     it.Joint.AngularMotorX__enabled = true;
            //     it.Joint.AngularMotorX__forceLimit = 10_000;
            //     it.Joint.AngularMotorX__targetVelocity = leftWheelPower * -EnginePower;
            // }
            // else
            // {
            //     it.Joint.AngularMotorX__enabled = true;
            //     it.Joint.AngularMotorX__forceLimit = 10_000;
            //     it.Joint.AngularMotorX__targetVelocity = rightWheelPower * -EnginePower;
            // }
        }
    }
}
