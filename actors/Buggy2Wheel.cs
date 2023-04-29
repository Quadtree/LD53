using System;
using System.Collections.Generic;
using Godot;

public class Buggy2Wheel : RigidBody
{
    public Generic6DOFJoint Joint;

    List<PhysicsBody> InContactWith = new List<PhysicsBody>();

    float Traction;

    public override void _Ready()
    {
        CallDeferred(nameof(SetupJoints));
    }

    void SetupJoints()
    {
        Joint = new Generic6DOFJoint();
        AddChild(Joint);
        //Joint.Nodes__nodeA = this.GetParent().FindChildByName<RigidBody>("Buggy2Body");
        //Joint.SetNodeA()
        Joint.Nodes__nodeA = "../../Buggy2Body";
        Joint.Nodes__nodeB = $"../../{Name}";

        Joint.AngularLimitX__enabled = false;



        Joint.LinearLimitY__lowerDistance = 0;
        Joint.LinearLimitY__upperDistance = 0.5f;

        this.Connect("body_entered", this, nameof(BodyEntered));
        this.Connect("body_exited", this, nameof(BodyExited));

        ContactsReported = 100;
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        var bar = this.FindChildByName<Spatial>("Bar");

        var body = this.GetParent().FindChildByName<Spatial>("Buggy2Body");

        bar.GlobalTranslation = (body.GlobalTranslation + GlobalTranslation) / 2;
        bar.LookAt(body.GlobalTranslation, Vector3.Up);


    }

    public float MotorPower;

    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);

        if (Joint != null) Joint.Transform = Transform;

        var yRotation = this.GetParent().FindChildByName<Spatial>("Buggy2Body").GlobalRotation.y;
        var ourTransform = new Transform(new Quat(new Vector3(0, yRotation, 0)), new Vector3(0, 0, 0));

        var localSpaceSpeed = ourTransform.XformInv(LinearVelocity);

        localSpaceSpeed = new Vector3(localSpaceSpeed.x * 0.93f, localSpaceSpeed.y, localSpaceSpeed.z + -MotorPower);

        var modifiedWorldSpaceSpeed = ourTransform.Xform(localSpaceSpeed);

        LinearVelocity = (LinearVelocity * (1 - Traction) + modifiedWorldSpaceSpeed * Traction);

        //LinearDamp = Mathf.Abs(localSpaceSpeed.x) / (Mathf.Abs(localSpaceSpeed.z) + 0.1f) * 3;

        var TRACTION_CHANGE_RATE = 0.07f;

        Traction = (InContactWith.Count > 0 ? 1 : 0) * TRACTION_CHANGE_RATE + Traction * (1 - TRACTION_CHANGE_RATE);

        DebugInfo = $"yRotation={yRotation}\n" +
        $"localSpaceSpeed={Mathf.RoundToInt(localSpaceSpeed.x).ToString().PadLeft(2)},{Mathf.RoundToInt(localSpaceSpeed.x).ToString().PadLeft(2)},{Mathf.RoundToInt(localSpaceSpeed.z).ToString().PadLeft(2)}\n" +
        $"Velocity={LinearVelocity}\nInContactWith={InContactWith.Count}\nTraction={Traction}";
    }

    void BodyEntered(PhysicsBody other)
    {
        //GD.Print($"BodyEntered({other})");
        InContactWith.Add(other);

        Util.SpawnOneShotSound("res://sounds/collision0.wav", this, GlobalTranslation);
    }

    void BodyExited(PhysicsBody other)
    {
        //GD.Print($"BodyExited({other})");
        InContactWith.Remove(other);
    }



    public string DebugInfo;
}
