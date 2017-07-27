using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonitorPositionToggle : MonoBehaviour
{
    MRManager _MRManager; //get controller as set on the Manager
    GameObject controller; 
    SteamVR_TrackedController controllerInput;

    GameObject cameraOffsetModel;
    bool actorMonitorFollowCamera = false;
    Vector3 lastMonitorPosition;

    

    void Awake()
    {
        _MRManager = GameObject.FindObjectOfType(typeof(MRManager)) as MRManager;
        controller = _MRManager.calibrationController;
        controllerInput = controller.GetComponent<SteamVR_TrackedController>();
        cameraOffsetModel = GameObject.Find("Zed Offset");

        controllerInput.MenuButtonClicked += doMenuButtonClicked;
    }

    protected virtual void OnApplicationQuit()
    {
        controllerInput.MenuButtonClicked -= doMenuButtonClicked;
    }



    public void doMenuButtonClicked(object sender, ClickedEventArgs e)
    {
        actorMonitorFollowCamera = !actorMonitorFollowCamera;

        if (actorMonitorFollowCamera)
        {
            lastMonitorPosition = transform.position;
            transform.parent = cameraOffsetModel.transform;

            transform.position = new Vector3(cameraOffsetModel.transform.position.x, cameraOffsetModel.transform.position.y + .2f, cameraOffsetModel.transform.position.z + .1f);
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
    
}
