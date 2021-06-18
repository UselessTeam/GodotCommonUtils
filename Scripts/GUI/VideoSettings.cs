using System;
using Godot;
using Utils;

public class VideoSettings : Control, ISaveable {
    // Changing the resolution will result in scaling the viewport to keep the same information on-screen
    //
    // A CanvasLayer and all of its children will be unaffected by the change in resolution (which is 
    // usefull in the case of Control nodes that already naturally scale with the resolution)

    [Export] bool enable = false;

    Vector2 baseResolution;

    const string resolutionSeparator = " x ";
    SmartOptionButton resolutionOptions;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready () {
        if (enable) {
            baseResolution = new Vector2((int) ProjectSettings.GetSetting("display/window/size/width"), (int) ProjectSettings.GetSetting("display/window/size/height"));
            resolutionOptions = GetNode<SmartOptionButton>("VBoxContainer/Resolution/OptionButton");
            LoadSettings();

            resolutionOptions.Selected = resolutionOption;
            resolutionOptions.Connect("item_selected", this, nameof(SetResolutionFromOption));

            var fullscreenCheckbox = GetNode<CheckBox>("VBoxContainer/Fullscreen/CheckBox");
            fullscreenCheckbox.Pressed = Fullscreen;
            fullscreenCheckbox.Connect("toggled", this, nameof(SetFullscreen));
            // GetNode<CheckBox>("VBoxContainer/Borderless/CheckBox").Connect("toggled", this, nameof(SetBorderless));
        }
    }

    void SaveSettings () {
        FileEncoder.Write(Saver.Save(this), "video_settings".ToPath());
    }

    void LoadSettings () {
        try {
            if (FileEncoder.SaveVersionMatches("video_settings".ToPath())) {
                VideoSettings settings = (VideoSettings) Loader.Load(FileEncoder.Read("video_settings".ToPath()));
                Fullscreen = settings.Fullscreen;
                SetResolutionFromOption(settings.resolutionOption);
            } else
                LoadDefaultSettings();
        } catch (Exception) {
            LoadDefaultSettings();
        }
    }

    void LoadDefaultSettings () {
        Resolution = OptionToResolution(resolutionOptions.DefaultItem);
        Fullscreen = false;
    }

    [Save] int resolutionOption;
    Vector2 Resolution {
        get { return OptionToResolution(resolutionOption); }
        set {
            if (!OS.WindowFullscreen)
                OS.WindowSize = Resolution;

            GetTree().SetScreenStretch(SceneTree.StretchMode.Disabled, SceneTree.StretchAspect.Ignore, Resolution); //TODO

            var canvasTransform = GetViewport().CanvasTransform;
            canvasTransform.Scale = Resolution / baseResolution;
            GetViewport().CanvasTransform = canvasTransform;

            SaveSettings();
        }
    }

    void SetResolutionFromOption (int idx) {
        resolutionOption = idx;
        Resolution = OptionToResolution(idx);
    }
    Vector2 OptionToResolution (int idx) { return ToResolution(resolutionOptions.GetItemText(idx)); }

    [Save] bool fullscreen;
    bool Fullscreen {
        get { return fullscreen; }
        set {
            fullscreen = value;
            OS.WindowFullscreen = fullscreen;
            if (value == false)
                Resolution = Resolution;
            SaveSettings();
        }
    }

    void SetFullscreen (bool set_fullscreen) {
        Fullscreen = set_fullscreen;
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