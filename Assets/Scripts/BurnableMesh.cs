using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurnableMesh : MonoBehaviour 
{
    [SerializeField] private float m_TriangleLifetime = 0.7f;
	[SerializeField] private float m_DistanceThreshold = 1f;

	private bool m_Touched = false;

	private Vector3 m_Origin, m_Direction;

	private MeshFilter m_MeshFilter;
	private MeshCollider m_MeshCollider;

	private Mesh m_Mesh;
	private List<int> m_MeshTriangles;

	private List<TimedTriangle> m_BurntTriangles;
	private List<int> m_BurntTriangleIndexes;

    private void Start ()
    {
        m_MeshFilter = GetComponent<MeshFilter>();
		m_MeshCollider = GetComponent<MeshCollider>();

		m_Mesh = m_MeshFilter.mesh;
		m_Mesh.MarkDynamic();

		m_MeshTriangles = new List<int>(m_Mesh.triangles);

		m_BurntTriangles = new List<TimedTriangle>();
		m_BurntTriangleIndexes = new List<int>();
    }

	void Update ()
	{
		if (m_Touched) {
			for (int i = m_BurntTriangles.Count - 1; i >= 0; i--)
			{
				m_BurntTriangles[i].Update();
			
			    if (m_BurntTriangles[i].MarkedForDeletion) {
					DestroyTriangle(m_BurntTriangles[i]);
			    	m_BurntTriangles.RemoveAt(i);
			    }
			}
		}
	}

	void AddTriangle (int index, float time)
    {
		int[] vertices = new int[] {
			m_MeshTriangles[(index * 3)],
			m_MeshTriangles[(index * 3) + 1],
			m_MeshTriangles[(index * 3) + 2]
		};

		TimedTriangle triangle = new TimedTriangle(vertices, time);
		m_BurntTriangles.Add(triangle);
    }


	void DestroyTriangle (TimedTriangle triangle)
	{
		int i = 0;
		bool found = false;

		while (i < m_MeshTriangles.Count && !found)
		{
			// Check if it's the triangle we want to delete
			if (triangle.GetVertices()[0] == m_MeshTriangles[i]
			 && triangle.GetVertices()[1] == m_MeshTriangles[i + 1]
			 && triangle.GetVertices()[2] == m_MeshTriangles[i + 2] )
			{

				// Remove this triangle
				m_MeshTriangles.RemoveRange(i, 3);
				found = true;
			}

			i += 3;
		}

		m_Mesh.triangles = m_MeshTriangles.ToArray();
		m_Mesh.RecalculateBounds();
		m_Mesh.UploadMeshData(false);

		m_MeshFilter.mesh = m_Mesh;

		m_MeshCollider.sharedMesh = m_Mesh;
    }


	void OnCollisionEnter (Collision collision) 
	{
		if (collision.gameObject.CompareTag("Player"))
		{
			m_Touched = true;
		}
	}

	void OnCollisionStay (Collision collision) 
	{
		if (collision.gameObject.CompareTag("Player"))
		{
			PerformRaycast(collision);
		}
	}

	void PerformRaycast (Collision collision) {

		// Loop through all the contact points from the collision
			foreach (ContactPoint cp in collision.contacts) {
				RaycastHit hit;

				// Check if the normals are facing the character
				if (Vector3.Angle(cp.normal, collision.transform.position) >= 180) {
					m_Origin = collision.transform.position;
					m_Direction = cp.normal - cp.point;
				}
				// if not, perform the raycast towards the normal
				else {
					m_Origin = cp.point + cp.normal;
					m_Direction = -cp.normal;
				}

				Debug.DrawRay(m_Origin, m_Direction, Color.green);


				if (Physics.Raycast(m_Origin, m_Direction, out hit, m_DistanceThreshold))
	            {
					if (hit.triangleIndex != -1 && !m_BurntTriangleIndexes.Contains(hit.triangleIndex))
	            	{
						AddTriangle(hit.triangleIndex, m_TriangleLifetime);
						m_BurntTriangleIndexes.Add(hit.triangleIndex);
	            	}
				}
			}
	 }
}
