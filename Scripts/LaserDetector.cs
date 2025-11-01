using Godot;

public partial class LaserDetector : Node2D
{
	[Export] public float LaserLength = 500f;
	[Export] public Color LaserColorNormal = Colors.Green;
	[Export] public Color LaserColorAlert = Colors.Red;
	[Export] public NodePath PlayerPath;
	
	private RayCast2D _rayCast;
	private Line2D _laserBeam;
	private Node2D _player;
	private bool _isAlarmActive = false;
	private Timer _alarmTimer;
	private AudioStreamPlayer _alarmSound;
	private GpuParticles2D _particleSystem;
	
	public override void _Ready()
	{
		SetupRaycast();
		SetupVisuals();

		// Get player reference
		if (PlayerPath != null)
			_player = GetNode<Node2D>(PlayerPath);

		// Setup alarm timer
		_alarmTimer = new Timer();
		_alarmTimer.WaitTime = 2.72f;
		_alarmTimer.OneShot = true;
		_alarmTimer.Timeout += ResetAlarm;
		AddChild(_alarmTimer);
		
		_alarmSound = GetNode<AudioStreamPlayer>("AlarmSound");
		_particleSystem = GetNode<GpuParticles2D>("ParticleController");
	}
	
	private void SetupRaycast()
	{
		// Create and configure RayCast2D
		_rayCast = new RayCast2D();
		AddChild(_rayCast);
		_rayCast.TargetPosition = new Vector2(LaserLength, 0);
		_rayCast.Enabled = true;
	}
	
	private void SetupVisuals()
	{
		// Create Line2D for visuals
		_laserBeam = new Line2D();
		AddChild(_laserBeam);
		_laserBeam.Width = 2f;
		_laserBeam.ZIndex = -1;
		_laserBeam.DefaultColor = LaserColorNormal;
		_laserBeam.Points = new Vector2[] { Vector2.Zero, new Vector2(LaserLength, 0) };
	}
	
	public override void _PhysicsProcess(double delta)
	{
		_rayCast.ForceRaycastUpdate();
		UpdateLaserBeam();

		if (_rayCast.IsColliding())
		{
			var collider = _rayCast.GetCollider();
			if (collider == _player && !_isAlarmActive)
				TriggerAlarm();
		}
	}
	
	private void UpdateLaserBeam()
	{
		Vector2 endPoint;

		if (_rayCast.IsColliding())
			endPoint = ToLocal(_rayCast.GetCollisionPoint());
		else
			endPoint = _rayCast.TargetPosition;

		_laserBeam.Points = new Vector2[] { Vector2.Zero, endPoint };
	}

	private void TriggerAlarm()
	{
		_isAlarmActive = true;
		_laserBeam.DefaultColor = LaserColorAlert;
		GD.Print("ALARM! Player detected!");
		_alarmSound.Play();
		_particleSystem.Emitting = true;

		_alarmTimer.Start();
	}
	
	private void ResetAlarm()
	{
		_isAlarmActive = false;
		_laserBeam.DefaultColor = LaserColorNormal;
		_particleSystem.Emitting = false;
	}
}
