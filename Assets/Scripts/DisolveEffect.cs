using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisolveEffect : MonoBehaviour {


    private float value = 1.0f;
    private bool isRunning = false;
    private Material dissolveMaterial = null;
    private float timeScale = 0.5f;

	// Use this for initialization
	void Start ()
    {
        float maxVal = 0.0f;
        //Cogemos el material
        dissolveMaterial = GetComponent<Renderer>().material;
        //Cogemos vertices de la malla
        var verts = GetComponent<MeshFilter>().mesh.vertices;
        //Para cada vertice
        for (int i = 0; i < verts.Length; i++)
        {
            //obtenemos el valor para escalar la distancia
            var v1 = verts[i];
            for (int j = 0; j < verts.Length; j++)
            {
                if (j == i) continue;
                var v2 = verts[j];
                float mag = (v1 - v2).magnitude;
                if (mag > maxVal) maxVal = mag;

            }
        }

        dissolveMaterial.SetFloat("_LargestVal", maxVal * 0.5f);
    }
    public void Reset()
    {
        value = 1.0f;
        dissolveMaterial.SetFloat("_DissolveValue", value);
    }

    public void TriggerDissolve(Vector3 hitPoint)
    {
        value = 1.0f;
        dissolveMaterial.SetVector("_HitPos", (new Vector4(hitPoint.x, hitPoint.y, hitPoint.z, 1.0f)));
        isRunning = true;
    }

    // Update is called once per frame
    void Update ()
    {
        if (isRunning && dissolveMaterial != null)
        {
            value = Mathf.Max(0.0f, value - Time.deltaTime * timeScale);
            dissolveMaterial.SetFloat("_DissolveValue", value);
        }
    }
}
