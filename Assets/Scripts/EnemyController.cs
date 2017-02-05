using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EnemyController : MonoBehaviour {

	public Vector3 startPoint;
	public Vector3 endPoint;

	public bool m_Direction;
	private MovementBehaviour m_Movement;
	private Vector3 initialPosition;
	private float distanceToPoint = 0.05f;

	// Use this for initialization
	void Start () 
	{
		m_Movement = GetComponent<MovementBehaviour>();
		initialPosition = transform.position;
		m_Direction = false;
	

		InvokeRepeating("Shake", 0.35f, 0.35f);
	}

	void Shake() {
		m_Direction = !m_Direction;
		if (!m_Direction)
		{
			m_Movement.RotateX(25);
		}
		else
		{
			m_Movement.RotateX(-25);
		}
	}

	// Update is called once per frame
	void Update () 
	{
		
		if(Vector3.Distance(transform.position, initialPosition + startPoint) < distanceToPoint)
		{
			m_Movement.RotateY(180);
		}

		if(Vector3.Distance(transform.position, initialPosition + endPoint) < distanceToPoint)
		{
			m_Movement.RotateY(0);
		}


		m_Movement.SetDirection(Vector3.left,1);
	
	}

	void OnDrawGizmosSelected () {
		Gizmos.color = Color.red;
		Gizmos.DrawCube(transform.position + startPoint, Vector3.one / 100);
		Gizmos.DrawCube(transform.position + endPoint, Vector3.one / 100);
	}
		
}
