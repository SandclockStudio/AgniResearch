using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MovementBehaviour))]
[RequireComponent(typeof(SparksBehaviour))]
public class PlayerController : MonoBehaviour {

	private MovementBehaviour m_Movement;
    private SparksBehaviour m_Sparks;

    private bool CanMoveVertical {
    	get {
    		return wall || rope;
    	}
    }

    private bool wall = false, rope = false;

    // Use this for initialization
    void Start () {
    	m_Movement = GetComponent<MovementBehaviour>();
    	m_Sparks = GetComponent<SparksBehaviour>();
    }
	
	// Update is called once per frame
    void Update () {
    	float x = Input.GetAxis("Horizontal");
		float y = Input.GetAxis("Vertical");

		float aim = Input.GetAxis("Aim");

		Vector3 direction = Vector3.zero;

	    if (Mathf.Abs(aim) > 0) {

        	m_Sparks.Aim(aim);
        }
		if (Input.GetButtonDown("Sparks")) {

            m_Sparks.Throw();
        }

		if (Mathf.Abs(x) > 0) {
			direction += transform.TransformDirection(Vector3.forward) * x;
        }

		if (Mathf.Abs(y) > 0 && CanMoveVertical) {
			direction += transform.TransformDirection(Vector3.up) * y;
        }

        m_Movement.SetDirection(direction);
    }

    private void OnCollisionEnter (Collision collision) {
        if (collision.gameObject.CompareTag("Wall")) {
             wall = true;
        }
    }

    private void OnCollisionStay (Collision collision) {
        if (collision.gameObject.CompareTag("Rope")) {
            Vector3 direction = collision.transform.position - transform.position;
            transform.position += new Vector3(direction.x, 0, direction.z - transform.localScale.z/2);

            rope = true;
        }

        if (CanMoveVertical) {
        	m_Movement.UseGravity = false;
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

		if (!CanMoveVertical) {
        	m_Movement.UseGravity = true;
        }
    }

}
