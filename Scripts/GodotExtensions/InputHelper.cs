using System;
using System.Collections.Generic;
using Godot;

public static class InputHelper {

    public static bool IsClick (InputEvent _event) {
        return (_event is InputEventMouseButton mouseEvent && mouseEvent.ButtonIndex == (int) ButtonList.Left && mouseEvent.Pressed)// Mouse click
                || _event is InputEventScreenTouch screenTouch && screenTouch.Pressed;
    }

}