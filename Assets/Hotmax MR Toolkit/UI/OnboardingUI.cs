using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnboardingUI : MonoBehaviour {

    MRManager mrManager;
    RadialMenuController radMenuController;
    int activePanel = 0;
    GameObject monitorGlow;
    GameObject offsetHandleGlow;
    GameObject frame;

    bool handleGrab = false;
    bool monitorGrab = false;

    private void Awake()
    {
        frame = GameObject.Find("Frame");
        offsetHandleGlow = GameObject.Find("OffsetHandleGlow");
        monitorGlow = GameObject.Find("Monitor Case Glow");
    }

    void Start () {
        mrManager = GetComponent<MRManager>();
        radMenuController = mrManager.calibrationController.GetComponent<RadialMenuController>();
	}

    public void handleInteractionBegan() {
        offsetHandleGlow.GetComponent<Animator>().enabled = false;
        handleGrab = true;
    }

    public void handleInteractionEnd()
    {
        offsetHandleGlow.GetComponent<Animator>().enabled = true;
        handleGrab = false;
    }

    public void monitorInteractionBegan()
    {
        monitorGlow.GetComponent<Animator>().enabled = false;
        monitorGrab = true;
    }

    public void monitorInteractionEnd()
    {
        monitorGlow.GetComponent<Animator>().enabled = true;
        monitorGrab = false;
    }

    void Update()
    {
        if (activePanel == 0 && handleGrab)
        {
            changePanel();
        }

        if (activePanel == 1 && mrManager.constraintSwitched == true)
        {
            changePanel();
            mrManager.constraintSwitched = false;
            monitorGlow.GetComponent<Animator>().enabled = true;
        }

        if (activePanel == 2 && monitorGrab)
        {
            changePanel();
        }
        
        if(activePanel == 3 && handleGrab)
        {
            changePanel();
        }

        if (activePanel == 4 && frame.GetComponent<SaveFramePos>().offsetsSaved == true)
        {
            changePanel();
            frame.GetComponent<SaveFramePos>().offsetsSaved = false;
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
