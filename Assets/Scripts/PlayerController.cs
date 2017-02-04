using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MovementBehaviour))]
//[RequireComponent(typeof(PlayerMovementBehaviour))]
//[RequireComponent(typeof(SparksBehaviour))]

public class PlayerController : MonoBehaviour
{

	private Transform m_Fire;

    private Quaternion originalRotation;
	private float m_FuelAmount = 1;
	[SerializeField] private float m_MinJumpDistance = 0.3f;

	private RaycastHit hit;
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
        originalRotation = transform.rotation;
        m_Movement = GetComponent<MovementBehaviour>();
    	m_Sparks = GetComponent<SparksBehaviour>();
    	m_Fire = transform.FindChild("Fire");
    }
	
    void Update ()
    {
		float aim = Input.GetAxis("Aim");

		Vector3 direction = Vector3.zero;

	    if (Mathf.Abs(aim) > 0)
	    {
        	//m_Sparks.Aim(aim);
        }
		if (Input.GetButtonDown("Sparks"))
		{

           // m_Sparks.Throw();
        }

        FuelAmount -= 0.0005f;

		// m_Movement.UseGravity = true;

	


		if (Physics.Raycast(transform.position, Vector3.down, out hit, m_MinJumpDistance )) 
		{
			m_Movement.isJumping = false;
		}

		if(!m_Movement.beingInputed && wall)
		{
			m_Movement.SetDirection(Vector3.up,0.2f);
		}

    }
		

	private void OnCollisionStay (Collision collision)
	{
		if (collision.gameObject.CompareTag("Rope"))
		{
			
			Vector3 newPos;
            transform.rotation = originalRotation * Quaternion.AngleAxis(collision.rigidbody.rotation.eulerAngles.z, Vector3.forward);
            Vector3 distance = new Vector3((collision.rigidbody.transform.position.x - transform.localPosition.x), (collision.rigidbody.transform.position.y - transform.localPosition.y), (collision.rigidbody.transform.position.z - transform.localPosition.z));
            newPos = new Vector3((distance.x) - (transform.localScale.x / 2), 0, (distance.z) - (transform.localScale.z / 2));
            transform.position += transform.up*0.02f + newPos;
			rope = true;
            m_Movement.UseGravity = false;
        }
		else
		{
			transform.rotation = originalRotation;
			m_Movement.UseGravity = !Grounded;
		}
    }
		
    private void OnCollisinExit (Collision collision)
    {
        if (collision.gameObject.CompareTag("Rope"))
        {
            transform.rotation = originalRotation;
            rope = false;
            m_Movement.UseGravity = true;
        }
		else
		{
    	    transform.rotation = originalRotation;
			m_Movement.UseGravity = !Grounded;
		}
    }

}
