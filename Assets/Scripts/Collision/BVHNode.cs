using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BVHNode
{
	AABB nodeAABB;
	int nodeLevel;
	List<Body> nodeBodies = new List<Body>();

	BVHNode left;
	BVHNode right;

	public BVHNode(List<Body> bodies, int level)
	{
		nodeBodies = bodies;
		nodeLevel = level;

		ComputeBoundary();
		Split();
	}

	public void ComputeBoundary()
	{
		if (nodeBodies.Count == 0) return;

		nodeAABB.center = nodeBodies[0].position;
		nodeAABB.size = Vector2.zero;

		nodeBodies.ForEach(body => nodeAABB.Expand(body.shape.GetAABB(body.position)));
	}

	public void Split()
	{
		int length = nodeBodies.Count;
		int half = length / 2;

		if (half >= 1)
		{
			left = new BVHNode(nodeBodies.GetRange(0, half), nodeLevel + 1);
			right = new BVHNode(nodeBodies.GetRange(half, length - half), nodeLevel + 1);

			nodeBodies.Clear();
		}
	}

	public void Query(AABB aabb, List<Body> results)
	{
		// check if query aabb intersects node aabb, return if not
		if (!nodeAABB.Contains(aabb)) return;

		// add intersecting node bodies
		results.AddRange(nodeBodies);

		// query child nodes
		left?.Query(aabb, results);
		right?.Query(aabb, results);
	}

	public void Draw()
	{
		Color color = BroadPhase.colors[nodeLevel % BroadPhase.colors.Length];

		AABB aabb = nodeAABB;
		aabb.size -= Vector2.one * (nodeLevel * 0.15f);
		aabb.Draw(color);

		left?.Draw();
		right?.Draw();
	}
}
