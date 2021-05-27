using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InverseKinematicSegment : KinematicSegment
{
	private void Update()
	{
        transform.localScale = Vector2.one * size;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

	public override void Initialize(KinematicSegment parent, Vector2 position, float angle, float length, float size)
	{
        this.parent = parent;
        this.size = size;

        polar = new Coordinate.Polar{ angle = angle, length = length };
        start = position;
    }

    public void Follow(Vector2 target)
    {
        Vector2 direction = (target - start).normalized * length;

        polar = Coordinate.CartesianToPolar(direction);
        start = target - direction;
    }

}
