using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Simulator : Singleton<Simulator>
{
	[SerializeField] IntData fixedFPS;
	[SerializeField] StringData fps;
	[SerializeField] List<Force> forces;

	public List<Body> bodies { get; set; } = new List<Body>();
	public float fixedDeltaTime => 1.0f / fixedFPS.value;

	Camera activeCamera;
	float timeAccumulator = 0;

	private void Start()
	{
		activeCamera = Camera.main;
	}

	private void Update()
	{
		// get fps
		fps.value = (1.0f / Time.deltaTime).ToString("F2");

		// add current delta time to time accumulator
		timeAccumulator += Time.deltaTime;

		// apply forces to bodies
		forces.ForEach(force => force.ApplyForce(bodies));

		// integrate physics simulation with fixed delta time
		while (timeAccumulator >= fixedDeltaTime)
		{
			bodies.ForEach(body =>
			{
				Integrator.SemiImplicitEuler(body, fixedDeltaTime);
			});
			timeAccumulator -= fixedDeltaTime;
		}

		// reset body acceleration
		bodies.ForEach(body => body.acceleration = Vector2.zero);
	}

	public Vector3 GetScreenToWorldPosition(Vector2 screen)
	{
		Vector2 world = activeCamera.ScreenToWorldPoint(screen);
		return world;
	}
}
