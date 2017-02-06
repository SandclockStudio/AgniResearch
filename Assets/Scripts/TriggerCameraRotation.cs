using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerCameraRotation : MonoBehaviour {

    public float m_RotationTime;
    public float m_Rotation;
    private float m_CurrentRotation = 0;
    private float step;

    void OnTriggerEnter (Collider collider) {
    	if (collider.CompareTag("Player")) {
            StartCoroutine(RotateCoroutine(collider));
        }
    }

    IEnumerator RotateCoroutine (Collider collider) {
        while (m_CurrentRotation < Mathf.Abs(m_Rotation)) {
            step = m_Rotation / m_RotationTime * Time.fixedDeltaTime;

            Camera.main.transform.Rotate(0, step, 0);

            m_CurrentRotation += Mathf.Abs(step);
            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }

        collider.transform.Rotate(0, m_Rotation, 0);
        yield return null;
    }
}
