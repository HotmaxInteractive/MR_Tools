using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Calibration_Store : MonoBehaviour {

    //Calibration Mode is On
    public static bool calibrationModeIsOn = false;
    public delegate void calibrationIsOnHandler(bool value);
    public static event calibrationIsOnHandler calibrationModeIsOnEvent;

    //Calibration Stage
    public static stages calibrationStage = stages.stage0;
    public enum stages
    {
        stage0,
        stage1,
        stage2,
        stage3
    }
    public delegate void calibrationStageHandler(stages value);
    public static event calibrationStageHandler calibrationStageEvent;

    //save and load events (no store variable since they are just notifications with no argument)
    public delegate void saveZedOffsetHandler();
    public static event saveZedOffsetHandler saveZedOffsetEvent;
    public delegate void loadZedOffsetHandler();
    public static event loadZedOffsetHandler loadZedOffsetEvent;

    public delegate void saveFrameOffsetHandler();
    public static event saveFrameOffsetHandler saveFrameOffsetEvent;
    public delegate void loadFrameOffsetHandler();
    public static event loadFrameOffsetHandler loadFrameOffsetEvent;


    void Start () {}
	void Update () {}


    // -- mutators
    //
    public void SET_CALIBRATION_IS_ON(bool value)
    {
        calibrationModeIsOn = value;
        calibrationModeIsOnEvent(value);
    }

    public void SET_STAGE_0()
    {
        calibrationStage = stages.stage0;
        calibrationStageEvent(stages.stage0);
    }
    public void SET_STAGE_1()
    {
        calibrationStage = stages.stage1;
        calibrationStageEvent(stages.stage1);
    }
    public void SET_STAGE_2()
    {
        calibrationStage = stages.stage2;
        calibrationStageEvent(stages.stage2);
    }
    public void SET_STAGE_3()
    {
        calibrationStage = stages.stage3;
        calibrationStageEvent(stages.stage3);
    }

    public void SAVE_ZED_OFFSET()
    {
        saveZedOffsetEvent();
    }
    public void LOAD_ZED_OFFSET()
    {
        loadZedOffsetEvent();
    }

    public void SAVE_FRAME_OFFSET()
    {
        saveFrameOffsetEvent();
    }
    public void LOAD_FRAME_OFFSET()
    {
        loadFrameOffsetEvent();
    }

}
