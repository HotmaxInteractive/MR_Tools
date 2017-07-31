using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class raycastToMenu : MonoBehaviour
{

    GameObject controller; //get hand and steamVR tracked controller
    SteamVR_TrackedController controllerInput;
    public Transform pointer;

    MRManager mrManager;

    private void Start()
    {
        mrManager = GameObject.FindObjectOfType(typeof(MRManager)) as MRManager;
        controller = mrManager.calibrationController;
        controllerInput = controller.GetComponent<SteamVR_TrackedController>();

        controllerInput.TriggerClicked += doTriggerClicked;
    }

    public void doTriggerClicked(object sender, ClickedEventArgs e)
    {

        //get direction of the controller
        Ray myRay = new Ray(this.transform.position, this.transform.forward);
        float length = 10000;

        RaycastHit hit;
        if (Physics.Raycast(myRay, out hit, length))
        {
            length = Vector3.Distance(hit.point, this.transform.position);

            if (hit.transform.gameObject.GetComponent<uiButton>() != null)
            {
                hit.transform.gameObject.GetComponent<uiButton>().raycastBtnHit = true;
            }
        }
    }

    void Update()
    {
        Ray myRay = new Ray(this.transform.position, this.transform.forward);
        float length = 10000;

        RaycastHit hit;
        if (Physics.Raycast(myRay, out hit, length))
        {
            length = Vector3.Distance(hit.point, this.transform.position);

            pointer.transform.position = hit.point;

        }
    }
}
