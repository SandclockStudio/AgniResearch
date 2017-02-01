using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MovementBehaviour : MonoBehaviour {

	private Rigidbody m_Rigidbody;

	private Vector3 m_Direction;

	public bool UseGravity {
		get {
			return m_Rigidbody.useGravity;
		}
		set {
			m_Rigidbody.useGravity = value;
		}
	}

	[SerializeField] private float m_Speed = 10;

	[SerializeField] private bool m_Randomize = false;

	// Use this for initialization
	void Start () {
		m_Rigidbody = GetComponentInChildren<Rigidbody>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		
		m_Rigidbody.velocity = m_Direction * m_Speed;

		if (m_Randomize) {
			m_Rigidbody.velocity += (new Vector3(Random.value / 10f, Random.value / 10f, 0f));
		} 
	}

	public void SetDirection (Vector3 direction) {
		m_Direction = direction;
	}

	public void SetRandomize (bool randomize) {
		m_Randomize = randomize;
	}
}
