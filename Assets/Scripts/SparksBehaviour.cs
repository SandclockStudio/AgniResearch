using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SparksBehaviour : MonoBehaviour {

	private Transform m_Origin;
	private Transform m_Aim;

	[SerializeField] private GameObject m_SparkPrefab;
	[SerializeField] private int m_SparkCount = 10; 
	[SerializeField] private float m_Step = 1; 
	[SerializeField] private float m_Limit = 0.25f; 
	[SerializeField] private int m_Spread = 45; 

	// Use this for initialization
	void Start () {
		m_Origin = transform.FindChild("SparksOrigin");
		m_Aim = transform.FindChild("SparksAim");

		if (m_SparkPrefab == null) {
			Debug.LogError("SparksBehaviour must have a Spark Prefab assigned");	
		}
	}

	public void Aim (float direction) {

		m_Aim.position += Vector3.up * direction * m_Step;

		float clamped = Mathf.Clamp(m_Aim.position.y, m_Origin.position.y - m_Limit, m_Origin.position.y + m_Limit);

		m_Aim.position = new Vector3(m_Aim.position.x, clamped, m_Aim.position.z);
	}

	public void Throw () {
		StartCoroutine(ThrowSparks());
	}

	private IEnumerator ThrowSparks () {

		List<GameObject> sparks = new List<GameObject>();

		while (sparks.Count < m_SparkCount) {
			
			GameObject spark = Instantiate(m_SparkPrefab, m_Origin.position, Quaternion.identity) as GameObject;
			spark.SetActive(false);
			sparks.Add(spark);
		}

		int i = m_SparkCount / 2 * -1;

		while (sparks.Count > 0) {

			int index = Random.Range(0, sparks.Count);

			GameObject spark = sparks[index];

			Vector3 direction = Vector3.Normalize(m_Aim.position - m_Origin.position);

			float x = direction.x + Mathf.Cos(Mathf.Deg2Rad * m_Spread * i / m_SparkCount);
			float y = direction.y + Mathf.Sin(Mathf.Deg2Rad * m_Spread * i / m_SparkCount);

			x += Random.value / 5;
			y += Random.value / 5;

			MovementBehaviour mb = spark.GetComponent<MovementBehaviour>();
			mb.SetRandomize(true);
			mb.SetDirection(new Vector3(x, y, 0),1);

			spark.SetActive(true);
			sparks.RemoveAt(index);

			yield return new WaitForSeconds(0.005f);
		}
	}
}
