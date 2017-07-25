using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VR;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using System.IO;

public class MRManager : MonoBehaviour
{
    string vrDevice;

    public GameObject calibrationController;
    public Transform trackedObject;
	[HideInInspector]
    public Transform head;

    public bool monitorOn;
    public bool calibrationToolsOn;

    Transform hmdModel;

    [HideInInspector]
    public GameObject monitor;
    GameObject zedFrame;
    GameObject frameCalibrator;

    GameObject cameraCalibrator;
    [HideInInspector]
    public GameObject cameraHandle;
    GameObject zedOffset;

    [HideInInspector]
    public GameObject offsetTools;
    GameObject offsetMenu;

	[HideInInspector]
	public GameObject steamVRComponents;
	[HideInInspector]
	public GameObject NVRComponents;
	[HideInInspector]
	public GameObject VRTKComponents;
	[HideInInspector]

	public bool steamVRLoadedInInspector = false;
	[HideInInspector]
	public bool NVRLoadedInInspector = false;

	string componentRename;

    constraintItem[] rbconst = new constraintItem[7];
    [HideInInspector]
    public bool constraintSwitched = false;


    void Start()
    {

        vrDevice = VRDevice.model;

        if (calibrationController == null)
        {
            Debug.Log("<color=red>Callibration controller has to have SteamVR_TrackedController component :</color>");
        }

        hmdModel = GameObject.Find("Head Offset Model").transform;
        head = GameObject.FindGameObjectWithTag("Player").transform.GetChild(0).transform;
        cameraCalibrator = GameObject.Find("Camera Offset Model");
        cameraHandle = GameObject.Find("Offset Handle");
        zedOffset = GameObject.Find("Zed Offset");

        offsetTools = GameObject.Find("Offset Tools");
        offsetMenu = GameObject.Find("Offset Menu");
        monitor = GameObject.Find("Actor Monitor");

        //parent the radial menu to the hand and change the local values -- values depend on Vive v Oculus
        offsetMenu.transform.SetParent(calibrationController.transform);
        if (vrDevice == "Vive. MV")
        {
            offsetMenu.transform.localPosition = new Vector3(0f, -0.02f, 0.06f);
            offsetMenu.transform.localEulerAngles = new Vector3(-130f, 175f, 3.8f);
        } else
        {
            offsetMenu.transform.localPosition = new Vector3(0.008f, 0.008f, -0.03f);
            offsetMenu.transform.localEulerAngles = new Vector3(-130f, 175f, 3.8f);
        }

        rbconst[0] = new constraintItem(RigidbodyConstraints.FreezePositionX);
        rbconst[1] = new constraintItem(RigidbodyConstraints.FreezePositionY);
        rbconst[2] = new constraintItem(RigidbodyConstraints.FreezePositionZ);
        rbconst[3] = new constraintItem(RigidbodyConstraints.FreezeRotationZ);
        rbconst[4] = new constraintItem(RigidbodyConstraints.FreezeRotationY);
        rbconst[5] = new constraintItem(RigidbodyConstraints.FreezeRotationX);
        rbconst[6] = new constraintItem(RigidbodyConstraints.None);


        //ON START PARENT THE ZEDOFF TO THE TRACKED OBJECT
        zedOffset.transform.SetParent(trackedObject.transform);
        //zedOffset.transform.localPosition = new Vector3(0, 0, 0);


    }


