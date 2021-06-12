using System;
using Godot;

public class TargetCamera : Camera2D {
    public Node2D Target = null;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready() {

    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta) {
        if (Target != null)
            GlobalPosition = Target.GlobalPosition;
    }
}