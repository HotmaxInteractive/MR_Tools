using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonitorPositionToggle : MonoBehaviour
{

    GameObject mrToolkit;
    GameObject controller; //get hand and steamVR tracked controller
    SteamVR_TrackedController controllerInput;

    GameObject cameraOffsetModel;
    bool actorMonitorFollowCamera = false;

    Vector3 lastMonitorPosition;

    

    void Awake()
    {
        mrToolkit = GameObject.Find("MR Toolkit");
        cameraOffsetModel = GameObject.Find("Camera Offset Model");

        controller = mrToolkit.GetComponent<MRManager>().calibrationController;

        controllerInput = controller.GetComponent<SteamVR_TrackedController>();

        controllerInput.MenuButtonClicked += doMenuButtonClicked;
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
            transform.parent = mrToolkit.transform;
            transform.localScale = transform.localScale * 3;

        }
    }
    
}
