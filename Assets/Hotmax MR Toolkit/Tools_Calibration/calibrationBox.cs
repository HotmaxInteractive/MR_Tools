using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;


public class calibrationBox : MonoBehaviour {

    MRManager _MRManager;
    zedController _zedOffset;

    public Transform localZedModel;

    GameObject _calibrationController;
    GameObject _offsetMenu;
    constraintItem[] rbconst = new constraintItem[7];


    void Start () {
		_MRManager = GameObject.FindObjectOfType(typeof(MRManager)) as MRManager;
        _zedOffset = GameObject.FindObjectOfType(typeof(zedController)) as zedController;
        print(_zedOffset);

        _calibrationController = _MRManager.calibrationController;
        _offsetMenu = GameObject.Find("Offset Lock Menu");


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

    }

    void Update () {
        //print(localZedModel);
        //print(_zedOffset);
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
