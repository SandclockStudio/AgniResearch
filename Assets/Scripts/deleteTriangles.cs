using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deleteTriangles : MonoBehaviour 
{

    // Use this for initialization
    GameObject player,plane;
	Vector3 origin,direction;
	List<MyTriangle> trianglesList = new List<MyTriangle>();
	List<int> indexs = new List<int>();

	public GameObject meshToDelete;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

	void Update()
	{
		for(int i = 0; i < trianglesList.Count; i++)
		{
			trianglesList[i].update();
			if(trianglesList[i].getTime() <= 0 && trianglesList[i].delete == false)
			{
				destroyTriangle(trianglesList[i].getTriangles());
				trianglesList[i].delete = true;
			}
		}
	}

	void addTriangle (int index,float time)
    {
		
		Mesh mesh = transform.GetComponent<MeshFilter>().mesh;
		int[] meshTriangles = mesh.triangles;
		int[] triangles = new int[3];

		triangles[0] = meshTriangles[(index*3)];
		triangles[1] = meshTriangles[(index*3)+1];
		triangles[2] = meshTriangles[(index*3)+2];

		MyTriangle triangle = new MyTriangle(triangles,time);
		trianglesList.Add(triangle);
    }


	void destroyTriangle(int [] triangles)
	{
		Mesh mesh = meshToDelete.transform.GetComponent<MeshFilter>().mesh;
		int[] oldTriangles = mesh.triangles;

		int[] newTriangles = new int[mesh.triangles.Length - 3];

		int j = 0;
		int i = 0;
	
		bool hola = true;

		while(j < mesh.triangles.Length-3)
		{	
			
			if(triangles[0] != oldTriangles[j] && triangles[1] != oldTriangles[j+1] && triangles[2] != oldTriangles[j+2] )
			{
				
				newTriangles[i++] = oldTriangles[j++];
				newTriangles[i++] = oldTriangles[j++];
				newTriangles[i++] = oldTriangles[j++];

			}
			else
			{
				j+=3;
			}
		}

		Destroy(meshToDelete.gameObject.GetComponent<MeshCollider>());
		meshToDelete.transform.GetComponent<MeshFilter>().mesh.triangles = newTriangles;
		meshToDelete.gameObject.AddComponent<MeshCollider>();
	}

	void OnCollisionStay(Collision col) 
	{
		if (col.rigidbody.tag == "Player")
		{
			RaycastHit hit;

			origin = player.transform.position;

			MeshFilter filter = (MeshFilter)gameObject.GetComponent<MeshFilter>();

			if(filter && filter.mesh.normals.Length > 0)
				direction = -filter.transform.TransformDirection(filter.mesh.normals[0]);
		

			if (Physics.Raycast(origin,direction, out hit, 20.0f) && !indexs.Contains(hit.triangleIndex)) 
			{
				addTriangle(hit.triangleIndex,0.7f);
				indexs.Add(hit.triangleIndex);
			}
		 }
	 }
}
