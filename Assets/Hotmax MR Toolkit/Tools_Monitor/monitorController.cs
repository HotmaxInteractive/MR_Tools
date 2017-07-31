using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class monitorController : MonoBehaviour {

    //gameobject reference
    MRManager _MRManager;
    zedController _zedOffset;
    Vector3 lastMonitorPosition;
    public GameObject normalCase;
    public GameObject glowCase;

    //store and events
    Monitor_Store _MonitorStoreRef;   //needed to reference non-static mutators methods in class
    private bool _calibrationModeIsOn = Calibration_Store.calibrationModeIsOn;                  //local ref to calibrationMode_is_on  -- set initial value by referencing static member
    private Calibration_Store.stages _calibrationStage = Calibration_Store.calibrationStage;    //local ref to calibration stage enum -- set initial value by referencing static member

    #region Event Listeners
    void Awake()
    {
        Monitor_Store.monitorIsOnEvent += updateLocalMonitorMode;
        Monitor_Store.actorMonitorFollowCameraEvent += updateActorMonitorFollowCamera;

        Calibration_Store.calibrationStageEvent += turnOnTutorialGlowOnStage;
        Calibration_Store.calibrationModeIsOnEvent += turnOnTutorialGlowOnStart;
    }
    protected virtual void OnApplicationQuit()
    {
        Monitor_Store.monitorIsOnEvent -= updateLocalMonitorMode;
        Monitor_Store.actorMonitorFollowCameraEvent -= updateActorMonitorFollowCamera;

        Calibration_Store.calibrationStageEvent += turnOnTutorialGlowOnStage;
        Calibration_Store.calibrationModeIsOnEvent += turnOnTutorialGlowOnStart;
    }
    #endregion

    void Start()
    {
        //create ref for non static mutator methods in store
        _MonitorStoreRef = GameObject.FindObjectOfType(typeof(Monitor_Store)) as Monitor_Store;
    }

    #region EventCallbacks
    private void updateLocalMonitorMode(bool value)
    {
        if (value == true)
        {
            foreach(Transform child in transform) { child.gameObject.SetActive(true); }
            glowCase.SetActive(false); //turn tutorial glow off though
        } else
        {
            foreach(Transform child in transform) { child.gameObject.SetActive(false); }
        }
    }

    private void updateActorMonitorFollowCamera(bool value)
    {
        _zedOffset = GameObject.FindObjectOfType(typeof(zedController)) as zedController;
        _MRManager = GameObject.FindObjectOfType(typeof(MRManager)) as MRManager;

        if (value == true)
        {
            lastMonitorPosition = transform.position;
            transform.parent = _zedOffset.transform;

            transform.position = new Vector3(_zedOffset.transform.position.x, _zedOffset.transform.position.y + .2f, _zedOffset.transform.position.z + .1f);
            transform.localEulerAngles = new Vector3(0, 180, 0);
            transform.localScale = transform.localScale / 3;
        }
        else
        {
            transform.position = lastMonitorPosition;
            transform.parent = _MRManager.transform;
            transform.localScale = transform.localScale * 3;

        }
    }

    private void turnOnTutorialGlowOnStage(Calibration_Store.stages value)
    {
        _calibrationStage = value;
        checkTutorialGlow();
    }
    private void turnOnTutorialGlowOnStart(bool value)
    {
        _calibrationModeIsOn = value;
        checkTutorialGlow();
    }
    private void checkTutorialGlow()
    {
        //check to see if calibration mode is on, and stage is at one
        if (_calibrationStage == Calibration_Store.stages.stage0 && _calibrationModeIsOn)
        {
            normalCase.SetActive(false);
            glowCase.SetActive(true);
        }
        else
        {
            normalCase.SetActive(true);
            glowCase.SetActive(false);
        }
    }
    #endregion


    #region FeatureShortcut
    [BoxGroup("Toggle Monitor Mode"), Button]
    public void TurnOnMonitor()
    {
        _MonitorStoreRef.SET_MONITOR_IS_ON(true);
    }

    [BoxGroup("Toggle Monitor Mode"), Button]
    public void TurnOffMonitor()
    {
        _MonitorStoreRef.SET_MONITOR_IS_ON(false);
    }
    #endregion
}
