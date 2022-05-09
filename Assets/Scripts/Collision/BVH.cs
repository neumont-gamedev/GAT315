using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BVH : BroadPhase
{
	BVHNode rootNode;

	public override void Build(AABB aabb, List<Body> bodies)
	{
		List<Body> sorted = new List<Body>(bodies);
		sorted.OrderBy(body => (body.position.x));

		// create bvh root node
		rootNode = new BVHNode(sorted);
	}

	public override void Query(AABB aabb, List<Body> results)
	{
		rootNode.Query(aabb, results);
	}

	public override void Query(Body body, List<Body> results)
	{
		Query(body.shape.GetAABB(body.position), results);
	}

	public override void Draw()
	{
		rootNode?.Draw();
	}
}
