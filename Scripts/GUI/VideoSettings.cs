using System;
using Godot;

public class VideoSettings : Control {
    // Changing the resolution will result in scaling the viewport to keep the same information on-screen
    //
    // A CanvasLayer and all of its children will be unaffected by the change in resolution (which is 
    // usefull in the case of Control nodes that already naturally scale with the resolution)

    Vector2 baseResolution;

    const string resolutionSeparator = " x ";
    OptionButton resolutionOptions;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready () {
        baseResolution = new Vector2((int) ProjectSettings.GetSetting("display/window/size/width"), (int) ProjectSettings.GetSetting("display/window/size/height"));

        resolutionOptions = GetNode<OptionButton>("VBoxContainer/Resolution/OptionButton");
        resolutionOptions.Connect("item_selected", this, nameof(SetResolution));

        GetNode<CheckBox>("VBoxContainer/Fullscreen/CheckBox").Connect("toggled", this, nameof(SetFullscreen));
        // GetNode<CheckBox>("VBoxContainer/Borderless/CheckBox").Connect("toggled", this, nameof(SetBorderless));
    }

    void SetResolution (int idx) {
        var newResolution = ToResolution(resolutionOptions.GetItemText(idx));
        if (!OS.WindowFullscreen)
            OS.WindowSize = newResolution;

        GetTree().SetScreenStretch(SceneTree.StretchMode.Disabled, SceneTree.StretchAspect.Ignore, newResolution); //TODO

        var canvasTransform = GetViewport().CanvasTransform;
        canvasTransform.Scale = newResolution / baseResolution;
        GetViewport().CanvasTransform = canvasTransform;
    }

    void SetFullscreen (bool fullscreen) {
        OS.WindowFullscreen = fullscreen;
    }
    // void SetBorderless (bool borderless) {
    //     OS.WindowBorderless = borderless;
    // }

    Vector2 ToResolution (string resolutionString) {
        int mid = resolutionString.Find(resolutionSeparator);
        int midLength = resolutionSeparator.Length;
        int end = resolutionString.Find("(");
        return new Vector2(int.Parse(resolutionString.Substr(0, mid)), int.Parse(resolutionString.Substr(mid + midLength, end - mid - midLength)));
    }
}
