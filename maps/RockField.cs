using System;
using Godot;

public class RockField : Spatial
{
    [Export]
    int NumRocks;

    [Export]
    float FieldSize;

    [Export]
    float RockScaling;

    [Export]
    string RockTypeOverride;

    public override void _Ready()
    {
        CallDeferred(nameof(Setup));
    }

    void Setup()
    {
        for (var i = 0; i < NumRocks; ++i)
        {
            var rock = GD.Load<PackedScene>(RockTypeOverride?.Length > 0 ? RockTypeOverride : $"res://actors/rocks/Rock{Util.RandInt(1, 5)}.tscn").Instance<Spatial>();

            AddChild(rock);
            rock.GlobalTranslation = GlobalTranslation + new Vector3(Util.RandF(-FieldSize, FieldSize), 3, Util.RandF(-FieldSize, FieldSize));
            //foreach (var it in rock.FindChildrenByType<Spatial>()) new Vector3(RockScaling, RockScaling, RockScaling);
        }
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //
    //  }
}
