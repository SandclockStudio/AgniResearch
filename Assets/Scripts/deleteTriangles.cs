using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deleteTriangles : MonoBehaviour {

    // Use this for initialization
    GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void deleteTri(int index)
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
	
	// Update is called once per frame
	void Update ()
    {
        
		if(Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 1000.0f))
            {
                deleteTri(hit.triangleIndex);
            }
        }
        

	}
}
