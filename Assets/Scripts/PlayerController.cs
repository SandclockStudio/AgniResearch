using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    private Rigidbody rb;
    public float speed;
    private bool wall;

    // Use this for initialization
    void Start ()
    {
        rb = GetComponent<Rigidbody>();
        wall = false;
    }
	
	// Update is called once per frame

    void Update()
    {
        if (Input.GetKey(KeyCode.D))
        {
            rb.AddForce(new Vector3(1,0,0)*speed);
        }

        if (Input.GetKey(KeyCode.A))
        {
            rb.AddForce(new Vector3(-1, 0, 0) * speed);
        }

        if (Input.GetKey(KeyCode.W) && wall)
        {
            rb.AddForce(new Vector3(0, 1, 0) * speed);
        }

        if (Input.GetKey(KeyCode.S) && wall)
        {
            rb.AddForce(new Vector3(0, -1, 0) * speed);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("wall"))
        {
            wall = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("wall"))
        {
            wall = false;
        }
    }
}
