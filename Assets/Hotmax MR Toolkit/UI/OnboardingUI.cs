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
    GameObject frame;
    GameObject onboardingUI;

    public GameObject actorMonitor;

    public state state_;

    [HideInInspector]
    public int enumLength;

    private void Awake()
    {
        frame = GameObject.Find("Frame");
        monitorGlow = GameObject.Find("Monitor Case Glow");
        onboardingUI = GameObject.Find("OnboardingUI");
    }

    void Start()
    {
        mrManager = GetComponent<MRManager>();
        enumLength = System.Enum.GetValues(typeof(state)).Length;

    }

    public enum state
    {
        stage0,
        stage1,
        stage2,
        stage3
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





    void Update()
    {
        //on start function add in all of the animations and ui that need to initially happen for stage 1

        switch (state_)
        {
            case state.stage0:
                changePanel(0);
                monitorGlow.GetComponent<Animator>().enabled = false;
                break;
            case state.stage1:
                changePanel(1);
                break;
            case state.stage2:              
                changePanel(2);
                monitorGlow.GetComponent<Animator>().enabled = true;
                break;
            case state.stage3:
                changePanel(3);
                monitorGlow.GetComponent<Animator>().enabled = false;
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
