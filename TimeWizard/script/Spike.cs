using Godot;
using System;

public class Spike : Area2D
{
    public override void _Ready()
    {
        Connect("body_entered", this, "OnSpikeBodyEntered");
    }
    public void OnSpikeBodyEntered(Node body)
    {
        if(body.Name == "Player")
            (body as Player).TakeDamage();
    }
}
