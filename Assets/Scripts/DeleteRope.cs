using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteRope : MonoBehaviour
{

    // Use this for initialization
    GameObject player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void DeleteRopes()
    {
   
        Destroy(gameObject, 0.079f);
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.rigidbody.tag == "Player")
        {
            DeleteRopes();
        }
    }
}
