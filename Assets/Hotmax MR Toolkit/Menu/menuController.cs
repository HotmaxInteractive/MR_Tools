using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class menuController : MonoBehaviour {

    //get store refs for non-static mutators
    Main_Store _MainStoreRef;
    Calibration_Store _CalibrationStoreRef;
    Monitor_Store _MonitorStoreRef;

    //local refs
    private GameObject canvas;
    private GameObject calibrationTop;
    private GameObject calibrationBody;

    //get local refs and initial values of store variables
    private bool _menuIsOpen = Main_Store.menuIsOpen;
    private bool _calibrationModeIsOn = Calibration_Store.calibrationModeIsOn;
    private Calibration_Store.stages _calibrationStage = Calibration_Store.calibrationStage;
    private bool _actorMonitorIsOn = Monitor_Store.actorMonitorIsOn;
    private bool _actorMonitorFollowCamera = Monitor_Store.actorMonitorFollowCamera;


    #region Event Listeners
    void Awake()
    {
        Main_Store.menuIsOpenEvent += updateMenuIsOpen;

        Calibration_Store.calibrationModeIsOnEvent += updateLocalCalibrationMode;
        Calibration_Store.calibrationStageEvent += updateLocalCalibrationStage;

        Monitor_Store.monitorIsOnEvent += updateLocalMonitorMode;
        Monitor_Store.actorMonitorFollowCameraEvent += updateActorMonitorFollowCamera;

        //create ref for non static mutator methods in store -- and make sure menu starts off if thats what store says
        _MainStoreRef = GameObject.FindObjectOfType(typeof(Main_Store)) as Main_Store;
        _CalibrationStoreRef = GameObject.FindObjectOfType(typeof(Calibration_Store)) as Calibration_Store;
        _MonitorStoreRef = GameObject.FindObjectOfType(typeof(Monitor_Store)) as Monitor_Store;
        if (_menuIsOpen == false)
        {
            updateMenuIsOpen(false);
        }
    }
    protected virtual void OnApplicationQuit()
    {
        Main_Store.menuIsOpenEvent -= updateMenuIsOpen;

        Calibration_Store.calibrationModeIsOnEvent -= updateLocalCalibrationMode;
        Calibration_Store.calibrationStageEvent -= updateLocalCalibrationStage;

        Monitor_Store.monitorIsOnEvent -= updateLocalMonitorMode;
        Monitor_Store.actorMonitorFollowCameraEvent -= updateActorMonitorFollowCamera;
    }
    #endregion


    void Start ()
    {
        //setter to eliminate any bugs
        canvas = transform.FindChild("Canvas").gameObject;
        resetMenu();
    }

    private void resetMenu()
    {
        //reset the menu back to the main Components
        foreach (Transform child in canvas.transform) { child.gameObject.SetActive(false); }
        canvas.transform.FindChild("Top Main").gameObject.SetActive(true);
        canvas.transform.FindChild("Body Main").gameObject.SetActive(true);
        canvas.transform.FindChild("Bottom Main").gameObject.SetActive(true);
    }

    public void incrementStageMutatorNext()
    {
        print("next button");
        switch (_calibrationStage)
        {
            case Calibration_Store.stages.stage0:
                _CalibrationStoreRef.SET_STAGE_1();
                break;
            case Calibration_Store.stages.stage1:
                _CalibrationStoreRef.SET_STAGE_2();
                break;
            case Calibration_Store.stages.stage2:
                _CalibrationStoreRef.SET_STAGE_3();
                break;
            case Calibration_Store.stages.stage3:
                _CalibrationStoreRef.SET_CALIBRATION_IS_ON(false);
                break;
            default:
                print("you dun fucked up now");
                break;
        }
    }
    public void incrementStageMutatorPrev()
    {
        print("prev button");
        switch (_calibrationStage)
        {
            case Calibration_Store.stages.stage0:
                _CalibrationStoreRef.SET_CALIBRATION_IS_ON(false);
                break;
            case Calibration_Store.stages.stage1:
                _CalibrationStoreRef.SET_STAGE_0();
                break;
            case Calibration_Store.stages.stage2:
                _CalibrationStoreRef.SET_STAGE_1();
                break;
            case Calibration_Store.stages.stage3:
                _CalibrationStoreRef.SET_STAGE_2();
                break;
            default:
                print("you dun fucked up now");
                break;
        }
    }


    #region Event Callbacks

    private void updateMenuIsOpen(bool value)
    {
        canvas = transform.FindChild("Canvas").gameObject; 
        _menuIsOpen = value;

        if (_menuIsOpen) {
            canvas.SetActive(true); 
        } else {
            canvas.SetActive(false);
            resetMenu();
        }
    }

    private void updateLocalCalibrationMode(bool value)
    {
        _calibrationModeIsOn = value;

        if (_calibrationModeIsOn == true)
        {
            //make sure that menu gets turned on
            _MainStoreRef.SET_MENU_IS_OPEN(true);

            //set menu to calibration mode
            foreach (Transform child in canvas.transform) { child.gameObject.SetActive(false); }
            canvas.transform.FindChild("Top Calibration").gameObject.SetActive(true);
            canvas.transform.FindChild("Body Calibration").gameObject.SetActive(true);
            canvas.transform.FindChild("Bottom Calibration").gameObject.SetActive(true);
        } else
        {
            resetMenu();
        }
    }

    private void updateLocalCalibrationStage(Calibration_Store.stages value)
    {
        _calibrationStage = value;

        //find local menu representations
        canvas = transform.FindChild("Canvas").gameObject;
        calibrationTop = canvas.transform.FindChild("Top Calibration").gameObject;
        calibrationBody = canvas.transform.FindChild("Body Calibration").gameObject;

        //deactive every child
        foreach (Transform child in calibrationTop.transform) { child.gameObject.SetActive(false); }
        foreach (Transform child in calibrationBody.transform) { child.gameObject.SetActive(false); }

        //active current stage
        switch (value)
        {
            case Calibration_Store.stages.stage0:
                calibrationTop.transform.GetChild(0).gameObject.SetActive(true);
                calibrationBody.transform.GetChild(0).gameObject.SetActive(true);
                break;
            case Calibration_Store.stages.stage1:
                calibrationTop.transform.GetChild(1).gameObject.SetActive(true);
                calibrationBody.transform.GetChild(1).gameObject.SetActive(true);
                break;
            case Calibration_Store.stages.stage2:
                calibrationTop.transform.GetChild(2).gameObject.SetActive(true);
                calibrationBody.transform.GetChild(2).gameObject.SetActive(true);
                break;
            case Calibration_Store.stages.stage3:
                calibrationTop.transform.GetChild(3).gameObject.SetActive(true);
                calibrationBody.transform.GetChild(3).gameObject.SetActive(true);
                break;
            default:
                calibrationTop.transform.GetChild(0).gameObject.SetActive(true);
                calibrationBody.transform.GetChild(0).gameObject.SetActive(true);
                break;
        }

    }

    private void updateLocalMonitorMode(bool value)
    {
        _actorMonitorIsOn = value;
    }

    private void updateActorMonitorFollowCamera(bool value)
    {
        _actorMonitorFollowCamera = value;
    }

    #endregion


    #region Feature Shortcut
    [BoxGroup("Toggle Menu Open"), Button]
    public void TurnOnMenu()
    {
        _MainStoreRef.SET_MENU_IS_OPEN(true);
    }

    [BoxGroup("Toggle Menu Open"), Button]
    public void TurnOffMenu()
    {
        _MainStoreRef.SET_MENU_IS_OPEN(false);
    }
    #endregion

}
