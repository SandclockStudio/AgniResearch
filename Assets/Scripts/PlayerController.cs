using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MovementBehaviour))]
//[RequireComponent(typeof(PlayerMovementBehaviour))]
//[RequireComponent(typeof(SparksBehaviour))]

public class PlayerController : MonoBehaviour
{

	private Transform m_Fire;

	private float m_FuelAmount = 1;

	public float FuelAmount {
		get {
			return m_FuelAmount;
		}
		set {
			m_FuelAmount = value;

			//m_Fire.localScale = Vector3.one * m_FuelAmount;
			//m_Fire.localPosition = new Vector3(0, m_FuelAmount / 5 * 4, 0);
		}
	}

	private MovementBehaviour m_Movement;
	//private PlayerMovementBehaviour m_Movement;
    private SparksBehaviour m_Sparks;

    public bool Grounded
    {
    	get
    	{
    		return wall || rope;
    	}
    }

    public bool wall = false, rope = false;

    void Start ()
    {
		m_Movement = GetComponent<MovementBehaviour>();
		//m_Movement = GetComponent<PlayerMovementBehaviour>();
    	m_Sparks = GetComponent<SparksBehaviour>();
    	m_Fire = transform.FindChild("Fire");
    }
	
    void Update ()
    {
		/*
    	float x = Input.GetAxis("Horizontal");
		float y = Input.GetAxis("Vertical");
		*/
		float aim = Input.GetAxis("Aim");

		Vector3 direction = Vector3.zero;

	    if (Mathf.Abs(aim) > 0)
	    {
        	m_Sparks.Aim(aim);
        }
		if (Input.GetButtonDown("Sparks"))
		{

            m_Sparks.Throw();
        }
			
		/*
		if (Mathf.Abs(x) > 0)
		{
			direction += transform.TransformDirection(Vector3.forward * x);
			//direction += Vector3.forward * x;
        }

		if (Mathf.Abs(y) > 0 && Grounded)
		{
			direction += transform.TransformDirection(Vector3.up * y);
			//direction += Vector3.up * y;
        }
        */

        FuelAmount -= 0.0005f;
		// m_Movement.UseGravity = true;

       // m_Movement.SetDirection(direction);
    }
		

	private void OnCollisionStay (Collision collision)
	{
		if (collision.gameObject.CompareTag("Rope"))
		{
			Vector3 distance = new Vector3((collision.rigidbody.transform.position.x - transform.position.x), (collision.rigidbody.transform.position.y - transform.position.y), (collision.rigidbody.transform.position.z - transform.position.z));
			Vector3 newPos;
			Vector3 deg = transform.rotation.eulerAngles;


			newPos = new Vector3(distance.x- transform.localScale.x / 2, 0.01f, distance.z - transform.localScale.z/2);
			transform.position += newPos;
			rope = true;

		}

		m_Movement.UseGravity = !Grounded;
	}
		
    private void OnCollisionExit (Collision collision)
    {
        if (collision.gameObject.CompareTag("Rope"))
        {
            rope = false;
        }

		m_Movement.UseGravity = !Grounded;
    }

}
