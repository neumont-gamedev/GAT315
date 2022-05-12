using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Simulator : Singleton<Simulator>
{
	[SerializeField] BoolData simulate;
	[SerializeField] IntData fixedFPS;
	[SerializeField] StringData fps;
	[SerializeField] StringData collisionInfo;
	[SerializeField] EnumData broadPhaseType;
	[SerializeField] List<Force> forces;

	public List<Body> bodies { get; set; } = new List<Body>();
	public float fixedDeltaTime => 1.0f / fixedFPS.value;

	BroadPhase[] broadPhases = { new Quadtree(), new BVH(), new NullBroadPhase() };
	BroadPhase broadPhase;
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

		if (!simulate.value) return;

		// add current delta time to time accumulator
		timeAccumulator += Time.deltaTime;

		// apply forces to bodies
		forces.ForEach(force => force.ApplyForce(bodies));

		Vector2 screenSize = GetScreenSize();

		broadPhase = broadPhases[broadPhaseType.value];

		// integrate physics simulation with fixed delta time
		while (timeAccumulator >= fixedDeltaTime)
		{
			// construct broad-phase tree
			broadPhase.Build(new AABB(Vector2.zero, screenSize), bodies);
			var contacts = new List<Contact>();
			Collision.CreateBroadPhaseContacts(broadPhase, bodies, contacts);
			Collision.CreateNarrowPhaseContacts(contacts);

			Collision.SeparateContacts(contacts);
			Collision.ApplyImpulses(contacts);

			bodies.ForEach(body =>
			{
				Integrator.SemiImplicitEuler(body, fixedDeltaTime);
				body.position = body.position.Wrap(-screenSize * 0.5f, screenSize * 0.5f);
			});
			timeAccumulator -= fixedDeltaTime;
		}

		broadPhase.Draw();
		collisionInfo.value = broadPhase.queryResultCount + "/" + bodies.Count;

		// reset body acceleration
		bodies.ForEach(body => body.acceleration = Vector2.zero);
	}

	public Body GetScreenToBody(Vector3 screen)
	{
		Body body = null;

		Ray ray = activeCamera.ScreenPointToRay(screen);
		RaycastHit2D hit = Physics2D.GetRayIntersection(ray);
		if (hit.collider)
		{
			hit.collider.gameObject.TryGetComponent<Body>(out body);
		}

		return body;
	}

	public Vector3 GetScreenToWorldPosition(Vector2 screen)
	{
		Vector2 world = activeCamera.ScreenToWorldPoint(screen);
		return world;
	}

	public Vector2 GetScreenSize()
	{
		return activeCamera.ViewportToWorldPoint(Vector2.one) * 2;
	}

	public void Clear()
	{
		bodies.ForEach(body => Destroy(body.gameObject));
		bodies.Clear();
	}

}
