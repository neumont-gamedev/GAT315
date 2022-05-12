using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BVH : BroadPhase
{
	BVHNode rootNode;

	public override void Build(AABB aabb, List<Body> bodies)
	{
		queryResultCount = 0;
		List<Body> sorted = bodies.OrderBy(body => (body.position.x)).ToList();

		// create bvh root node
		rootNode = new BVHNode(sorted, 0);
	}

	public override void Query(AABB aabb, List<Body> results)
	{
		rootNode.Query(aabb, results);
		queryResultCount += results.Count;
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
