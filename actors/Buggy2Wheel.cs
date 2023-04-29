using System;
using Godot;

public class Buggy2Wheel : RigidBody
{
    Generic6DOFJoint Joint;

    public override void _Ready()
    {
        CallDeferred(nameof(SetupJoints));
    }

    void SetupJoints()
    {
        Joint = new Generic6DOFJoint();
        GetParent().AddChild(Joint);
        //Joint.Nodes__nodeA = this.GetParent().FindChildByName<RigidBody>("Buggy2Body");
        //Joint.SetNodeA()
        Joint.Nodes__nodeA = "../Buggy2Body";
        Joint.Nodes__nodeB = $"../{Name}";

        Joint.AngularLimitX__enabled = false;

        Joint.Transform = Transform;
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //
    //  }
}
