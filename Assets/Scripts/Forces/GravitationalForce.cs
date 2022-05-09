using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravitationalForce : Force
{
	[SerializeField] FloatData gravitation;

	public override void ApplyForce(List<Body> bodies)
	{
		if (gravitation.value == 0) return;

		for (int i = 0; i < bodies.Count; i++)
		{
			for (int j = i + 1; j < bodies.Count; j++)
			{
				Body bodyA = bodies[i];
				Body bodyB = bodies[j];

				Vector2 direction = bodyA.position - bodyB.position;
				float distance = Mathf.Max(1, direction.magnitude);
				float force = gravitation.value * (bodyA.mass * bodyB.mass) / distance;

				bodyA.ApplyForce(-direction.normalized * force, Body.eForceMode.Force);
				bodyB.ApplyForce( direction.normalized * force, Body.eForceMode.Force);
			}
		}
	}
}
