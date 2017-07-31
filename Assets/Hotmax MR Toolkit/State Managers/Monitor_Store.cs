using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monitor_Store : MonoBehaviour {

    //Actor Monitor is On
    public static bool actorMonitorIsOn = true;
    public delegate void monitorIsOnHandler(bool value);
    public static event monitorIsOnHandler monitorIsOnEvent;

    //Actor Monitor is following the camera
    public static bool actorMonitorFollowCamera = false;
    public delegate void actorMonitorFollowCameraHandler(bool value);
    public static event actorMonitorFollowCameraHandler actorMonitorFollowCameraEvent;


    void Start () {}
	void Update () {}


    // -- mutators
    //
    public void SET_MONITOR_IS_ON(bool value)
    {
        actorMonitorIsOn = value;
        monitorIsOnEvent(value);
    }

    public void SET_MONITOR_IS_FOLLOWING_CAMERA(bool value)
    {
        actorMonitorFollowCamera = value;
        actorMonitorFollowCameraEvent(value);
    }
}
