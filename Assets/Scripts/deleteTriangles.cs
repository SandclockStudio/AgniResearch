using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteTriangles : MonoBehaviour {

    // Use this for initialization
    GameObject player,plane;
	Vector3 origin,direction;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void DeleteTriangle (int index)
    {
        Destroy(this.gameObject.GetComponent<MeshCollider>());

        Mesh mesh = transform.GetComponent<MeshFilter>().mesh;
        int[] oldTriangles = mesh.triangles;

        int[] newTriangles = new int[mesh.triangles.Length - 3];

        int i = 0;
        int j = 0;

        while(j < mesh.triangles.Length)
        {
            if(j != index * 3)
            {
                newTriangles[i++] = oldTriangles[j++];
                newTriangles[i++] = oldTriangles[j++];
                newTriangles[i++] = oldTriangles[j++];
            }
            else
            {
                j += 3;
            }
        }
        transform.GetComponent<MeshFilter>().mesh.triangles = newTriangles;
        this.gameObject.AddComponent<MeshCollider>();
    }


	void OnCollisionStay(Collider col)
	{
		if (col.attachedRigidbody.tag == "Player")
		{
			RaycastHit hit;

			origin = player.transform.position;

			MeshFilter filter = gameObject.GetComponent(MeshFilter);

			if(filter && filter.mesh.normals.Length > 0)
				direction = -filter.transform.TransformDirection(filter.mesh.normals[0]);
		

			if (Physics.Raycast(origin,direction, out hit, 20.0f))
			{
				DeleteTriangle(hit.triangleIndex);
			}
		}
	}
}
