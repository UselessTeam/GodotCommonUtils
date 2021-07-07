using System;
using System.Collections.Generic;
using Godot;

public class TitleScreenButtons : VBoxContainer {
    [Export] public PackedScene newGameScene = null;
    [Export] public PackedScene loadGameScene = null;
    [Export] public NodePath newGameConfirmationPath = null;

    [Export] List<NodePath> buttonPaths;
    [Export] List<NodePath> panelPaths;


    [Signal] public delegate void NewGame ();
    [Signal] public delegate void LoadGame ();

    List<Control> panels = new List<Control>();

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

        Action startNewGame = () => {
            EmitSignal(nameof(NewGame));
            GetTree().ChangeSceneTo(newGameScene);
        };

        Callback.Connect(GetNode<Button>("StartGame"), "pressed", () => {
            if (newGameConfirmationPath == null) {
                startNewGame();
                return;
            }
            AcceptDialog confirmation = GetNode<AcceptDialog>(newGameConfirmationPath);
            confirmation.Popup_();
            Callback.ConnectOnce(confirmation, "confirmed", startNewGame);
        });

        for (int i = 0 ; i < panelPaths.Count ; i++)
            panels.Add(GetNode<Control>(panelPaths[i]));

        for (int i = 0 ; i < Math.Min(buttonPaths.Count, panelPaths.Count) ; i++) {
            int j = i;
            Callback.Connect(GetNode<Control>(buttonPaths[i]), "pressed", () => {
                if (panels[j].Visible) panels[j].Hide();
                else {
                    HidePanels();
                    panels[j].Show();
                }
            });
        }

        Callback.Connect(GetNode<Button>("Quit"), "pressed", () => GetTree().Quit());
    }

}
