using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MovementBehaviour : MonoBehaviour {

	private Rigidbody m_Rigidbody;

	private Vector3 m_Direction;

	public bool isJumping;

	public bool beingInputed;

	public bool UseGravity 
	{
		get 
		{
			return m_Rigidbody.useGravity;
		}
		set 
		{
			m_Rigidbody.useGravity = value;
		}
	}

	[SerializeField] private float m_Speed;
	[SerializeField] private float m_JumpForce = 100;


	[SerializeField] private bool m_Randomize = false;

	// Use this for initialization
	void Start () 
	{
		m_Rigidbody = GetComponentInChildren<Rigidbody>();
	}
	
	// Update is called once per frame


	public void SetDirection (Vector3 direction, float speedModifier) 
	{
		
		transform.Translate(direction * m_Speed * speedModifier);
		beingInputed = true;

	}

	public void Jump () 
	{
		if (!isJumping)
		{
			isJumping = true;
			m_Rigidbody.AddForce(Vector3.up * m_JumpForce);
		}
		beingInputed = true;
	}

	public void SetRandomize (bool randomize) {
		m_Randomize = randomize;
	}

	public void Shake (bool shakeSide, int angle)
	{

		Vector3 vector = transform.localEulerAngles;
		if (shakeSide)
			transform.localEulerAngles = new Vector3(vector.x,vector.y+angle,vector.z+0);
		
		else

			transform.localEulerAngles = new Vector3(-90*Time.deltaTime,0,0);
	}

	public void RotateY (float angle)
	{
		transform.eulerAngles = new Vector3(transform.eulerAngles.x,angle,transform.eulerAngles.z);
	}

	public void RotateX (float angle)
	{
		transform.eulerAngles = new Vector3(angle,transform.eulerAngles.y,transform.eulerAngles.z);
	}

	public void RotateZ (float angle)
	{
		transform.eulerAngles = new Vector3(transform.eulerAngles.x,transform.eulerAngles.y,angle);
	}
}
