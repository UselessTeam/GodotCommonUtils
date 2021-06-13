using System;
using Godot;

public class RandomPosition2D : Node2D {
    [Export] NodePath position1Path;
    [Export] NodePath position2Path;

    Position2D position1;
    Position2D position2;

    RandomNumberGenerator random = new Godot.RandomNumberGenerator();

    // Called when the node enters the scene tree for the first time.
    public override void _Ready() {
        position1 = GetNode<Position2D>(position1Path);
        position2 = GetNode<Position2D>(position2Path);
    }

    public Vector2 GetRandomPosition() {
        return position1.Position + random.Randf() * (position2.Position - position1.Position);
    }
}
