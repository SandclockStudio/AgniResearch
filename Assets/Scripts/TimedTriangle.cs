using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedTriangle 
{
	private int[] m_Indexes;

	private Vector3[] m_Vertices;

	public float m_Lifetime;

	public Vector3 average;

	public int m_Index;

	public bool MarkedForDeletion {
		get {
			return m_Lifetime <= 0;
		}
	}

	public TimedTriangle (int[] indexes, Vector3[] vertices, float targetTime, int index)
	{
		m_Indexes = indexes;
		m_Vertices = vertices;
		m_Lifetime = targetTime;
		m_Index= index;
		average = (vertices[0] + vertices[1] + vertices[2]) / 3;
	}

	public void Update ()
	{
		m_Lifetime -= Time.deltaTime;
	}

	public float GetTime ()
	{
		return m_Lifetime;
	}


	public int[] GetIndexes ()
	{
		return m_Indexes;
	}

	public Vector3[] GetVertices ()
	{
		return m_Vertices;
	}
		

}
