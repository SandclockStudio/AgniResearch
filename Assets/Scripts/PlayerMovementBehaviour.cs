using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementBehaviour : MonoBehaviour {

	[SerializeField] private float m_Speed = 10;

	private Vector3 m_Direction;

	void FixedUpdate () {
		Vector3 velocity = m_Direction * m_Speed / 100;
		transform.Translate(velocity);
	}

	public void SetDirection (Vector3 direction) {
		m_Direction = direction;
	}
}
