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

    [Export]
    int RandomSeed;

    public override void _Ready()
    {
        CallDeferred(nameof(Setup));
    }

    public static float RandF(Random rand, float min, float max)
    {
        return (float)rand.NextDouble() * (max - min) + min;
    }

    void Setup()
    {
        var rand = new Random(RandomSeed);

        for (var i = 0; i < NumRocks; ++i)
        {
            var rock = GD.Load<PackedScene>(RockTypeOverride?.Length > 0 ? RockTypeOverride : $"res://actors/rocks/Rock{Util.RandInt(1, 5)}.tscn").Instance<Spatial>();

            AddChild(rock);

            var rockStart = GlobalTranslation + new Vector3(RandF(rand, -FieldSize, FieldSize), 10, RandF(rand, -FieldSize, FieldSize));

            var res = GetWorld().DirectSpaceState.IntersectRay(
                rockStart,
                rockStart + new Vector3(0, -50, 0)
            );

            if (!res.Contains("position")) continue;

            rock.GlobalTranslation = (Vector3)res["position"];
            rock.GlobalRotation = new Vector3(RandF(rand, -4, 4), RandF(rand, -4, 4), RandF(rand, -4, 4));

            if (rock is RigidBody)
            {
                ((RigidBody)rock).Sleeping = true;
            }

            //foreach (var it in rock.FindChildrenByType<Spatial>()) new Vector3(RockScaling, RockScaling, RockScaling);
        }
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //
    //  }
}
