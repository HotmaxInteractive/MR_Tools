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

    [HideInInspector]
    public int enumLength;

    private void Awake()
    {
        frame = GameObject.Find("Frame");
        offsetHandleGlow = GameObject.Find("OffsetHandleGlow");
        monitorGlow = GameObject.Find("Monitor Case Glow");
        onboardingUI = GameObject.Find("OnboardingUI");
    }

    void Start()
    {
        mrManager = GetComponent<MRManager>();


        //NOTE: **BUG** This sequence of AddListers doesn't work if [OnBeginInteraction.AddListener(monitorInteractionBegan);] goes after [OnEndInteraction.AddListener(handleInteractionEnd);] -do not know why.   -Andy 7/21/2017 
        GameObject.Find("Actor Monitor").GetComponent<NewtonVR.NVRInteractableItem>().OnBeginInteraction.AddListener(monitorInteractionBegan);
        GameObject.Find("Zed Offset").GetComponent<NewtonVR.NVRInteractableItem>().OnBeginInteraction.AddListener(handleInteractionBegan);
        GameObject.Find("Zed Offset").GetComponent<NewtonVR.NVRInteractableItem>().OnEndInteraction.AddListener(handleInteractionEnd);       
        GameObject.Find("Actor Monitor").GetComponent<NewtonVR.NVRInteractableItem>().OnEndInteraction.AddListener(monitorInteractionEnd);

        enumLength = System.Enum.GetValues(typeof(state)).Length;

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
        stage0,
        stage1,
        stage2,
        stage3,
        stage4,
        stage5
    }


    //mutator -- mutates state
    public void SET_STAGE_0()
    {
        state_ = state.stage0;
    }
    public void SET_STAGE_1()
    {
        state_ = state.stage1;
    }
    public void SET_STAGE_2()
    {
        state_ = state.stage2;
    }
    public void SET_STAGE_3()
    {
        state_ = state.stage3;
    }
    public void SET_STAGE_4()
    {
        state_ = state.stage4;
    }
    public void SET_STAGE_5()
    {
        state_ = state.stage5;
    }




    void Update()
    {
        //on start function add in all of the animations and ui that need to initially happen for stage 1

        switch (state_)
        {
            case state.stage0:
                changePanel(0);
                monitorGlow.GetComponent<Animator>().enabled = false;
                offsetHandleGlow.GetComponent<Animator>().enabled = true;
                break;
            case state.stage1:
                changePanel(1);
                offsetHandleGlow.GetComponent<Animator>().enabled = false;
                break;
            case state.stage2:              
                changePanel(2);
                monitorGlow.GetComponent<Animator>().enabled = true;
                break;
            case state.stage3:
                changePanel(3);
                monitorGlow.GetComponent<Animator>().enabled = false;
                break;
            case state.stage4:
                changePanel(4);
                break;
            case state.stage5:
                changePanel(5);
                break;

        }
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
