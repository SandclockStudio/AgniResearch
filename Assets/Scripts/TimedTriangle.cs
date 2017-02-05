using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedTriangle 
{
	private Vector3[] m_Vertices;
	public float m_Lifetime;
	public Vector3 average;

	public bool MarkedForDeletion {
		get {
			return m_Lifetime <= 0;
		}
	}

	public TimedTriangle (Vector3[] vertices, float targetTime)
	{
		m_Vertices = vertices;
		m_Lifetime = targetTime;
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
    
	public Vector3[] GetVertices ()
	{
		return m_Vertices;
	}
		

}
