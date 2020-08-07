using Godot;
using System;

public class World : Node2D
{
    private bool canRewind = true;
    private Node2D timePoint = null;
    private KinematicBody2D player = null;
    private Timer resetRewindTimer = null;
    public override void _Ready()
    {
        player = GetNode<KinematicBody2D>("Player");
        timePoint = GetNode<Node2D>("TimePoint");
        resetRewindTimer = GetNode<Timer>("ResetRewindTimer");

        player.Connect("PlaceTimePoint", this, "SetTimePoint");
        player.Connect("TryToRewind", this, "TryToRewind");
        resetRewindTimer.Connect("timeout", this, "ResetCanRewind");
    }

    public override void _Input(InputEvent @event)
    {
        
            
    }

    private void SetTimePoint(Vector2 pos)
    {
        timePoint.GlobalPosition = pos;
    }

    private void TryToRewind(bool isPlayerDying)
    {
        if(canRewind) {
            player.GlobalPosition = timePoint.GlobalPosition;
            canRewind = false;
            resetRewindTimer.Start(0.4f);
            (player as Player).RewindStatus(false);
        } else if (isPlayerDying){
            GetTree().ChangeScene("res://scene/World.tscn");
        }
    }

    private void ResetCanRewind()
    {
        (player as Player).RewindStatus(true);
        canRewind = true;
    }
}
