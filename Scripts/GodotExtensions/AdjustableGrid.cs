using System;
using Godot;

public class AdjustableGrid : GridContainer {
    [Export] NodePath containerPath = "../../";
    [Export] float itemWidth = 180f;
    [Export] float margin = 10f;
    private Control container;
    public override void _Ready () {
        base._Ready();
        container = GetNode<Control>(containerPath);
    }
    public override void _Process (float _delta) {
        Refresh();
    }

    public void Refresh () {
        int columns = Math.Max(1, (int) ((container?.RectSize.x - margin ?? 0f) / itemWidth));
        if (columns != Columns) {
            Columns = columns;
        }
    }
}
