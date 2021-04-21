using System;
using Godot;

public class TitleScreenButtons : VBoxContainer {

    [Export] PackedScene startGameScene;

    public override void _Ready () {
        Callback.Connect(GetNode<Button>("StartGame"), "pressed", () => GetTree().ChangeSceneTo(startGameScene));

        Callback.Connect(GetNode<Button>("Settings"), "pressed", () => {
            var settings = GetNode<Control>("../Settings");
            if (settings.Visible) settings.Hide();
            else settings.Show();
        });

        Callback.Connect(GetNode<Button>("Quit"), "pressed", () => GetTree().Quit());

    }

}
