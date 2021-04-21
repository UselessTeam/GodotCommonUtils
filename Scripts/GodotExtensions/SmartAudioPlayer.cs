using System;
using Godot;

public class SmartAudioPlayer : AudioStreamPlayer {

    [Export(PropertyHint.File, "*.wav")] string Tracks;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready () {

    }
}
