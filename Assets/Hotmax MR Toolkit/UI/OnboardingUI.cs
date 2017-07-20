using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnboardingUI : MonoBehaviour {

    MRManager mrManager;

	void Start () {
        mrManager = GetComponent<MRManager>();
	}

    public void stopHandleGlow() {
        mrManager.cameraHandle.GetComponent<Animator>().enabled = false;
    }

    public void startHandleGlow()
    {
        mrManager.cameraHandle.GetComponent<Animator>().enabled = true;
    }
}
