using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;
using Sirenix.OdinInspector;

public class calibrationController : MonoBehaviour {

    //file path for saving and loading offsets
    private const string offsetPathFilePos = "pos.cfg";
    private const string framePathFilePos = "framePos.cfg";

    //initial calibration ON recorder -- needed for first time gameObject reposition events
    private bool calibrationModeHasBeenOn = false;

    //gameobject references
    private GameObject _zedOffset;
    private GameObject _headModel;
    private GameObject _offsetMenu;
    private GameObject _head;
    private GameObject _frame;
    private MRManager _mrManager; //need for controller

    //store and events
    Calibration_Store _CalibrationStoreRef;                                                     //needed to reference non-static methods in class
    private bool _calibrationModeIsOn = Calibration_Store.calibrationModeIsOn;                  //local ref to calibrationMode_is_on  -- set initial value by referencing static member
    private Calibration_Store.stages _calibrationStage = Calibration_Store.calibrationStage;    //local ref to calibration stage enum -- set initial value by referencing static member


    #region Event Listeners
    void Awake()
    {
        Calibration_Store.calibrationModeIsOnEvent += updateLocalCalibrationMode;
        Calibration_Store.calibrationStageEvent += updateLocalCalibrationStage;

        Calibration_Store.saveZedOffsetEvent += SaveOffsetPosition;
        Calibration_Store.loadZedOffsetEvent += LoadOffsetPosition;

        Calibration_Store.saveFrameOffsetEvent += SaveFrameZPosition;
        Calibration_Store.loadFrameOffsetEvent += LoadFrameZPosition;

        //create ref for non static mutator methods in store -- and make sure calibration starts off if thats what store says
        _CalibrationStoreRef = GameObject.FindObjectOfType(typeof(Calibration_Store)) as Calibration_Store;
        if (_calibrationModeIsOn == false)
        {
            updateLocalCalibrationMode(false);
        }
    }
    protected virtual void OnApplicationQuit()
    {
        Calibration_Store.calibrationModeIsOnEvent -= updateLocalCalibrationMode;
        Calibration_Store.calibrationStageEvent -= updateLocalCalibrationStage;

        Calibration_Store.saveZedOffsetEvent -= SaveOffsetPosition;
        Calibration_Store.loadZedOffsetEvent -= LoadOffsetPosition;

        Calibration_Store.saveFrameOffsetEvent -= SaveFrameZPosition;
        Calibration_Store.loadFrameOffsetEvent -= LoadFrameZPosition;
    }
    #endregion


    void Start() {}

    //reposition helpers
    private void setRadialMenuTransform(GameObject menu)
    {
        if (VRDevice.model == "Vive. MV")
        {
            menu.transform.localPosition = new Vector3(0f, -0.02f, 0.06f);
            menu.transform.localEulerAngles = new Vector3(-130f, 175f, 3.8f);
        }
        else
        {
            menu.transform.localPosition = new Vector3(0.008f, 0.008f, -0.03f);
            menu.transform.localEulerAngles = new Vector3(-130f, 175f, 3.8f);
        }
    }
    private void setHeadTransform(GameObject head)
    {  
        head.transform.localPosition = new Vector3(0, 0, 0);
        head.transform.localEulerAngles = new Vector3(0, 0, 0);
    }


    #region EventCallbacks
    private void updateLocalCalibrationMode(bool value)
    {
        _calibrationModeIsOn = value;

        _head = GameObject.FindGameObjectWithTag("Player").transform.GetChild(0).gameObject;
        _mrManager = GameObject.FindObjectOfType(typeof(MRManager)) as MRManager;
        Transform _controller = _mrManager.calibrationController.transform;

        if (value == true)
        {
            //turn everything on that needs to be turned on
            if (calibrationModeHasBeenOn)
            {
                _head.transform.FindChild("Head Offset Model").gameObject.SetActive(true);
                _controller.FindChild("Offset Lock Menu").gameObject.SetActive(true);
                foreach (Transform child in transform) { child.gameObject.SetActive(true); }
            }
            else //if first time then reposition stuff as well
            {
                _offsetMenu = transform.FindChild("Offset Lock Menu").gameObject;
                _headModel = transform.FindChild("Head Offset Model").gameObject;

                _headModel.transform.SetParent(_head.transform);
                _head.transform.FindChild("Head Offset Model").gameObject.SetActive(true);
                _offsetMenu.transform.SetParent(_controller);
                _controller.FindChild("Offset Lock Menu").gameObject.SetActive(true);

                foreach (Transform child in transform) { child.gameObject.SetActive(true); }

                //set radial position as well first time
                setRadialMenuTransform(_controller.FindChild("Offset Lock Menu").gameObject);
                setHeadTransform(_head.transform.FindChild("Head Offset Model").gameObject);

            }

            //make sure calibration stage is set to 0
            _CalibrationStoreRef.SET_STAGE_0();

        }

        else //value is false
        {
            //if calibration mode has already ran, then target the gameobjects directly
            if (calibrationModeHasBeenOn)
            {
                _head.transform.FindChild("Head Offset Model").gameObject.SetActive(false);
                _controller.FindChild("Offset Lock Menu").gameObject.SetActive(false);
            }
            foreach (Transform child in transform) { child.gameObject.SetActive(false); }
        }

        //on initial calibration mode do a lot of work we dont have to do again
        if (_calibrationModeIsOn && !calibrationModeHasBeenOn) { calibrationModeHasBeenOn = true; }
    }


