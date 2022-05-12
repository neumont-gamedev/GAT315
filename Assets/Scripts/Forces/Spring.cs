using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spring
{
	public Body bodyA { get; set; }
	public Body bodyB { get; set; }

	public float restLength { get; set; }
	public float k { get; set; }

	public void ApplyForce()
	{
		Vector2 force = Force(bodyA.position, bodyB.position, restLength, k);

		bodyA.ApplyForce( force, Body.eForceMode.Acceleration);
		bodyB.ApplyForce(-force, Body.eForceMode.Acceleration);

		Debug.DrawLine(bodyA.position, bodyB.position);
	}

	// force to move position towards anchor when length > rest length
	static public Vector2 Force(Vector2 position, Vector2 anchor, float restLength, float k)
	{
		Vector2 direction = position - anchor; // direction = position <-- anchor
		float length = direction.magnitude;
		float x = length - restLength; // displacement of spring from resting length
		float f = -k * x; // force = k (stiffness) * x (displacement)

		return f * direction.normalized;
	}
}
