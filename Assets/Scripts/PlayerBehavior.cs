using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour {

    private Rigidbody rb;
    public float speed;
    private bool wall;

    // Use this for initialization
    void Start ()
    {
        rb = GetComponent<Rigidbody>();
        wall = false;
    }
	

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Wall"))
        {
             wall = true;
			rb.useGravity = false;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
		wall = false;
		rb.useGravity = true;
    }

	public bool getWall ()
	{
		return wall;
	}
}
