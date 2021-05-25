using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForwardKinematic : MonoBehaviour
{
    public ForwardKinematicSegment original;
    public int count = 5;
    [Range(0.1f, 3.0f)] public float size = 1;
    [Range(0.1f, 3.0f)] public float length = 1;

    List<ForwardKinematicSegment> segments = new List<ForwardKinematicSegment>();

	private void Start()
	{
        KinematicSegment parent = null;
		for (int i = 0; i < count; i++)
		{
            var segment = Instantiate(original, transform);
            segment.Initialize(parent, transform.position, 0, length, size);

            segments.Add(segment);
            parent = segment;
		}
	}

	void Update()
    {
        foreach (ForwardKinematicSegment segment in segments)
        {
            segment.length = length;
            segment.size = size;

            if (segment.parent != null)
            {
                segment.start = segment.parent.end;
            }
        }
    }


}
