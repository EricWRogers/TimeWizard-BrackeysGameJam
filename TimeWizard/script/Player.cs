using Godot;
using System;

public class Player : KinematicBody2D
{
    [Signal]
    public delegate void PlaceTimePoint(Vector2 pos);
    public float Acceleration = 512;
    public float MaxSpeed = 64;
    public float Friction = 8f;
    public float Gravity = 200;
    public float JumpForce = 128;
    public int JumpCount = 0;

    private Vector2 motion = new Vector2();
    private Sprite sprite = null;

    public override void _Ready()
    {
        sprite = GetNode<Sprite>("Sprite");
    }

    public override void _PhysicsProcess(float delta)
    {
        if(Input.IsActionJustPressed("action_place_point"))
            EmitSignal("PlaceTimePoint", GlobalPosition);
        Movement(delta);
    }

    private void Movement(float delta)
    {
        float xInput = Input.GetActionStrength("move_right") - Input.GetActionStrength("move_left");

        if(xInput != 0)
        {
            motion.x += xInput * Acceleration * delta;
            motion.x = Mathf.Clamp(motion.x, -MaxSpeed, MaxSpeed);

            if(xInput > 0)
                sprite.FlipH = false;
            else
                sprite.FlipH = true;
        }
        else
        {
            motion.x = Mathf.Lerp(motion.x, 0, Friction * delta);
        }

        motion.y += Gravity * delta;

        //if(TestMove(transform, Vector2.Down)) {
        if(IsOnFloor() || IsOnWall()) { // Wall Jump
            JumpCount = 0;
        } else {
            if(Input.IsActionJustReleased("move_jump") && motion.y < -JumpForce/2) {
                motion.y = -JumpForce/2;
            }
        }
            
        
        if(JumpCount < 1) {
            if(Input.IsActionJustPressed("move_jump"))
            {
                motion.y = -JumpForce;
                JumpCount++;
            }
        }

        motion = MoveAndSlide(motion,Vector2.Up);
    }
}
