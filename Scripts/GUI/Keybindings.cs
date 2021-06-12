using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using Utils;

public class Keybindings : Control {
    public enum InputMethod {
        Keyboard,
        Joypad,
        None,
    }

    public InputMethod CurrentInputMethod { get; private set; } = InputMethod.Keyboard;

    // Indicates if the action that will have its input rewired
    public string ActionWaitingForInput { get; protected set; } = null;

    [Export] bool SkipUiActions = true;
    [Export] bool SkipDebugActions = true;

    [Export] NodePath buttonContainerPath = null;
    Control buttonContainer;

    [Export] NodePath keybindingPopupPath = null;

    [Export] PackedScene keybindingElement = null;

    public override void _Ready() {
        Connect("draw", this, nameof(InitializeKeybindings));
        buttonContainer = GetNode<Control>(buttonContainerPath);
    }

    void InitializeKeybindings() {
        foreach (Node node in buttonContainer.GetChildren())
            node.QueueFree();

        foreach (string action in InputMap.GetActions()) {
            if (SkipUiActions && action.StartsWith("ui_")) continue;
            if (SkipDebugActions && action.StartsWith("debug_")) continue;

            KeybindingElement element = (KeybindingElement) keybindingElement.Instance();
            buttonContainer.AddChild(element);
            element.Action = action;

            foreach (InputEvent _event in InputMap.GetActionList(action)) {
                if (_event.MatchInputMethod(CurrentInputMethod)) {
                    element.InputEvent = _event;
                    break;
                }
                //Debug
            }
            element.ConnectPressed(this, nameof(GetNewInput), element.Action.InArray());

        }
    }

    KeybindingElement GetKeybingingElement(string action) {
        foreach (var element in buttonContainer.GetChildren().Cast<KeybindingElement>())
            if (element.Action == action) return element;
        return null;
    }

    void GetNewInput(string action) {
        GetNode<Control>(keybindingPopupPath).Show();
        ActionWaitingForInput = action;
    }

    public override void _Input(InputEvent _event) {
        var method = _event.InputMethod();
        if (method == InputMethod.None) return;

        if (method != InputMethod.None && method != CurrentInputMethod) {
            CurrentInputMethod = method;
            if (IsVisibleInTree())
                InitializeKeybindings();
        }
        if (ActionWaitingForInput != null) {
            GetNode<Control>(keybindingPopupPath).Hide();
            KeybindingElement element = GetKeybingingElement(ActionWaitingForInput);
            InputMap.ActionEraseEvent(ActionWaitingForInput, element.InputEvent);
            InputMap.ActionAddEvent(ActionWaitingForInput, _event);
            element.InputEvent = _event;
            ActionWaitingForInput = null;
        }
    }

}

public static class InputEventExtension {
    public static Keybindings.InputMethod InputMethod(this InputEvent _event) {
        if (_event is InputEventKey || (_event is InputEventMouseButton && !(_event as InputEventMouseButton).Pressed))
            return Keybindings.InputMethod.Keyboard;
        else if (_event is InputEventJoypadButton || (_event is InputEventJoypadMotion && Mathf.Abs((_event as InputEventJoypadMotion).AxisValue) > 0.5))
            return Keybindings.InputMethod.Joypad;
        else
            return Keybindings.InputMethod.None;
    }

    public static bool MatchInputMethod(this InputEvent _event, Keybindings.InputMethod method) {
        return _event.InputMethod() == method;
    }

    public static InputEvent ToAbsolute(this InputEvent _event) {
        if (_event is InputEventKey)
            return _event;
        else if (_event is InputEventMouseButton) {
            var absolute = new InputEventMouseButton();
            absolute.ButtonIndex = (_event as InputEventMouseButton).ButtonIndex;
            absolute.Pressed = false;
            return absolute;
        } else if (_event is InputEventJoypadButton)
            return _event;
        else if (_event is InputEventJoypadMotion)
            return _event;
        else
            return _event;
    }

    // Deprecated for now, use AsText() instead
    //
    public static string AsPrettyText(this InputEvent _event) {
        if (_event is InputEventKey)
            return _event.AsText();
        else if (_event is InputEventMouseButton)
            return "Mouse button " + (_event as InputEventMouseButton).ButtonIndex;
        else if (_event is InputEventJoypadButton)
            return "Gamepad button " + (_event as InputEventJoypadButton).ButtonIndex;
        else if (_event is InputEventJoypadMotion)
            return "Gamepad axis " + (_event as InputEventJoypadMotion).Axis + " " + (((_event as InputEventJoypadMotion).AxisValue > 0) ? "+" : "-");
        else
            return "Input not recognized";
    }
}

