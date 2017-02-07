using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BurnableMesh : MonoBehaviour 
{
    [SerializeField] private float m_TriangleLifetime = 0.7f;
	[SerializeField] private float m_RaycastDistanceThreshold = 20.0f;
	[SerializeField] private float m_DistanceThreshold = 0.25f;

    [SerializeField] private bool m_Destructible = true;

	private bool m_Touched = false;

	private Vector3 m_Origin, m_Direction;

	private bool rayCasted = false;
	private MeshFilter m_MeshFilter;
	private MeshCollider m_MeshCollider;

	private float m_MeshDeletionCount,m_UpdateCount;
	private PlayerController m_Player;

	private Mesh m_Mesh;
	private List<int> m_MeshTriangles;

	private List<TimedTriangle> m_BurntTriangles;
	private List<int> m_BurntTriangleIndexes;

    private Material m_Material;
    private Texture2D m_BurnMap;
    [SerializeField] private int m_BurnMapSize = 64;
    [SerializeField] private int m_BurnRadius = 4;

    private void Start ()
    {
        // Mesh stuff
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
    
        // Shader stuff
        m_Material = GetComponent<Renderer>().material;
        m_BurnMap = new Texture2D(m_BurnMapSize, m_BurnMapSize, TextureFormat.RGB24, false);

        Color[] pixels = m_BurnMap.GetPixels();
        for (int i = 0; i < pixels.Length; i++) {
            pixels[i] = Color.black;
        }
        m_BurnMap.SetPixels(pixels);
        m_BurnMap.Apply();

        m_Material.SetTexture("_BurnMap", m_BurnMap);
    }

    void Update ()
	{
        if (m_Touched && m_UpdateCount % 2 == 0) 
		{
            for (int i = m_BurntTriangles.Count - 1; i >= 0; i--)
			{
                TimedTriangle currentTriangle = m_BurntTriangles[i];
                currentTriangle.Update();

                // Burn map
                int xCenter = (int)(m_BurnMapSize * currentTriangle.m_TexCoord.x);
                int yCenter = (int)(m_BurnMapSize * currentTriangle.m_TexCoord.y);

                if (xCenter > m_BurnMapSize) xCenter = m_BurnMapSize;
                else if (xCenter  < 0)        xCenter = m_BurnRadius;
                if (yCenter  > m_BurnMapSize) yCenter = m_BurnMapSize ;
                else if (yCenter  < 0)        yCenter = m_BurnRadius;

                Color[] pixels = m_BurnMap.GetPixels(xCenter, yCenter, m_BurnRadius * 2, m_BurnRadius * 2);

                for (int index = 0, x = -m_BurnRadius, y = -m_BurnRadius; index < pixels.Length; index++, x++) {
                    if (x == m_BurnRadius) {
                        y++;
                        x = 0;
                    }

                    float distanceFactor = 1 - (Vector2.Distance(
                        new Vector2(xCenter, yCenter),
                        new Vector2(xCenter + x, yCenter + y)
                    ) / m_BurnRadius);

                    float lifetimeFactor = 1.66f - (currentTriangle.m_Lifetime / m_TriangleLifetime);

                    float current = pixels[index].grayscale;
                    float value = (current + distanceFactor) / 2 * lifetimeFactor;
                    
                    pixels[index] = new Color(value, value, value);
                }

                m_BurnMap.SetPixels(xCenter - m_BurnRadius, yCenter - m_BurnRadius, m_BurnRadius * 2, m_BurnRadius * 2, pixels);
                m_BurnMap.Apply();

                // Mesh
                if (m_Destructible) {
                    if (currentTriangle.MarkedForDeletion && m_MeshDeletionCount <= 1) {
                        DestroyTriangle(currentTriangle);
                        m_BurntTriangles.RemoveAt(i);
                        m_MeshDeletionCount++;
                    }
                }
			}
		}
		else {
            m_UpdateCount++;
        }

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

            Vector2 texCoord = (m_Mesh.uv[indexes[0]] + m_Mesh.uv[indexes[1]] + m_Mesh.uv[indexes[2]]) / 3;

            TimedTriangle triangle = new TimedTriangle(vertices, time, texCoord);
			m_BurntTriangles.Add(triangle);
    	}
	}


	void DestroyTriangle (TimedTriangle triangle)
	{
		int i = 0;

		Vector3 point0, point1, point2;

		while (i < m_MeshTriangles.Count)
		{
			int nEqual = 0;

			point0 = m_Mesh.vertices[m_MeshTriangles[i]];
			point1 = m_Mesh.vertices[m_MeshTriangles[i + 1]];
			point2 = m_Mesh.vertices[m_MeshTriangles[i + 2]];

			if (Vector3.Distance(point0,triangle.m_Average) < m_DistanceThreshold) nEqual++;
		    if (Vector3.Distance(point1, triangle.m_Average) < m_DistanceThreshold) nEqual++;
			if (Vector3.Distance(point2, triangle.m_Average) < m_DistanceThreshold) nEqual++;

			// Check if it's the triangle we want to delete
			if (nEqual >= 1) {
                m_MeshTriangles.RemoveRange(i, 3);
            }

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

		MeshFilter filter = (MeshFilter) gameObject.GetComponent<MeshFilter>();

		if (filter && filter.mesh.normals.Length > 0)
			m_Direction = -filter.transform.TransformDirection(filter.mesh.normals[0]);

		Debug.DrawRay(m_Origin, m_Direction, Color.green);

		if (Physics.Raycast(m_Origin, m_Direction, out hit, m_RaycastDistanceThreshold)) 
		{
			if (hit.triangleIndex != -1 && !m_BurntTriangleIndexes.Contains(hit.triangleIndex))
			{
				AddTriangle(hit.triangleIndex, m_TriangleLifetime);
				m_BurntTriangleIndexes.Add(hit.triangleIndex);
            }

			m_Player.wall = true;
			m_Player.GetComponent<Rigidbody>().useGravity = false;
		}
		else
		{
			m_Player.wall = false;
			m_Player.GetComponent<Rigidbody>().useGravity = true;
		}
	 }
}
