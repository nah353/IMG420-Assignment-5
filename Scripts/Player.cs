using Godot;
using System;

public partial class Player : CharacterBody2D
{
	[Export] public float Speed = 300f;
	[Export] public float PushForce = 50f;
	
	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;
		
		velocity = Vector2.Zero;

		// Get input
		if (Input.IsActionPressed("move_up"))
			velocity.Y -= 1;
		if (Input.IsActionPressed("move_down"))
			velocity.Y += 1;
		if (Input.IsActionPressed("move_left"))
			velocity.X -= 1;
		if (Input.IsActionPressed("move_right"))
			velocity.X += 1;

		// Move
		velocity = velocity.Normalized() * Speed;
		Velocity = velocity;
		MoveAndSlide();
		
		// Handle chain collisions
		int collisionCount = GetSlideCollisionCount();
		for (int i = 0; i < collisionCount; i++)
		{
			var collider = GetSlideCollision(i);
			// Check if collision is a rigidbody2d
			if (collider.GetCollider() is RigidBody2D chainSegment)
			{
				chainSegment.ApplyCentralImpulse(-collider.GetNormal() * PushForce);
			}
		}
	}
}
