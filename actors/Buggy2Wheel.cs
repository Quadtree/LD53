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

        // Joint.LinearLimitY__lowerDistance = 0;
        // Joint.LinearLimitY__upperDistance = 0.5f;
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //
    //  }

    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);

        var ourTransform = Transform;
        ourTransform.origin = new Godot.Vector3(0, 0, 0);

        var localSpaceSpeed = ourTransform.Xform(LinearVelocity);

        var counterSlideForce = new Vector3(-localSpaceSpeed.x, 0, 0) * 4;

        var worldSpaceCounterSlideForce = ourTransform.XformInv(counterSlideForce);

        //AddForce(counterSlideForce, new Vector3(0, 0, 0));

        DebugInfo = $"localSpaceSpeed={Mathf.RoundToInt(localSpaceSpeed.x).ToString().PadLeft(2)},##,{Mathf.RoundToInt(localSpaceSpeed.z).ToString().PadLeft(2)}\ncounterSlideForce={counterSlideForce}";
    }

    public string DebugInfo;
}
