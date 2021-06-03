using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KinematicTarget : MonoBehaviour
{
	public Camera m_camera = null;

	private void Start()
	{
		m_camera = (m_camera == null) ? Camera.main : m_camera;
	}

	void Update()
	{
		if (Input.GetMouseButton(0))
		{
			transform.position = m_camera.ScreenToWorldPoint(Input.mousePosition);
		}
	}
}
