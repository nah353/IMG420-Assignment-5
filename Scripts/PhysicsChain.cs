using Godot;
using System.Collections.Generic;

public partial class PhysicsChain : Node2D
{
	[Export] public int ChainSegments = 5;
	[Export] public float SegmentDistance = 24f;
	[Export] public int ImpulseForce = 500;
	[Export] public PackedScene SegmentScene;
	
	private List<RigidBody2D> _segments = new List<RigidBody2D>();
	private List<Joint2D> _joints = new List<Joint2D>();

	public override void _Ready()
	{
		CreateChain();
	}

	private void CreateChain()
	{
		// Create static anchor
		var anchor = new StaticBody2D();
		AddChild(anchor);

		var anchorShape = new CollisionShape2D{ Shape = new CapsuleShape2D { Radius = 10, Height = 10 } };
		anchor.AddChild(anchorShape);

		// Hold previous link
		Node2D previousBody = anchor;
		Vector2 position = Vector2.Zero;

		// Build chain segments and joints
		for (int i = 0; i < ChainSegments; i++)
		{
			// Create a new segment
			var segment = SegmentScene.Instantiate<RigidBody2D>();
			segment.Position = position + new Vector2(0, SegmentDistance);
			AddChild(segment);
			_segments.Add(segment);

			// Create joint as child of previous body
			var joint = new PinJoint2D();
			previousBody.AddChild(joint);
			_joints.Add(joint);

			// Configure joint connections
			joint.NodeA = previousBody.GetPath();
			joint.NodeB = segment.GetPath();

			// Position joint halfway between current and next link
			joint.GlobalPosition = (previousBody.GlobalPosition + segment.GlobalPosition) / 2f;

			// Adjust bias and softness
			joint.Bias = 0.2f;
			joint.Softness = 0.1f;

			// Move to next link
			previousBody = segment;
			position = segment.Position;
		}
	}

	// Take in segment index and apply force to that segment
	public void ApplyForceToSegment(int segmentIndex, Vector2 force)
	{
		if (segmentIndex < 0 || segmentIndex >= _segments.Count)
			return;

		var segment = _segments[segmentIndex];
		// Make sure segment can move
		segment.Sleeping = false;
		segment.ApplyCentralImpulse(force);
	}
	
	// Check for input (SPacebar) to apply input
	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("apply_force"))
		{
			var force = new Vector2(ImpulseForce, -ImpulseForce);
			ApplyForceToSegment(9, force);
		}
	}
}
