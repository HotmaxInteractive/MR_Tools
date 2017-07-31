using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zedController : MonoBehaviour {

    //get store for initial load offsets
    Calibration_Store _CalibrationStoreRef;

    //get MRManager for tracked object
    MRManager _MRManager;

    void Start () {
        _CalibrationStoreRef = GameObject.FindObjectOfType(typeof(Calibration_Store)) as Calibration_Store;
        _MRManager = GameObject.FindObjectOfType(typeof(MRManager)) as MRManager;

        //send event to tell the calibrator controller to load the zeds initial offsets
        _CalibrationStoreRef.LOAD_ZED_OFFSET();
        //_CalibrationStoreRef.LOAD_FRAME_OFFSET();

        //nest this component in the tracked object
        transform.SetParent(_MRManager.trackedObject.transform);
    }
	
	void Update () {}
}
