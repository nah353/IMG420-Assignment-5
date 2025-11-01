using Godot;
using System;

public partial class ParticleController : GpuParticles2D
{
	private ShaderMaterial _shaderMaterial;
	private float _elapsedTime = 0f;
	
	public override void _Ready()
	{
		// TODO: Load and apply custom shader
		var shader = GD.Load<Shader>("res://Scripts/custom_particle.gdshader");
		_shaderMaterial = new ShaderMaterial();
		_shaderMaterial.Shader = shader;
		Material = _shaderMaterial;
		
		// TODO: Configure particle properties (Amount, Lifetime, Speed, etc.)
		Amount = 4;
		Lifetime = 2.5f;
		Explosiveness = 0.25f;
		Randomness = 0.5f;
		
		// TODO: Set process material properties
		// Hint: Use a new ShaderMaterial with your custom shader
		var processMaterial = (ParticleProcessMaterial)ProcessMaterial;
		processMaterial.AngleMin = -720.0f;
		processMaterial.AngleMax = 720.0f;
		processMaterial.Spread = 180.0f;
		processMaterial.InitialVelocityMin = 100.0f;
		processMaterial.InitialVelocityMax = 100.0f;
		processMaterial.Gravity = Vector3.Zero;
	}
	
	public override void _Process(double delta)
	{
		// TODO: Update shader parameters over time
		// Hint: Use shader parameters to create animated effects
		_elapsedTime += (float)delta;

		float waveIntensity = 0.2f + 0.1f * Mathf.Sin(_elapsedTime * 2.0f);

		// Animate color gradient
		Color startColor = new Color(1.0f, 0.5f + 0.5f * Mathf.Sin(_elapsedTime * 1.5f), 0.2f, 1.0f);
		Color endColor = new Color(1.0f, 0.0f, 0.5f + 0.5f * Mathf.Cos(_elapsedTime * 1.2f), 1.0f);

		_shaderMaterial.SetShaderParameter("wave_intensity", waveIntensity);
		_shaderMaterial.SetShaderParameter("color_start", startColor);
		_shaderMaterial.SetShaderParameter("color_end", endColor);
		
	}
}
