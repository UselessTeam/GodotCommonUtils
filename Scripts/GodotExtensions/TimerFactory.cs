using System;
using Godot;

public static class TimerFactory {

    public static Timer MakeTimer(Node parentNode, float time, Action callback, bool loop = false) {
        var timer = new Timer();
        timer.OneShot = !loop;
        parentNode.AddChild(timer);
        if (!loop) callback += () => timer.QueueFree();
        Callback.Connect(timer, "timeout", callback);
        timer.Start(time);
        return timer;
    }

}