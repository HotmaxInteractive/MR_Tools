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

        monitorGlow.GetComponent<Animator>().enabled = false;
        offsetHandleGlow.GetComponent<Animator>().enabled = true;

        GameObject.Find("Zed Offset").GetComponent<NewtonVR.NVRInteractableItem>().OnBeginInteraction.AddListener(handleInteractionBegan);
        GameObject.Find("Zed Offset").GetComponent<NewtonVR.NVRInteractableItem>().OnEndInteraction.AddListener(handleInteractionEnd);
        GameObject.Find("Actor Monitor").GetComponent<NewtonVR.NVRInteractableItem>().OnBeginInteraction.AddListener(monitorInteractionBegan);
        GameObject.Find("Actor Monitor").GetComponent<NewtonVR.NVRInteractableItem>().OnEndInteraction.AddListener(monitorInteractionEnd);

    }

    //[SerializeField]
    public void handleInteractionBegan() {
        handleGrab = true;
    }

    //[SerializeField]
    public void handleInteractionEnd()
    { 
        handleGrab = false;
    }

    //[SerializeField]
    public void monitorInteractionBegan()
    {
        monitorGrab = true;
    }

    //[SerializeField]
    public void monitorInteractionEnd()
    {
        monitorGrab = false;
    }

    void Update()
    {
        // Grabs the Offset Handle
        if (activePanel == 0 && handleGrab)
        {
            offsetHandleGlow.GetComponent<Animator>().enabled = false;
            changePanel();
        }

        // Uses the pad controls
        if (activePanel == 1 && mrManager.constraintSwitched == true)
        {
            changePanel();
            mrManager.constraintSwitched = false;
            monitorGlow.GetComponent<Animator>().enabled = true;
        }

        //grabs the actor monitor
        if (activePanel == 2 && monitorGrab)
        {
            changePanel();
            monitorGlow.GetComponent<Animator>().enabled = false;

            print("2");
        }
        
        //Grabs the offset handle again while looking at the monitor
        if(activePanel == 3 && handleGrab)
        {
            changePanel();
            print("3");
        }

        //Hits trigger to save the offset
        if (activePanel == 4 && frame.GetComponent<SaveFramePos>().offsetsSaved == true)
        {
            changePanel();
            frame.GetComponent<SaveFramePos>().offsetsSaved = false;
        }


    }

    public void changePanel()
    {

            // Getting the 
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
