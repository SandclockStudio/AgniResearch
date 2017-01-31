using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderAnimationBehaviour : MonoBehaviour {

    [Range(1, 12)] public int framesToSkip = 2;

    private MeshRenderer[] m_Renderers;
    private Material[] m_Mat;

    private int direction = 1;

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
                if (mat.mainTextureOffset.x == 1 || mat.mainTextureOffset.x == 0) {
                    direction *= -1;
                }

                mat.mainTextureOffset = new Vector2(mat.mainTextureOffset.x + 0.1f * direction, mat.mainTextureOffset.y);
            }

        }
    }
}
