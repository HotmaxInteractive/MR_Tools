using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;


public class calibrationBox : MonoBehaviour {

    MRManager _MRManager;
    GameObject _zedOffset;
    public Transform localZedModel;

    GameObject _calibrationController;
    GameObject _offsetMenu;
    constraintItem[] rbconst = new constraintItem[7];


    void Start () {
		_MRManager = GameObject.FindObjectOfType(typeof(MRManager)) as MRManager;
        _calibrationController = _MRManager.calibrationController;
        _offsetMenu = GameObject.Find("Offset Lock Menu");
        _zedOffset = GameObject.Find("Zed Offset");


        //TODO: make this wait until the Render Model is populated with its components
        //TODO: set this gameobject to have the same orientation of the tracked object, at the time of the callback that will be added in here in place of this invoke function
        Invoke("createRender", 1);


        //initialize the constraint array
        rbconst[0] = new constraintItem(RigidbodyConstraints.FreezePositionX);
        rbconst[1] = new constraintItem(RigidbodyConstraints.FreezePositionY);
        rbconst[2] = new constraintItem(RigidbodyConstraints.FreezePositionZ);
        rbconst[3] = new constraintItem(RigidbodyConstraints.FreezeRotationZ);
        rbconst[4] = new constraintItem(RigidbodyConstraints.FreezeRotationY);
        rbconst[5] = new constraintItem(RigidbodyConstraints.FreezeRotationX);
        rbconst[6] = new constraintItem(RigidbodyConstraints.None);


        // -- parent the radial menu to the hand and change the local values -- values depend on Vive v Oculus
        //
        if (_calibrationController == null) { Debug.LogError("Calibration controller has to have SteamVR_TrackedController component"); }
       _offsetMenu.transform.SetParent(_calibrationController.transform);
       if (VRDevice.model == "Vive. MV")
       {
           _offsetMenu.transform.localPosition = new Vector3(0f, -0.02f, 0.06f);
           _offsetMenu.transform.localEulerAngles = new Vector3(-130f, 175f, 3.8f);
       }
       else
       {
           _offsetMenu.transform.localPosition = new Vector3(0.008f, 0.008f, -0.03f);
           _offsetMenu.transform.localEulerAngles = new Vector3(-130f, 175f, 3.8f);
       }
       

    }

    void Update () {
        //sets zedOffset to equal local transform
        _zedOffset.transform.localPosition = localZedModel.localPosition;
        _zedOffset.transform.localRotation = localZedModel.localRotation;
    }


    // -- callback to instantiate the controller model in the calibration box, when it gets loaded
    //
    void createRender()
    {
        Instantiate(_MRManager.trackedObject.GetChild(0), transform);
    }


    // -- method that radial UI uses on button press
    //
    public void SwitchConstraint(int button)
    {
        if (button == 6)
        {
            localZedModel.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        }
        else
        {
            localZedModel.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            localZedModel.gameObject.GetComponent<Rigidbody>().constraints &= ~rbconst[button].constraint;

        }
    }
    public class constraintItem
    {
        public float trans;
        public RigidbodyConstraints constraint;
        public constraintItem(RigidbodyConstraints constraintArg)
        {
            constraint = constraintArg;
        }
    }
}
