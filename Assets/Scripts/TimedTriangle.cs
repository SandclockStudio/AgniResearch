using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedTriangle 
{
    private Vector3[] m_Vertices;
	public float m_Lifetime;
	public Vector3 m_Average;
    public Vector2 m_TexCoord;

	public bool MarkedForDeletion {
		get {
			return m_Lifetime <= 0;
		}
	}

	public TimedTriangle (Vector3[] vertices, float targetTime, Vector2 texCoord)
	{
		m_Vertices = vertices;
		m_Lifetime = targetTime;
		m_Average = (vertices[0] + vertices[1] + vertices[2]) / 3;
        m_TexCoord = texCoord;

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
