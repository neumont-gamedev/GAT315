using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterDisplacement : MonoBehaviour
{
	[Range(0, 5)] public float amplitude = 1;
	[Range(0, 5)] public float rate = 1;
	[Range(0, 1)] public float scale = 1;
	[Range(0, 20)] public float cellDensity = 10;
	[Range(0, 20)] public float cellOffset = 10;

	MeshRenderer meshRenderer;

	private void Start()
	{
		meshRenderer = GetComponent<MeshRenderer>();
	}

	private void Update()
	{
		meshRenderer.material.SetFloat("_Amplitude", amplitude);
		meshRenderer.material.SetFloat("_Rate", rate);
		meshRenderer.material.SetFloat("_Scale", scale);
		meshRenderer.material.SetFloat("_CellDensity", cellDensity);
		meshRenderer.material.SetFloat("_CellOffset", cellOffset);
	}

}
