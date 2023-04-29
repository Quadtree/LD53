using System;
using System.Linq;
using Godot;

public class MoonBase : Spatial
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {

    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        var pc = GetTree().CurrentScene.FindChildByType<Buggy2>();

        if (pc.FindChildByName<Spatial>("Buggy2Body").GlobalTranslation.DistanceTo(GlobalTranslation) < 10)
        {
            if (!pc.HasCargo)
            {
                pc.HasCargo = true;
                pc.Destination = Util.Choice(GetTree().CurrentScene.FindChildrenByType<MoonBase>().Where(it => it != this));
            }
            else if (pc.Destination == this)
            {
                GetTree().CurrentScene.FindChildByType<InGameUI>().Score += 10;
                pc.HasCargo = false;
            }
        }
    }
}
