using Godot;
using System;

public class Portal : Area2D
{
    [Export] public string SceneName = "";
    public void OnPortalBodyEntered(Node body)
    {
        if(body.Name == "Player")
            GetTree().ChangeScene(SceneName);
    }
}
