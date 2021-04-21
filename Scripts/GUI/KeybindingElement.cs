using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using Utils;

public class KeybindingElement : HBoxContainer {

    public string Action {
        get { return action.Text; }
        set { action.Text = value; }
    }
    public InputEvent InputEvent {
        get { return inputEvent; }
        set {
            inputEvent = value;
            inputButton.Text = inputEvent.AsPrettyText();
        }
    }
    public string InputName {
        get { return inputButton.Text; }
    }

    Label action;
    InputEvent inputEvent;
    Button inputButton;

    public override void _Ready () {
        action = GetNode<Label>("Label");
        inputButton = GetNode<Button>("Button");
    }

    public void ConnectPressed (Node target, string method, Godot.Collections.Array binds = null) {
        inputButton.Connect("pressed", target, method, binds);
    }

}