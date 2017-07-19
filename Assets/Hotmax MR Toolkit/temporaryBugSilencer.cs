using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class temporaryBugSilencer : MonoBehaviour {

    void Update() {
        GameObject cameraUI = GameObject.Find("Controller UI Camera");
        cameraUI.GetComponent<Camera>().allowHDR = false;
        if (cameraUI.GetComponent<Camera>().allowHDR == false) {
            Destroy(gameObject);
        }
    }
}
