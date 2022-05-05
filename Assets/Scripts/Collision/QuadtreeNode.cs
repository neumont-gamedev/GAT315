using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadtreeNode
{
	AABB nodeAABB;
	int nodeCapacity;
	List<Body> nodeBodies = new List<Body>();

	QuadtreeNode northeast; // = null
	QuadtreeNode northwest;
	QuadtreeNode southeast;
	QuadtreeNode southwest;
	bool subdivided = false;

	public QuadtreeNode(AABB aabb, int capacity)
	{
		nodeAABB = aabb;
		nodeCapacity = capacity;
	}

	public void Insert(Body body)
	{
		// check if with node AABB
		if (!nodeAABB.Contains(body.shape.GetAABB(body.position))) return;

		// check if within capacity
		if (nodeBodies.Count < nodeCapacity)
		{
			nodeBodies.Add(body);
		}
		else
		{
			// subdivide
		}

	}


}
