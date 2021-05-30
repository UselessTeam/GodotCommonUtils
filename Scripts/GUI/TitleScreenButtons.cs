using System;
using Godot;

public class TitleScreenButtons : VBoxContainer {

    [Export] PackedScene newGameScene;
    [Export] PackedScene loadGameScene;

    public override void _Ready () {
        var loadButton = GetNode<Button>("LoadGame");
        if (!Utils.FileEncoder.SaveExists())
            loadButton.Disabled = true;
        else {
            Callback.Connect(loadButton, "pressed", () => GetTree().ChangeSceneTo(loadGameScene));
        }

        Callback.Connect(GetNode<Button>("StartGame"), "pressed", () => GetTree().ChangeSceneTo(newGameScene));

        Callback.Connect(GetNode<Button>("Settings"), "pressed", () => {
            var settings = GetNode<Control>("../Settings");
            if (settings.Visible) settings.Hide();
            else settings.Show();
        });

        Callback.Connect(GetNode<Button>("Quit"), "pressed", () => GetTree().Quit());

    }

}
