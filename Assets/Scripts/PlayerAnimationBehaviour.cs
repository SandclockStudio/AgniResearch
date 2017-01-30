using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationBehaviour : MonoBehaviour {

    [Range(1, 12)] public int framesToSkip = 2;

    MeshRenderer[] m_Renderers;
    Material[] m_Mat;

    // Use this for initialization
    void Start () {
        m_Renderers = GetComponentsInChildren<MeshRenderer>();
        m_Mat = new Material[m_Renderers.Length];

        int i = 0;
        foreach (MeshRenderer r in m_Renderers) {
            m_Mat[i++] = r.material;
        }
	}
	
	// Update is called once per frame
	void Update () {
        Animate(framesToSkip);
	}

    void Animate (float skip) {
        if (Time.frameCount % skip == 0) {

            foreach (Material mat in m_Mat) {
                mat.mainTextureOffset = new Vector2(mat.mainTextureOffset.x + 0.1f, mat.mainTextureOffset.y);

                if (mat.mainTextureOffset.x > 1) {
                    mat.mainTextureOffset = new Vector2(0f, mat.mainTextureOffset.y);
                }
            }

        }
    }
}