    //gets called publically from the UI
    public void SwitchConstraint(int button)
    {
        if (button == 6)
        {
            zedOffset.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        }
        else
        {
            zedOffset.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            zedOffset.gameObject.GetComponent<Rigidbody>().constraints &= ~rbconst[button].constraint;

        }

        constraintSwitched = true;

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


    void Update()
    {

        //Turn tools on that you need through editor
        if (monitorOn)
        {
            monitor.SetActive(true);
        } else
        {
            monitor.SetActive(false);
        }
        
        if (calibrationToolsOn)
        {
            offsetTools.gameObject.SetActive(true);
            zedOffset.GetComponent<BoxCollider>().enabled = true;
            cameraHandle.SetActive(true);
            offsetMenu.SetActive(true);
        } else
        {
            offsetTools.gameObject.SetActive(false);
            zedOffset.GetComponent<BoxCollider>().enabled = false;
            cameraHandle.SetActive(false);
            offsetMenu.SetActive(false);
        }

        //Calibration models -- staying in line
        hmdModel.position = head.position;
        hmdModel.rotation = head.rotation;

        cameraCalibrator.transform.position = zedOffset.transform.position;
        cameraCalibrator.transform.rotation = zedOffset.transform.rotation;

    }


//------------------------------------------------------------------------------------------------------------




	public void loadVRComponent(GameObject VRComponent){

		if (GameObject.FindGameObjectWithTag("Player") == null) {
			
			Instantiate (VRComponent);
			VRComponent.tag = "Player"; 

			head = VRComponent.transform.GetChild(0);

			GameObject.FindGameObjectWithTag("Player").name = componentRename;


			if(GUI.changed){
				EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
			}
		}
	}

	public void getSteamVRFromResourcesFolder(){

		if(!Directory.Exists("Assets/Dependencies/SteamVR")){
			Debug.Log("SteamVR plugin is not in your Assets folder!");
		}else{
			steamVRComponents = Resources.Load ("[CameraRig]") as GameObject;
			componentRename = steamVRComponents.name;

			RemoveOldInteractionComponents ();

			HotmaxInteraction[] hotmaxInteractionObjects = GameObject.FindObjectsOfType(typeof(HotmaxInteraction)) as HotmaxInteraction[];

			foreach (HotmaxInteraction hotmaxInteraction in hotmaxInteractionObjects) {
				if (hotmaxInteraction.gameObject.GetComponent<Valve.VR.InteractionSystem.Interactable> () == null) {
					hotmaxInteraction.gameObject.AddComponent<Valve.VR.InteractionSystem.Interactable> ();
				}
			}
		}
	}

	public void getNVRFromResourcesFolder(){
		if (!Directory.Exists ("Assets/Dependencies/NewtonVR")) {
			Debug.Log("NewtonVR plugin is not in your Assets folder!");

		} else {
			NVRComponents = Resources.Load ("NVRPlayer") as GameObject;
            NVRComponents.GetComponent<NewtonVR.NVRCanvasInput>().LaserEnabled = false;
			componentRename = NVRComponents.name;

			RemoveOldInteractionComponents ();

			HotmaxInteraction[] hotmaxInteractionObjects = GameObject.FindObjectsOfType(typeof(HotmaxInteraction)) as HotmaxInteraction[];

			foreach (HotmaxInteraction hotmaxInteraction in hotmaxInteractionObjects) {
               
                if (hotmaxInteraction.gameObject.GetComponent<NewtonVR.NVRInteractableItem> () == null) {
					hotmaxInteraction.gameObject.AddComponent<NewtonVR.NVRInteractableItem> ();
                    hotmaxInteraction.gameObject.GetComponent<NewtonVR.NVRInteractableItem>().DisableKinematicOnAttach = true;
                    hotmaxInteraction.gameObject.GetComponent<NewtonVR.NVRInteractableItem>().EnableKinematicOnDetach = true;
                    hotmaxInteraction.gameObject.GetComponent<NewtonVR.NVRInteractableItem>().EnableGravityOnDetach = false;
                }
			}
		}
	}

	// Use this method to wipe out old interaction components before loading in the needed one. It finds the HotmaxInteraction flag then removes the interaction script.
	public void RemoveOldInteractionComponents(){
		HotmaxInteraction[] hotmaxInteractionObjects = GameObject.FindObjectsOfType(typeof(HotmaxInteraction)) as HotmaxInteraction[];

		foreach (HotmaxInteraction hotmaxInteraction in hotmaxInteractionObjects) {
			if (hotmaxInteraction.gameObject.GetComponent<Valve.VR.InteractionSystem.Interactable> () != null) {
				DestroyImmediate(hotmaxInteraction.gameObject.GetComponent<Valve.VR.InteractionSystem.Interactable> ());
			}
			if (hotmaxInteraction.gameObject.GetComponent<NewtonVR.NVRInteractableItem> () != null) {
				DestroyImmediate(hotmaxInteraction.gameObject.GetComponent<NewtonVR.NVRInteractableItem> ());
			}
		}
	}
}