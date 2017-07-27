using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using System.IO;

public class MRManager : MonoBehaviour
{

    public GameObject calibrationController;
    public Transform trackedObject;
	[HideInInspector]
    public Transform head;

    public bool monitorOn;
    public bool calibrationToolsOn;

    Transform hmdModel;

    [HideInInspector]
    public GameObject monitor;

    GameObject zedOffset;

    [HideInInspector]
    public GameObject calibrationTools;

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

    [HideInInspector]
    public bool constraintSwitched = false;


    void Start()
    {

        hmdModel = GameObject.Find("Head Offset Model").transform;
        head = GameObject.FindGameObjectWithTag("Player").transform.GetChild(0).transform;
        zedOffset = GameObject.Find("Zed Offset");


        calibrationTools = GameObject.Find("Calibration");
        monitor = GameObject.Find("Actor Monitor");


        // -- nest the zedOffset into the trackedObject
        //
        zedOffset.transform.SetParent(trackedObject.transform);

    }


    void Update()
    {
        //TODO -- move this stateful stuff into their proper contexts

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
            calibrationTools.gameObject.SetActive(true);
        } else
        {
            calibrationTools.gameObject.SetActive(false);
        }

        //Calibration models -- staying in line
        hmdModel.position = head.position;
        hmdModel.rotation = head.rotation;

       

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