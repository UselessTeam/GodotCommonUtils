using System;
using Godot;

public static class BB {
    public static readonly string Example = "[img]res://Assets/Sprites/example.png[/img]";

    public static string Format (string value) {
        return value
            .Replace("[?", "[url=?")
            .Replace("[/?]", "[/url]")
            .Replace("[url=", "[color=#4ff9f9][url=")
            .Replace("[/url]", "[/url][/color]")
            .Replace("[mon]", "Mon" + BB.Example);
    }
}