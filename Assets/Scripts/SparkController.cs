using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SparkController : MonoBehaviour {
	[SerializeField] private float m_Lifetime = 3f;

	void Start () {
		Destroy(gameObject, m_Lifetime);
	}

}