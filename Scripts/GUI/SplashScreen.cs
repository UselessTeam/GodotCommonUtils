using System;
using Godot;

public class SplashScreen : Control {
    AnimationPlayer animationPlayer;

    public override void _Ready () {
        animationPlayer = GetNode<AnimationPlayer>("../AnimationPlayer");
        animationPlayer.Play("splash_screen");

        animationPlayer.Connect("animation_finished", this, nameof(OnAnimationFinished));

    }

    void OnAnimationFinished (string animation) {
        if (animation == "splash_screen")
            this.Hide();
    }

}
