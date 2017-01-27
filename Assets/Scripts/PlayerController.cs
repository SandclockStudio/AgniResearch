using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    private Rigidbody rb;
    public float speed;
    private bool wall, rope;

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

        if (Input.GetKey(KeyCode.W) && (wall || rope))
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
        if(collision.gameObject.CompareTag("Wall"))
        {
             wall = true;
        }
      
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Rope"))
        {
            Vector3 distance = new Vector3((collision.rigidbody.transform.position.x - transform.position.x), (collision.rigidbody.transform.position.y - transform.position.y), (collision.rigidbody.transform.position.z - transform.position.z));
            Vector3 newPos = new Vector3(transform.position.x+distance.x, transform.position.y, transform.position.z + distance.z - transform.localScale.z/2);
            transform.position = newPos;
            rope = true;
           
        }
    }

    private void OnCollisionExit(Collision collision)
    {
		if (collision.gameObject.CompareTag("Wall"))
        {
            wall = false;
        }
        if (collision.gameObject.CompareTag("Rope"))
        {
            rope = false;
        }
    }

}
