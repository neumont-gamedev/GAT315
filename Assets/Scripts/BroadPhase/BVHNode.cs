using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BVHNode
{
	public AABB aabb;
	public List<Body> bodies;
	public BVHNode left;
	public BVHNode right;

	public BVHNode(List<Body> bodies)
	{
		this.bodies = bodies;

		ComputeBoundary();
		Split();
	}

	public void Query(AABB aabb, List<Body> bodies)
	{
		// check if aabb is within node boundary, if not exit
		if (!this.aabb.Contains(aabb)) return;

		// add bodies if node bodies exist
		if (this.bodies.Count > 0)
		{
			bodies.AddRange(this.bodies);
		}

		// query left/right nodes with aabb
		left?.Query(aabb, bodies);
		right?.Query(aabb, bodies);
	}

	public void ComputeBoundary()
	{
		if (bodies.Count == 0) return;

		aabb.center = bodies[0].position;
		aabb.size = Vector3.zero;

		bodies.ForEach(body => this.aabb.Expand(body.shape.aabb));
	}

	public void Split()
	{
		int count = bodies.Count;
		int half = count / 2;
		if (half >= 1)
		{
			left = new BVHNode(bodies.GetRange(0, half));
			right = new BVHNode(bodies.GetRange(half, half + (count % 2)));

			bodies.Clear();
		}
	}

	public void Draw()
	{
		aabb.Draw(Color.white);

		left?.Draw();
		right?.Draw();
	}
}
