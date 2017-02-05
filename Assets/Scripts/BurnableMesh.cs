using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BurnableMesh : MonoBehaviour 
{
    [SerializeField] private float m_TriangleLifetime = 0.7f;
	[SerializeField] private float m_RaycastDistanceThreshold = 20.0f;
	[SerializeField] private float m_DistanceThreshold = 0.25f;

	private bool m_Touched = false;

	private Vector3 m_Origin, m_Direction;

	private MeshFilter m_MeshFilter;
	private MeshCollider m_MeshCollider;

	private float m_MeshDeletionCount,m_UpdateCount;
	private PlayerController m_Player;

	private Mesh m_Mesh;
	private List<int> m_MeshTriangles;

	private List<TimedTriangle> m_BurntTriangles;
	private List<int> m_BurntTriangleIndexes;

    private Material dissolveMaterial = null;
    private float timeScale = 0.6f;
    private float value = 1.0f;

    private void Start ()
    {
        m_MeshFilter = GetComponent<MeshFilter>();
		m_MeshCollider = GetComponent<MeshCollider>();

		m_Mesh = m_MeshFilter.mesh;
		m_Mesh.MarkDynamic();
		m_Player = GameObject.FindObjectOfType<PlayerController>();
		m_MeshTriangles = new List<int>(m_Mesh.triangles);

		m_BurntTriangles = new List<TimedTriangle>();
		m_BurntTriangleIndexes = new List<int>();
		m_MeshDeletionCount = 0;
		m_UpdateCount = 0;
        //shaders rules
        dissolveMaterial = GetComponent<Renderer>().material; 
    }

	void Update ()
	{
		if (m_Touched && m_UpdateCount%2 == 0) 
		{ 
			

			for (int i = m_BurntTriangles.Count - 1; i >= 0; i--)
			{
				m_BurntTriangles[i].Update();

				m_BurntTriangles = m_BurntTriangles.OrderBy(x => x.m_Lifetime).ToList();

                value = Mathf.Max(0.8f, value - Time.deltaTime * timeScale);
                dissolveMaterial.SetFloat("_DissolveValue", value);

                if (m_BurntTriangles[i].MarkedForDeletion && m_MeshDeletionCount <= 1 ) 
				{
                    DestroyTriangle(m_BurntTriangles[i]);
			    	m_BurntTriangles.RemoveAt(i);
					m_MeshDeletionCount++;
			    }
			}
		}
		else
			m_UpdateCount++;
		
		m_MeshDeletionCount = 0;
	}

	void AddTriangle (int index, float time)
    {
		if (index * 3 + 2 <= m_MeshTriangles.Count) {

			int[] indexes = new int[] {
				m_MeshTriangles[(index * 3)],
				m_MeshTriangles[(index * 3) + 1],
				m_MeshTriangles[(index * 3) + 2]
			};

			Vector3[] vertices = new Vector3[]{
				m_Mesh.vertices[indexes[0]],
				m_Mesh.vertices[indexes[1]],
				m_Mesh.vertices[indexes[2]]
			};

			TimedTriangle triangle = new TimedTriangle(indexes, vertices, time,index);
			m_BurntTriangles.Add(triangle);
    	}
	}


	void DestroyTriangle (TimedTriangle triangle)
	{
		int i = 0;



		Vector3 point0,point1,point2;

		while (i < m_MeshTriangles.Count)
		{
			int nEqual = 0;

			point0 = m_Mesh.vertices[m_MeshTriangles[i]];
			point1 = m_Mesh.vertices[m_MeshTriangles[i + 1]];
			point2 = m_Mesh.vertices[m_MeshTriangles[i + 2]];

			if (Vector3.Distance(point0,triangle.average) < m_DistanceThreshold) nEqual++;
		    if (Vector3.Distance(point1, triangle.average) < m_DistanceThreshold) nEqual++;
			if (Vector3.Distance(point2, triangle.average) < m_DistanceThreshold) nEqual++;

			// Check if it's the triangle we want to delete
			if (nEqual >= 1)
				m_MeshTriangles.RemoveRange(i, 3);
			
				i += 3;
		}

		m_Mesh.triangles = m_MeshTriangles.ToArray();
		m_MeshCollider.sharedMesh = m_Mesh;
    }


	void OnCollisionEnter (Collision collision) 
	{
		if (collision.gameObject.CompareTag("Player"))
		{
			m_Touched = true;
			collision.rigidbody.velocity = Vector3.zero;
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

		RaycastHit hit;

		m_Origin = collision.transform.position;

		MeshFilter filter = (MeshFilter)gameObject.GetComponent<MeshFilter>();

		if (filter && filter.mesh.normals.Length > 0)
			m_Direction = -filter.transform.TransformDirection(filter.mesh.normals[0]);

		Debug.DrawRay(m_Origin, m_Direction, Color.green);

		if (Physics.Raycast(m_Origin, m_Direction, out hit, m_RaycastDistanceThreshold)) 
		{
			if (hit.triangleIndex != -1 && !m_BurntTriangleIndexes.Contains(hit.triangleIndex))
			{
				AddTriangle(hit.triangleIndex, m_TriangleLifetime);
				m_BurntTriangleIndexes.Add(hit.triangleIndex);
                dissolveMaterial.SetVector("_HitPos", (new Vector4(m_Origin.x, m_Origin.y, m_Origin.z, 1.0f)));
            }

			m_Player.wall = true;
		}
		else
		{
			m_Player.wall = false;
		}
	 }
}
