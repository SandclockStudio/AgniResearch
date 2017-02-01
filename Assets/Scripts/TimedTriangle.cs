using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedTriangle 
{
	private int[] m_Vertices;
	private float m_Lifetime;

	public bool MarkedForDeletion {
		get {
			return m_Lifetime <= 0;
		}
	}

	public TimedTriangle (int[] vertices, float targetTime)
	{
		this.m_Vertices = vertices;
		this.m_Lifetime = targetTime;
	}

	public void Update ()
	{
		m_Lifetime -= Time.deltaTime;
	}

	public float GetTime ()
	{
		return m_Lifetime;
	}

	public int[] GetVertices ()
	{
		return m_Vertices;
	}

}
