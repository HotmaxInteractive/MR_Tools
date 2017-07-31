using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class uiButton : MonoBehaviour {

    NewtonVR.NVRButton nvrButton;
    OnboardingUI panelUI;
    public buttonStates btnStates_;
    posManager savePos;

    [HideInInspector]
    public bool raycastBtnHit = false;

    private void Start()
    {
        nvrButton = GetComponent<NewtonVR.NVRButton>();
        panelUI = GameObject.FindObjectOfType(typeof(OnboardingUI)) as OnboardingUI;
        savePos = GameObject.FindObjectOfType(typeof(posManager)) as posManager;
    }

    public enum buttonStates
    {
        foreward,
        backward,
        saveZedOffset,
        saveFramePosition

    }

    private void Update()
    {
        if (raycastBtnHit)
        {
            switch (btnStates_)
            {
                case buttonStates.foreward:
                    if ((int)panelUI.state_ != panelUI.enumLength -1)
                    {
                        panelUI.SendMessage("SET_STAGE_" + ((int)panelUI.state_ + 1).ToString());
                    }
                    else
                    {
                        return;
                    }
                    break;
                case buttonStates.backward:
                    if ((int)panelUI.state_ != 0)
                    {
                        panelUI.SendMessage("SET_STAGE_" + ((int)panelUI.state_ - 1).ToString());
                    }
                    else {
                        return;
                    }
                    break;
                case buttonStates.saveZedOffset:
                    savePos.SavePos();
                    break;
                case buttonStates.saveFramePosition:
                    print("Saving Frame Position...");
                    break;

                    }
            }
        raycastBtnHit = false;
    }
}
