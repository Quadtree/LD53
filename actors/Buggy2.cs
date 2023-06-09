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

    public float PitTime;
    public float PrevLocCharge;

    public Vector3 FixedPosition;
    public float FixedPositionTime;

    public Vector3[] PreviousLocations = new Vector3[8];

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
            GD.Print($"ROLLOVER ACCIDENT! {RightingForce}");
            var spin = body.AngularVelocity.Normalized();
            spin.y = 0;
            body.AddTorque(spin * RightingForce);
            RightingForce += 1f;

            if (RightingForce > 800)
            {
                SetFixedPosition(body.GlobalTranslation + new Vector3(0, 10, 0));
            }
        }
        else
        {
            RightingForce = 10f;
        }

        PrevLocCharge += delta;
        if (PrevLocCharge >= 1)
        {
            PrevLocCharge -= 1;
            for (var i = PreviousLocations.Length - 1; i > 0; --i)
            {
                PreviousLocations[i] = PreviousLocations[i - 1];
            }
            PreviousLocations[0] = body.GlobalTranslation;
        }

        if (body.GlobalTranslation.x > 0 && body.GlobalTranslation.z > 0 && body.GlobalTranslation.y < -5)
        {
            GD.Print("IN THE PIT!");
            PitTime += delta;
            if (PitTime > 2)
            {
                var outOfPitLocation = PreviousLocations[5] + new Vector3(0, 5, 0);
                GD.Print($"Jumping out of the pit to {outOfPitLocation}");
                SetFixedPosition(outOfPitLocation);
            }

            if (PitTime > 5)
            {
                var outOfPitLocation = GetTree().CurrentScene.FindChildByName<Spatial>("PitSafeSpot").GlobalTranslation;
                GD.Print($"REALLY Jumping out of the pit to {outOfPitLocation}");
                SetFixedPosition(outOfPitLocation);
            }
        }
        else
        {
            PitTime = 0;
        }

        var MAX_DAMP = 100f;
        var DEFAULT_DAMP = 0.2f;

        if (FixedPositionTime > 0)
        {
            body.GlobalTranslation = FixedPosition;
            body.GlobalRotation = new Vector3(0, 0, 0);
            //body.LinearVelocity = new Vector3(0, 0, 0);
            //body.AngularVelocity = new Vector3(0, 0, 0);
            body.LinearDamp = MAX_DAMP;
            body.AngularDamp = MAX_DAMP;

            foreach (var it in this.FindChildrenByType<Buggy2Wheel>())
            {
                //it.LinearVelocity = new Vector3(0, 0, 0);
                //it.AngularVelocity = new Vector3(0, 0, 0);
                it.LinearDamp = MAX_DAMP;
                it.AngularDamp = MAX_DAMP;
            }

            FixedPositionTime -= delta;
        }
        else
        {
            body.LinearDamp = DEFAULT_DAMP;
            body.AngularDamp = DEFAULT_DAMP;

            foreach (var it in this.FindChildrenByType<Buggy2Wheel>())
            {
                it.LinearDamp = DEFAULT_DAMP;
                it.AngularDamp = DEFAULT_DAMP;
            }
        }

        var destLoc = GetTree().CurrentScene.FindChildByType<Destination>();
        if (Destination != null)
        {
            this.FindChildByName<Spatial>("OffscreenAim").LookAt(destLoc.GlobalTranslation, Vector3.Up);

            var isBehind = GetViewport().GetCamera().IsPositionBehind(destLoc.GlobalTranslation);
            var upx = GetViewport().GetCamera().UnprojectPosition(destLoc.GlobalTranslation);

            this.FindChildByName<Spatial>("OffscreenAim").Visible = isBehind || upx.x < 0 || upx.x > GetViewport().Size.x || upx.y < 0 || upx.y > GetViewport().Size.y;

            // if (isBehind)

            // var upx = GetViewport().GetCamera().UnprojectPosition(destLoc.GlobalTranslation);
            // GD.Print(upx);
        }
        else
        {
            this.FindChildByName<Spatial>("OffscreenAim").Visible = false;
        }

        if (Input.IsActionPressed("restart_game"))
        {
            GetTree().ChangeScene("res://maps/Moon1.tscn");
        }

        var shouldPlayEngineAudio = leftWheelPower != 0 || rightWheelPower != 0;

        if (shouldPlayEngineAudio != this.FindChildByName<AudioStreamPlayer3D>("EngineAudio").Playing) this.FindChildByName<AudioStreamPlayer3D>("EngineAudio").Playing = shouldPlayEngineAudio;
    }

    void SetFixedPosition(Vector3 pos)
    {
        FixedPosition = pos;
        FixedPositionTime = 1.5f;

        foreach (var it in this.FindChildrenByType<Buggy2Wheel>())
        {
            it.GlobalTranslation = pos;
        }
    }
}
