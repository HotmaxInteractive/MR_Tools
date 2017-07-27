using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Calibration_Manager : MonoBehaviour {

    //state enum
    public stages stages_;

    //place buttons here for save methods that can be called


    void Start () {
		
	}
	
	void Update () {
		
	}


    // -- state
    //
    public enum stages
    {
        stage0,
        stage1,
        stage2,
        stage3
    }

    // -- mutators
    //
    public void SET_STAGE_0()
    {
        stages_ = stages.stage0;
    }
    public void SET_STAGE_1()
    {
        stages_ = stages.stage1;
    }
    public void SET_STAGE_2()
    {
        stages_ = stages.stage2;
    }
    public void SET_STAGE_3()
    {
        stages_ = stages.stage3;
    }
}
