using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SparkController : MonoBehaviour {

    [SerializeField] private float m_Lifetime = 3f;

    void Start () {
        Destroy(gameObject, m_Lifetime);
    }

    void OnCollisionEnter (Collision collision) {
        if (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Rope")) {
            GameObject.FindWithTag("Player").transform.position = collision.contacts[0].point;

            SparkController[] otherSparks = GameObject.FindObjectsOfType<SparkController>();
            foreach (SparkController s in otherSparks) {
                s.enabled = false;
            }
        }
    }

}