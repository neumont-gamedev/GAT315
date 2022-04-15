using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Circle
{
	public Vector2 center;
	public float radius;

	public Circle(Vector2 center, float radius)
	{
		this.center = center;
		this.radius = radius;
	}

	public Circle(Body body)
	{
		center = body.position;
		radius = ((CircleShape)body.shape).radius;
	}

	public static bool Intersects(Circle circleA, Circle circleB)
	{
		Vector2 direction = circleA.center - circleB.center;
		float sqrDistance = direction.sqrMagnitude;
		float radius = circleA.radius + circleB.radius;

		return (sqrDistance <= (radius * radius));
	}
}
