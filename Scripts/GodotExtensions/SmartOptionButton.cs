using System;
using Godot;

public class SmartOptionButton : OptionButton {
    [Export] string[] ItemList;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready () {
        foreach (var item in ItemList) {
            AddItem(item);
        }
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }
}
