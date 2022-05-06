using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Simulator : Singleton<Simulator>
{
	[SerializeField] BoolData simulate;
	[SerializeField] IntData fixedFPS;
	[SerializeField] StringData fps;
	[SerializeField] List<Force> forces;

	public List<Body> bodies { get; set; } = new List<Body>();
	public float fixedDeltaTime => 1.0f / fixedFPS.value;

	BroadPhase broadPhase = new Quadtree();
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

		// integrate physics simulation with fixed delta time
		while (timeAccumulator >= fixedDeltaTime)
		{
			// construct broad-phase tree
			broadPhase.Build(new AABB(Vector2.zero, GetScreenSize()), bodies);

			//bodies.ForEach(body => body.shape.color = Color.white);
			Collision.CreateContacts(bodies, out var contacts);
			//contacts.ForEach(contact => 
			//{ 
			//	contact.bodyA.shape.color = Color.red; 
			//	contact.bodyB.shape.color = Color.red; 
			//});
			Collision.SeparateContacts(contacts);
			Collision.ApplyImpulses(contacts);

			bodies.ForEach(body =>
			{
				Integrator.SemiImplicitEuler(body, fixedDeltaTime);
				body.position = body.position.Wrap(-GetScreenSize() * 0.5f, GetScreenSize() * 0.5f);
				//body.shape.GetAABB(body.position).Draw(Color.white);
			});
			timeAccumulator -= fixedDeltaTime;
		}

		broadPhase.Draw();

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
