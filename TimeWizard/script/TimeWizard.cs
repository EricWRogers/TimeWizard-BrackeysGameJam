using Godot;
using System;

public class TimeWizard : KinematicBody2D
{
    private const float GRAVITY = 9.8f;
    private const float ACCELERATION = 50f;
    private const float FRICTION = 0.1f;
    private const float JUMP = -400f;
    private const float MAXSPEED = 200f;
    private Vector2 motion = new Vector2();
    public override void _PhysicsProcess(float delta)
    {
        Vector2 movementInput = Vector2.Zero;

        if(Input.IsActionPressed("ui_right"))
            movementInput.x += 1f;
        if(Input.IsActionPressed("ui_left"))
            movementInput.x -= 1f;
        
        motion.x += (movementInput.x*ACCELERATION);
        
        if(IsOnFloor()) {
            motion.x = Mathf.Lerp(motion.x, 0, FRICTION);
            if(Input.IsActionPressed("ui_up")) {
                motion.y = JUMP;
            }
        } else {
            motion.x = Mathf.Lerp(motion.x, 0, FRICTION/2);
            motion.y += GRAVITY;
        }
        
        motion.x = Mathf.Clamp(motion.x,-MAXSPEED,MAXSPEED);

        motion = MoveAndSlide(motion,Vector2.Up);
    }
}
