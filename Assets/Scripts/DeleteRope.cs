using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteRope : MonoBehaviour
{

    // Use this for initialization
    GameObject player;
	public float  originalChangeX, originalChangeY;
	public float changeX, changeY;
	public float angle;
    void Start()
    {
		angle = transform.localRotation.eulerAngles.z;
		changeX = originalChangeX;
		changeY = originalChangeY;
		//Time.timeScale = 0.2f;
    }

    void DeleteRopes()
    {
   
        Destroy(gameObject, 0.5f);
    }

    private void OnCollisionExit(Collision collision)
    {
		if(collision.rigidbody != null)
			if (collision.rigidbody.tag == "Player")
	        {
	            DeleteRopes();
	        }
    }
}
