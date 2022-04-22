using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointEffector : Force
{
	public enum eEffectorMode
	{
		Constant,
		InverseLinear,
		InverseSquared
	}

	public Shape shape;

	public Vector2 position { get => transform.position; set => transform.position = value; }
	float forceMagnitude { get; set; }
	eEffectorMode effectorMode { get; set; }

	public override void ApplyForce(List<Body> bodies)
	{
		bodies.ForEach(body =>
		{
			if (Collision.TestOverlap(body.shape, body.position, shape, position))
			{
				Vector2 direction = body.position - position;
				float distance = direction.magnitude;
				float t = distance / ((CircleShape)shape).radius;
				Vector2 force = direction.normalized;

				switch (effectorMode)
				{
					case eEffectorMode.Constant:
						force = force * forceMagnitude;
						break;
					case eEffectorMode.InverseLinear:
						force = force * ((1 - t) * forceMagnitude);
						break;
					case eEffectorMode.InverseSquared:
						force = force * ((1 - t) * (1 - t)) * forceMagnitude;
						break;
				}
				body.ApplyForce(force, Body.eForceMode.Force);
			}
		});
	}
}
