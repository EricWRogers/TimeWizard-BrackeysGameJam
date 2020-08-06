using Godot;
using System;

public class TimeWizard : KinematicBody2D
{
    private const float GRAVITY = 8f;
    private const float ACCELERATION = 5f;
    private const float FRICTION = 0.2f;
    private const float JUMP = -120;
    private const float MAXSPEED = 50f;
    private Vector2 motion = new Vector2();
    [Export] private bool hasDoubleJumped = false;
    private Sprite sprite = null;

    public override void _Ready()
    {
        sprite = GetNode<Sprite>("Sprite");
    }
    public override void _PhysicsProcess(float delta)
    {
        Vector2 movementInput = Vector2.Zero;

        if(Input.IsActionPressed("ui_right"))
        {
            movementInput.x += 1f;
            sprite.FlipH = false;
        }
        if(Input.IsActionPressed("ui_left"))
        {
            movementInput.x -= 1f;
            sprite.FlipH = true;
        }
        
        motion.x += (movementInput.x*ACCELERATION);
        
        if(IsOnFloor()) {
            motion.x = Mathf.Lerp(motion.x, 0, FRICTION);
            if(Input.IsActionPressed("ui_up")) {
                motion.y = JUMP;
            }
            hasDoubleJumped = false;
        } else {
            motion.x = Mathf.Lerp(motion.x, 0, FRICTION/2);
            if(Input.IsActionJustPressed("ui_up") && !hasDoubleJumped) {
                motion.y = JUMP;
                hasDoubleJumped = true;
            }
            motion.y += GRAVITY;
        }
        
        motion.x = Mathf.Clamp(motion.x,-MAXSPEED,MAXSPEED);

        motion = MoveAndSlideWithSnap(motion,Vector2.Up);
    }
}
