using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurnableMeshManager : MonoBehaviour {

	private Vector3 m_Origin, m_Direction;
	private GameObject player;
	private Collider[] m_myColliders;

	void Start()
	{
		player = GameObject.FindGameObjectWithTag("Player");
	}

	// Update is called once per frame
	void Update () 
	{

		m_Origin = player.transform.position;
	
		//m_Direction = transform.TransformDirection(Vector3.back);

		MeshFilter filter = (MeshFilter)gameObject.GetComponentInChildren<MeshFilter>();

		if (filter && filter.mesh.normals.Length > 0)
			m_Direction = -filter.transform.TransformDirection(filter.mesh.normals[0]);

		Debug.DrawRay(m_Origin, m_Direction, Color.green);

		m_myColliders = Physics.OverlapSphere(m_Origin,0.25f);


		for(int i = 0; i < m_myColliders.Length; i++)
		{

			if (m_myColliders[i].CompareTag("Wall"))
				
				m_myColliders[i].gameObject.GetComponent<BurnableMesh>().enabled = true;
			
		}
	}
}
