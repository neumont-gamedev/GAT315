using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Circle
{
	public Vector2 center { get; set; }
	public float radius { get; set; }

	public Circle(Vector2 center, float radius)
	{
		this.center = center;
		this.radius = radius;
	}

	public bool Contains(Vector2 point)
	{
		Vector2 direction = center - point;
		float sqrDistance = direction.sqrMagnitude;
		float sqrRadius = (radius * radius);

		return (sqrDistance <= sqrRadius);
	}

	public bool Contains(Circle circle)
	{
		Vector2 direction = center - circle.center;
		float sqrDistance = direction.sqrMagnitude;
		float sqrRadius = ((radius + circle.radius) * (radius + circle.radius));

		return (sqrDistance <= sqrRadius);
	}
	/*	
		public bool Contains(AABB aabb)
		{
			Vector2 direction = aabb.center - center;
			Vector2 point = center + (direction.normalized * radius);

			return (aabb.Contains(point));
		}*/
}
