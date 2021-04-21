using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

public class Callback : Node {
    Action callback;
    public void Execute () {
        if (callback != null) callback();
    }

    public Callback (Action action) {
        this.callback = action;
    }

    public static void Connect (Node node, string signal, Action action) {
        Callback call = new Callback(action);
        node.AddChild(call);
        node.Connect(signal, call, nameof(Execute));
    }
}