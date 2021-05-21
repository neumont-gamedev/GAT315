using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForwardKinematicSegment : KinematicSegment
{
	[Range(-90.0f, 90.0f)] public float inputAngle = 0.0f;
	public bool enableNoise = false;

	public float noiseRate { get; set; }

	float baseAngle = 0;
	float noise = 0;

	private void Start()
	{
		noise = Random.value * 100.0f;
	}

	private void Update()
	{
		transform.localScale = Vector3.one * width;
		float localAngle = inputAngle;

		if (enableNoise)
		{
			noise = noise + (noiseRate * Time.deltaTime);
			float t = Mathf.PerlinNoise(noise, 0);
			localAngle = Mathf.Lerp(-90, 90, t);
		}

		angle = (parent != null) ? (localAngle + parent.angle) : (localAngle + baseAngle);
		transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
	}

	public override void Initialize(KinematicSegment parent, Vector2 position, float angle, float length, float width)
	{
		this.parent = parent;
		this.width = width;

		this.angle = angle;
		this.length = length;

		start = position;
		baseAngle = angle;
	}
}
