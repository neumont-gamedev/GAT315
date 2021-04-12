using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleShape : Shape
{
	public override eType type => eType.Circle;
	public override float size { get => transform.localScale.x * 0.5f; set => transform.localScale = Vector2.one * value; }
	public override float mass => (Mathf.PI * size * size) * density;
}
