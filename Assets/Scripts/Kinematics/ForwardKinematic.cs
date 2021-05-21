using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForwardKinematic : MonoBehaviour
{
	public ForwardKinematicSegment segmentOriginal = null;
	[Range(1, 40)] public int count = 5;
	[Range(0.0f, 10.0f)] public float length = 1;
	[Range(0.0f, 10.0f)] public float width = 1;
	[Range(0.0f, 10.0f)] public float noiseRate = 1;

	List<ForwardKinematicSegment> segments = new List<ForwardKinematicSegment>();

	void Awake()
	{
		Vector2 position = transform.position;
		float angle = transform.eulerAngles.z;

		// base -> end
		ForwardKinematicSegment parent = null;
		for (int i = 0; i < count; i++)
		{
			ForwardKinematicSegment segment = Instantiate(segmentOriginal, transform);
			segment.Initialize(parent, position, angle, length, width);
			segments.Add(segment);

			parent = segment;
		}
	}

	private void Update()
	{
		int count = 1;
		foreach (ForwardKinematicSegment segment in segments)
		{
			segment.length = length;
			segment.width = width;
			segment.noiseRate = noiseRate * ((float)count / (float)segments.Count);
			count++;

			if (segment.parent != null)
			{
				segment.start = segment.parent.end;
			}
			segment.CalculateEnd();
		}
	}
}
