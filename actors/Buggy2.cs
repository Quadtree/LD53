using System;
using System.Collections.Generic;
using Godot;

public class Buggy2 : Spatial
{
    float EnginePower = 30f;

    List<Node> LeftWheels;
    List<Node> RightWheels;

    public bool HasCargo = false;
    public Spatial Destination = null;

    public override void _Ready()
    {
        //foreach (var it in GetTree().CurrentScene.FindChildrenByType<StaticBody>()) it.
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //
    //  }

    float RightingForce = 10;

    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);

        float leftWheelPower = 0;
        float rightWheelPower = 0;

        if (Input.IsActionPressed("turn_left"))
        {
            leftWheelPower += -1;
            rightWheelPower += 1;
        }
        if (Input.IsActionPressed("turn_right"))
        {
            leftWheelPower += 1;
            rightWheelPower += -1;
        }
        if (Input.IsActionPressed("accelerate"))
        {
            leftWheelPower += 1;
            rightWheelPower += 1;
        }
        if (Input.IsActionPressed("reverse"))
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

        var baseRotation = this.FindChildByName<Spatial>("Buggy2Body").Transform.basis.x;

        foreach (var it in this.FindChildrenByType<Buggy2Wheel>())
        {
            if (it.Joint == null) continue;
            //GD.Print(it.Transform.origin.x);
            if (LeftWheels.Contains(it))
            {
                it.MotorPower = leftWheelPower;
                //it.AngularVelocity = baseRotation * leftWheelPower * -EnginePower;
            }
            else
            {
                it.MotorPower = rightWheelPower;
                //it.AngularVelocity = baseRotation * rightWheelPower * -EnginePower;
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

        //GD.Print(this.FindChildByName<RigidBody>("Buggy2Body").LinearVelocity.Length());

        var body = this.FindChildByName<RigidBody>("Buggy2Body");

        //GD.Print(this.FindChildByName<Spatial>("Buggy2Body").GlobalTransform.basis.y.y);

        if (body.GlobalTransform.basis.y.y < 0)
        {
            GD.Print("ROLLOVER ACCIDENT!");
            body.AddTorque(body.AngularVelocity.Normalized() * RightingForce);
            RightingForce += 1f;
        }
        else
        {
            RightingForce = 10f;
        }
    }
}
