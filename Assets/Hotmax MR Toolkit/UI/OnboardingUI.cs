using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public class OnboardingUI : MonoBehaviour
{

    MRManager mrManager;
    RadialMenuController radMenuController;
    GameObject monitorGlow;
    GameObject offsetHandleGlow;
    GameObject frame;
    GameObject onboardingUI;

    public GameObject zedOffsetHandle;
    public GameObject actorMonitor;

    bool handleGrab = false;
    bool monitorGrab = false;

    public state state_;

    private void Awake()
    {
        frame = GameObject.Find("Frame");
        offsetHandleGlow = GameObject.Find("OffsetHandleGlow");
        monitorGlow = GameObject.Find("Monitor Case Glow");
        onboardingUI = GameObject.Find("OnboardingUI");

        state_ = state.stage1;

        monitorGlow.GetComponent<Animator>().enabled = false;
        offsetHandleGlow.GetComponent<Animator>().enabled = true;
        changePanel(0);
    }

    void Start()
    {
        mrManager = GetComponent<MRManager>();

        //NOTE: **BUG** This sequence of AddListers doesn't work if [OnBeginInteraction.AddListener(monitorInteractionBegan);] goes after [OnEndInteraction.AddListener(handleInteractionEnd);] -do not know why.   -Andy 7/21/2017 
        GameObject.Find("Actor Monitor").GetComponent<NewtonVR.NVRInteractableItem>().OnBeginInteraction.AddListener(monitorInteractionBegan);
        GameObject.Find("Zed Offset").GetComponent<NewtonVR.NVRInteractableItem>().OnBeginInteraction.AddListener(handleInteractionBegan);
        GameObject.Find("Zed Offset").GetComponent<NewtonVR.NVRInteractableItem>().OnEndInteraction.AddListener(handleInteractionEnd);       
        GameObject.Find("Actor Monitor").GetComponent<NewtonVR.NVRInteractableItem>().OnEndInteraction.AddListener(monitorInteractionEnd);
    }

    void handleInteractionBegan()
    {
        handleGrab = true;
        print("Handle Grab " + handleGrab);
    }

    void handleInteractionEnd()
    {
        handleGrab = false;
        print("Handle Grab " + handleGrab);
    }

    void monitorInteractionBegan()
    {
        monitorGrab = true;
        print("Monitor Grab " + monitorGrab);
    }

    void monitorInteractionEnd()
    {
        monitorGrab = false;
        print("Monitor Grab " + monitorGrab);
    }

    public enum state
    {
        stage1,
        stage2,
        stage3,
        stage4,
        stage5
    }



    void Update()
    {
        //on start function add in all of the animations and ui that need to initially happen for stage 1



        switch (state_)
        {
            case state.stage1:
                if (handleGrab)
                {
                    state_ = state.stage2;
                    changePanel(1);
                    offsetHandleGlow.GetComponent<Animator>().enabled = false;
                }
                break;
            case state.stage2:
                if (mrManager.constraintSwitched == true)
                {
                    state_ = state.stage3;
                    changePanel(2);
                    monitorGlow.GetComponent<Animator>().enabled = true;
                    mrManager.constraintSwitched = false;
                }
                break;
            case state.stage3:
                if (monitorGrab)
                {
                    state_ = state.stage4;
                    changePanel(3);
                    monitorGlow.GetComponent<Animator>().enabled = false;
                }
                break;
            case state.stage4:
                if (handleGrab)
                {
                    state_ = state.stage5;
                    changePanel(4);
                }
                break;
            case state.stage5:
                if (frame.GetComponent<SaveFramePos>().offsetsSaved)
                {
                    changePanel(5);
                }
                break;

        }

        /*

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
        }
        
        //Grabs the offset handle again while looking at the monitor
        if(activePanel == 3 && handleGrab)
        {
            changePanel();
        }

        //Hits trigger to save the offset
        if (activePanel == 4 && frame.GetComponent<SaveFramePos>().offsetsSaved == true)
        {
            changePanel();
            frame.GetComponent<SaveFramePos>().offsetsSaved = false;
        }

        */
    }

    public void changePanel(int activePanel)
    {
        // Set all panels to false.
        for (int i = 0; i < onboardingUI.transform.childCount; i++)
        {
            onboardingUI.transform.GetChild(i).gameObject.SetActive(false);
        }

        onboardingUI.transform.GetChild(activePanel).gameObject.SetActive(true);

    }
}
