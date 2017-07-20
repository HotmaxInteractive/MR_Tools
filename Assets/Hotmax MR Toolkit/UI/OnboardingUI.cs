using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnboardingUI : MonoBehaviour {

    MRManager mrManager;
    int activePanel = 0;

    void Start () {
        mrManager = GetComponent<MRManager>();
	}

    public void interactionBegan() {
        mrManager.cameraHandle.GetComponent<Animator>().enabled = false;
    }

    public void interactionEnd()
    {
        mrManager.cameraHandle.GetComponent<Animator>().enabled = true;
    }

    void Update()
    {
        if (activePanel == 0 && mrManager.cameraHandle.GetComponent<Animator>().enabled == false)
        {
            changePanel();
            print("activePanel: " + activePanel);
        }
    }

    public void changePanel()
    {

            // Iterate through the panels in order once an event
            if (activePanel == mrManager.offsetTools.transform.GetChild(0).transform.childCount - 1)
            {
                activePanel = 0;
            }
            else
            {
                activePanel += 1;
            }

            // Set all panels to false.
            for (int i = 0; i < mrManager.offsetTools.transform.GetChild(0).transform.childCount; i++)
            {
                mrManager.offsetTools.transform.GetChild(0).transform.GetChild(i).gameObject.SetActive(false);
            }
            mrManager.offsetTools.transform.GetChild(0).transform.GetChild(activePanel).gameObject.SetActive(true);

    }
}
