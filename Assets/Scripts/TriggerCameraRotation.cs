﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerCameraRotation : MonoBehaviour {

    void OnTriggerEnter(Collider collider) {
        Camera.main.transform.Rotate(0, -90, 0);
        collider.transform.Rotate(0, -90, 0);
        Destroy(gameObject);
    }
}
