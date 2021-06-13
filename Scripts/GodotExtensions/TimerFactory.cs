using System;
using Godot;

public static class TimerFactory {

    public static Timer MakeTimer(Node parentNode, float time, Action callback, bool loop = false) {
        var timer = new Timer();
        timer.OneShot = !loop;
        if (!loop) callback += () => timer.QueueFree();
        Callback.Connect(timer, "timeout", callback);
        if (time > 0) timer.WaitTime = time;
        timer.Autostart = true; ;
        parentNode.CallDeferred("add_child", timer);
        // timer.Start (time);
        return timer;
    }

}