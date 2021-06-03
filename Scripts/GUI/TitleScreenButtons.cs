using System;
using System.Collections.Generic;
using Godot;

public class TitleScreenButtons : VBoxContainer {

    [Export] PackedScene newGameScene;
    [Export] PackedScene loadGameScene;

    [Signal] public delegate void NewGame ();
    [Signal] public delegate void LoadGame ();

    List<Control> panels;

    void HidePanels () {
        foreach (var panel in panels)
            panel.Hide();
    }

    public override void _Ready () {
        var loadButton = GetNode<Button>("LoadGame");
        if (!Utils.FileEncoder.SaveExists())
            loadButton.Disabled = true;
        else {
            Callback.Connect(loadButton, "pressed", () => {
                EmitSignal(nameof(LoadGame));
                GetTree().ChangeSceneTo(loadGameScene);
            });
        }

        Callback.Connect(GetNode<Button>("StartGame"), "pressed", () => {
            EmitSignal(nameof(NewGame));
            GetTree().ChangeSceneTo(newGameScene);
        });

        panels = new List<Control>() {
            GetNode<Control>("../Settings"),
            GetNode<Control>("../Credits")
        };

        Callback.Connect(GetNode<Button>("Settings"), "pressed", () => {
            var settings = panels[0];
            if (settings.Visible)
                settings.Hide();
            else {
                HidePanels();
                settings.Show();
            }
        });

        Callback.Connect(GetNode<Button>("Credits"), "pressed", () => {
            var credits = panels[1];
            if (credits.Visible) credits.Hide();
            else {
                HidePanels();
                credits.Show();
            }
        });

        Callback.Connect(GetNode<Button>("Quit"), "pressed", () => GetTree().Quit());

    }

}
