using System;
using Godot;

public class Buggy2Wheel : RigidBody
{
    public Generic6DOFJoint Joint;

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

        Joint.Transform = Transform;

        Joint.LinearLimitY__lowerDistance = 0;
        Joint.LinearLimitY__upperDistance = 2;
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //
    //  }

    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);

        foreach (var it in this.FindChildrenByType<Buggy2Wheel>())
        {

        }
    }
}
