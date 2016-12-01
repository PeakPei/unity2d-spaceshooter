using UnityEngine;

[RequireComponent(
	typeof(ParticleSystem)
	)]
public class SpaceshipEngineThruster : MonoBehaviour
{
	[SerializeField]
	private ParticleSystem particleSystem;

	[SerializeField]
	private Rigidbody2D rigidbody;

	private void Awake()
	{
		particleSystem = GetComponent<ParticleSystem>();
		particleSystem.simulationSpace = ParticleSystemSimulationSpace.World;
		particleSystem.playOnAwake = false;

		var movement = transform.parent.GetComponent<SpaceshipMovement>();
		Debug.Assert(movement);
		movement.Moving += OnMoving;

		rigidbody = movement.GetComponent<Rigidbody2D>();
	}

	private void OnMoving(Vector2 velocity)
	{
		var speed = velocity == Vector2.zero ? velocity.magnitude : rigidbody.velocity.magnitude;
		particleSystem.startLifetime = speed * 0.1f;
		particleSystem.startSpeed = speed;
		particleSystem.startSize = Mathf.Clamp(particleSystem.startLifetime / 2f, 0.1f, 0.5f);
		particleSystem.maxParticles = Mathf.Clamp((int)(speed * 250), 500, 5000);
		var emission = particleSystem.emission;
		emission.type = ParticleSystemEmissionType.Time;
		emission.rate = Mathf.Clamp(speed * 100, 300f, 1000f);

		particleSystem.Play();
	}
}