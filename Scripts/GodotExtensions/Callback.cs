using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

public class Callback : Node {
    Action callback;
    public void Execute () {
        if (callback != null) {
            callback();
            if (callOnce == true) QueueFree();
        } else QueueFree();
    }

    bool callOnce = false;

    public Callback (Action action) {
        this.callback = action;
    }

    public static Callback Connect (Node node, string signal, Action action) {
        Callback call = new Callback(action);
        node.AddChild(call);
        node.Connect(signal, call, nameof(Execute));
        return call;
    }
    public static Callback ConnectOnce (Node node, string signal, Action action) {
        var call = Connect(node, signal, action);
        call.callOnce = true;
        return call;
    }
}