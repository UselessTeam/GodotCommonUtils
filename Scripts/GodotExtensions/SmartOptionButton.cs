using System;
using Godot;

public class SmartOptionButton : OptionButton {
    [Export] string[] ItemList;

    [Export] int DefaultItem;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready () {
        foreach (var item in ItemList) {
            AddItem(item);
            Selected = DefaultItem;
            var control = new Control();
        }
    }

}
