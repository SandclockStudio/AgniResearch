using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteRope : MonoBehaviour
{

    // Use this for initialization

    void Start()
    {
       
    }

    void DeleteRopes()
    {
   
        Destroy(gameObject, 1.0f);
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.rigidbody.tag == "Player")
        {
            DeleteRopes();
        }
    }
}
