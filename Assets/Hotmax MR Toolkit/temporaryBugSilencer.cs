using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class temporaryBugSilencer : MonoBehaviour {

    void Update() {
        if(GameObject.Find("Controller UI Camera") != null) { 
            GameObject cameraUI = GameObject.Find("Controller UI Camera");
            cameraUI.GetComponent<Camera>().allowHDR = false;
           
            if (cameraUI.GetComponent<Camera>().allowHDR == false)
            {
                cameraUI.GetComponent<Camera>().allowHDR = true;
                Destroy(gameObject);
            }
        }
    }
}
