using Godot;
using System;

public class World : Node2D
{
    private bool canRewind = true;
    private Node2D timePoint = null;
    private KinematicBody2D player = null;
    public override void _Ready()
    {
        player = GetNode<KinematicBody2D>("Player");
        player.Connect("PlaceTimePoint", this, "SetTimePoint");
        timePoint = GetNode<Node2D>("TimePoint");
    }

    public override void _Input(InputEvent @event)
    {
        if(Input.IsActionJustPressed("action_rewind"))
            player.GlobalPosition = timePoint.GlobalPosition;
    }

    private void SetTimePoint(Vector2 pos)
    {
        timePoint.GlobalPosition = pos;
    }

    private void TryToRewind(bool isPlayerDying)
    {
        if(canRewind)
        {
            
        }
    }
}
