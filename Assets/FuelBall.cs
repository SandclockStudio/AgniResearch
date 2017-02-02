using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuelBall : MonoBehaviour {

	[SerializeField] private float m_FuelAmount;

	void OnTriggerEnter (Collider collider) {
		if (collider.CompareTag("Player")) {
			GameObject.FindObjectOfType<PlayerController>().FuelAmount += m_FuelAmount;
		}
	}
}
