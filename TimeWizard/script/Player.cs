using Godot;
using System;

public class Player : KinematicBody2D
{
    [Signal]
    public delegate void PlaceTimePoint(Vector2 pos);

    [Signal]
    public delegate void TryToRewind(bool isPlayerDying);

    [Export] public Texture ManaFull = null;
    [Export] public Texture ManaEmpty = null;
    public float Acceleration = 512;
    public float MaxSpeed = 64;
    public float Friction = 8f;
    public float Gravity = 200;
    public float JumpForce = 128;
    public int JumpCount = 0;
    private bool takingDamage = false;

    private Vector2 motion = new Vector2();
    private Sprite sprite = null;
    private Timer hitCoolDownTimer = null;

    public override void _Ready()
    {
        sprite = GetNode<Sprite>("Sprite");
        hitCoolDownTimer = GetNode<Timer>("HitCoolDownTimer");

        hitCoolDownTimer.Connect("timeout", this, "hitCoolDown");
    }

    public override void _PhysicsProcess(float delta)
    {
        if (Input.IsActionJustPressed("action_place_point"))
            EmitSignal("PlaceTimePoint", GlobalPosition);
        if (Input.IsActionJustPressed("action_rewind"))
            EmitSignal("TryToRewind", false);

        Movement(delta);
    }

    private void Movement(float delta)
    {
        float xInput = Input.GetActionStrength("move_right") - Input.GetActionStrength("move_left");

        if (xInput != 0)
        {
            motion.x += xInput * Acceleration * delta;
            motion.x = Mathf.Clamp(motion.x, -MaxSpeed, MaxSpeed);

            if (xInput > 0)
                sprite.FlipH = false;
            else
                sprite.FlipH = true;
        }
        else
        {
            motion.x = Mathf.Lerp(motion.x, 0, Friction * delta);
        }

        motion.y += Gravity * delta;


        if(IsOnFloor() || IsOnWall()) { // Wall Jump
            JumpCount = 0;
        }
        if(JumpCount < 2) {
            if (Input.IsActionJustPressed("move_jump"))
            {
                JumpCount++;
                if(TestMove(Transform, Vector2.Down))
                {
                    motion.y = -JumpForce * 0.75f;
                }
                else if (TestMove(Transform, Vector2.Left))
                {
                    motion.y = -JumpForce;
                    motion.x = JumpForce;
                    sprite.FlipH = false;
                }
                else if (TestMove(Transform, Vector2.Right))
                {
                    motion.y = -JumpForce;
                    motion.x = -JumpForce;
                    sprite.FlipH = true;
                }
                else
                {
                    motion.y = -JumpForce * 0.75f;
                }
            }
        }

        if (Input.IsActionJustReleased("move_jump") && motion.y < 0)
        {
            motion.y = motion.y / 2;
        }

        motion = MoveAndSlide(motion, Vector2.Up);
    }

    private void hitCoolDown()
    {
        takingDamage = false;
    }

    public void RewindStatus(bool canRewind)
    {
        if (canRewind)
            sprite.Texture = ManaFull;
        else
            sprite.Texture = ManaEmpty;
    }

    public void TakeDamage()
    {
        if (!takingDamage)
        {
            takingDamage = true;
            hitCoolDownTimer.Start(0.05f);
            EmitSignal("TryToRewind", true);
        }
    }
}