    private void updateLocalCalibrationStage(Calibration_Store.stages returnValue)
    {
        _calibrationStage = returnValue;
    }
    #endregion


    #region Save Features

    [BoxGroup("Save Camera Offset"), Button]
    public void SaveOffsetPosition()
    {
        _zedOffset = GameObject.Find("Zed Offset");

        using (System.IO.StreamWriter file = new System.IO.StreamWriter(offsetPathFilePos))
        {
            string tx = "tx=" + _zedOffset.transform.localPosition.x.ToString();
            string ty = "ty=" + _zedOffset.transform.localPosition.y.ToString();
            string tz = "tz=" + _zedOffset.transform.localPosition.z.ToString();
            string rx = "rx=" + _zedOffset.transform.localRotation.eulerAngles.x.ToString();
            string ry = "ry=" + _zedOffset.transform.localRotation.eulerAngles.y.ToString();
            string rz = "rz=" + _zedOffset.transform.localRotation.eulerAngles.z.ToString();
            file.WriteLine(tx);
            file.WriteLine(ty);
            file.WriteLine(tz);
            file.WriteLine(rx);
            file.WriteLine(ry);
            file.WriteLine(rz);
            file.Close();
        }
    }

    [BoxGroup("Save Camera Offset"), Button]
    public void LoadOffsetPosition()
    {
        print("load zed offset");
        _zedOffset = GameObject.Find("Zed Offset");

        string[] lines = null;
        try
        {
            lines = System.IO.File.ReadAllLines(offsetPathFilePos);
        }
        catch (System.Exception) { }

        if (lines == null) return;
        Vector3 position = new Vector3(0, 0, 0);
        Vector3 eulerRotation = new Vector3(0, 0, 0);

        foreach (string line in lines)
        {
            string[] splittedLine = line.Split('=');
            if (splittedLine.Length == 2)
            {
                string key = splittedLine[0];
                string field = splittedLine[1];
                if (key == "tx") { position.x = float.Parse(field); }
                else if (key == "ty") { position.y = float.Parse(field); }            
                else if (key == "tz") { position.z = float.Parse(field); }   
                else if (key == "rx") { eulerRotation.x = float.Parse(field); }
                else if (key == "ry") { eulerRotation.y = float.Parse(field); }
                else if (key == "rz") { eulerRotation.z = float.Parse(field); }
            }
        }
        _zedOffset.transform.localPosition = position;
        _zedOffset.transform.localRotation = Quaternion.Euler(eulerRotation.x, eulerRotation.y, eulerRotation.z);
    }


    // -- SAVE AND LOAD FRAME
    //
    [BoxGroup("Save Frame Z Position"), Button]
    public void SaveFrameZPosition()
    {
        _frame = GameObject.Find("Frame");
        //_mrManager = GameObject.FindObjectOfType(typeof(MRManager)) as MRManager;

        ////local variables for controller and object we slot into controller
        //Transform controller = _mrManager.trackedObject;
        //GameObject frameCalibrator = Resources.Load("FrameCalibrator") as GameObject;

        ////instantiate and write to file, then destroy
        //Instantiate(frameCalibrator, controller.transform.position, Quaternion.identity, _frame.transform.parent);
        //GameObject frameSceneCalibrator = GameObject.Find("FrameCalibrator(Clone)");
        //using (System.IO.StreamWriter file = new System.IO.StreamWriter(framePathFilePos))
        //{
        //    string tx = "tx=" + frameSceneCalibrator.transform.localPosition.x.ToString();
        //    string ty = "ty=" + frameSceneCalibrator.transform.localPosition.y.ToString();
        //    string tz = "tz=" + frameSceneCalibrator.transform.localPosition.z.ToString();
        //    file.WriteLine(tx);
        //    file.WriteLine(ty);
        //    file.WriteLine(tz);
        //    file.Close();
        //}
        //Destroy(frameSceneCalibrator);

        ////call LOAD mutator
        //_CalibrationStoreRef.LOAD_FRAME_OFFSET();

    }

    [BoxGroup("Save Frame Z Position"), Button]
    public void LoadFrameZPosition()
    {
        _frame = GameObject.Find("Frame");

        string[] lines = null;
        try
        {
            lines = System.IO.File.ReadAllLines(framePathFilePos);
        }
        catch (System.Exception)
        { }

        if (lines == null) return;
        Vector3 position = new Vector3(0, 0, 0);

        foreach (string line in lines)
        {
            string[] splittedLine = line.Split('=');
            if (splittedLine.Length == 2)
            {
                string key = splittedLine[0];
                string field = splittedLine[1];
                if (key == "tx"){ position.x = float.Parse(field); }
                else if (key == "ty"){ position.y = float.Parse(field); }
                else if (key == "tz"){ position.z = float.Parse(field); }
            }
        }

        //set the local position from file, and run the scale method again
        _frame.transform.localPosition = new Vector3(0, 0, position.z);
        _frame.transform.parent.GetComponent<TextureOverlay>().scaleOnRuntime();
    }
    #endregion


    #region FeatureShortcut
    [BoxGroup("Toggle Calibration Mode"), Button]
    public void TurnOnCalibrationMode()
    {
        _CalibrationStoreRef.SET_CALIBRATION_IS_ON(true);
    }

    [BoxGroup("Toggle Calibration Mode"), Button]
    public void TurnOffCalibrationMode()
    {
        _CalibrationStoreRef.SET_CALIBRATION_IS_ON(false);
        _CalibrationStoreRef.SET_STAGE_0(); //reset the stage to beginning
    }
    #endregion
}
