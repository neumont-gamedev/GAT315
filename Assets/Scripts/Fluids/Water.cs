using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
	[HeaderAttribute("Water")]
	[SerializeField] [Range(0.0f, 1.0f)] float m_damping = 0.04f;
	[SerializeField] [Range(1.0f, 90.0f)] float m_simulationFPS = 60.0f;

	[HeaderAttribute("Mesh")]
	[SerializeField] MeshFilter m_meshFilter = null;
	[SerializeField] MeshCollider m_meshCollider = null;
	[SerializeField] [Range(2, 80)] int m_xMeshVertexNum = 2;
	[SerializeField] [Range(2, 80)] int m_zMeshVertexNum = 2;
	[SerializeField] [Range(1.0f, 80.0f)] float m_xMeshSize = 40.0f;
	[SerializeField] [Range(1.0f, 80.0f)] float m_zMeshSize = 40.0f;

	Mesh m_mesh = null;
	Vector3[] m_vertices;

	float m_simulationTime = 0.0f;
	int frame = 0;

	float[,] m_buffer1;
	float[,] m_buffer2;

	ref float[,] GetPrevious()
	{
		return ref (((frame % 2) == 0) ? ref m_buffer1 : ref m_buffer2);
	}

	ref float[,] GetCurrent()
	{
		return ref (((frame % 2) == 0) ? ref m_buffer2 : ref m_buffer1);
	}

	void Start()
	{
		m_mesh = m_meshFilter.mesh;
		MeshGenerator.Plane(m_meshFilter, m_xMeshSize, m_zMeshSize, m_xMeshVertexNum, m_zMeshVertexNum);
		m_vertices = m_mesh.vertices;

		m_buffer1 = new float[m_xMeshVertexNum, m_zMeshVertexNum];
		m_buffer2 = new float[m_xMeshVertexNum, m_zMeshVertexNum];
	}

	void Update()
	{
		m_simulationTime = m_simulationTime + Time.deltaTime;

		// update simulation
		while (m_simulationTime > (1.0f / m_simulationFPS))
		{
			float[,] previous = GetPrevious();
			float[,] current = GetCurrent();

			UpdateSimulation(ref previous, ref current);
			frame++;

			m_simulationTime = m_simulationTime - (1.0f / m_simulationFPS);
		}

		// set vertices height from current buffer
		for (int x = 0; x < m_xMeshVertexNum; x++)
		{
			for (int z = 0; z < m_zMeshVertexNum; z++)
			{
				float[,] current = GetCurrent();
				m_vertices[x + (z * m_xMeshVertexNum)].y = current[x, z];
			}
		}

		// recalculate mesh with new vertices
		m_mesh.vertices = m_vertices;
		m_mesh.RecalculateNormals();
		m_mesh.RecalculateTangents();
		m_mesh.RecalculateBounds();
		m_meshCollider.sharedMesh = m_mesh;
	}

	void UpdateSimulation(ref float[,] previous, ref float[,] current)
	{
		for (int x = 1; x < m_xMeshVertexNum-1; x++)
		{
			for (int z = 1; z < m_zMeshVertexNum-1; z++)
			{
				float value = previous[x, z + 1] +
							  previous[x, z - 1] +
							  previous[x - 1, z] +
							  previous[x + 1, z] +
							  previous[x + 1, z + 1] +
							  previous[x - 1, z + 1] +
							  previous[x + 1, z - 1] +
							  previous[x - 1, z - 1];

				value = value / 4.0f;
				value = value - current[x, z];
				value = value * Mathf.Pow(m_damping, Time.deltaTime);
				current[x, z] = value;
			}
		}
	}

	public void Touch(Ray ray, float strength, bool apply)
	{
		RaycastHit raycastHit;
		if (Physics.Raycast(ray, out raycastHit))
		{
			MeshCollider meshCollider = raycastHit.collider as MeshCollider;
			if (meshCollider == m_meshCollider)
			{
				int[] triangles = m_mesh.triangles;
				if (apply)
				{
					int index = triangles[raycastHit.triangleIndex * 3 + 0];
					int x = index % m_xMeshVertexNum;
					int z = index / m_xMeshVertexNum;

					if (x > 1 && x < m_xMeshVertexNum - 1 && z > 1 && z < m_zMeshVertexNum - 1)
					{
						float[,] current = GetCurrent();
						current[x, z] = strength;
					}
				}
			}
		}
	}
}
