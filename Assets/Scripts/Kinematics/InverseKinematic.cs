using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InverseKinematic : MonoBehaviour
{
	[SerializeField] InverseKinematicSegment segmentOriginal = null;
	[Range(1, 40)] public int count = 5;
	[Range(0.0f, 10.0f)] public float length = 1;
	[Range(0.0f, 10.0f)] public float width = 1;

	public Transform target;
	public Transform anchor;

	List<InverseKinematicSegment> segments = new List<InverseKinematicSegment>();

	void Awake()
	{
		// end -> base
		InverseKinematicSegment parent = null;
		for (int i = 0; i < count; i++)
		{
			InverseKinematicSegment segment = Instantiate(segmentOriginal, transform);
			segment.Initialize(parent, Vector2.zero, 0, length, width);
			segments.Add(segment);

			parent = segment;
		}
	}

	private void Update()
	{
		foreach (InverseKinematicSegment segment in segments)
		{
			segment.length = length;
			segment.width = width;

			Vector2 position = (segment.parent) ? segment.parent.start : (Vector2)target.position;
			segment.Follow(position);
		}

		if (anchor)
		{
			int first_index = segments.Count - 1;

			segments[first_index].start = anchor.position;
			segments[first_index].CalculateEnd();

			for (int i = first_index - 1; i >= 0; i--)
			{
				segments[i].start = segments[i + 1].end;
				segments[i].CalculateEnd();
			}
		}
	}
}
