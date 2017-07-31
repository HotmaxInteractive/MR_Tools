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
        //TODO can't reference this stuff if calibration mode is off
        zedOffset = GameObject.Find("Zed Offset");


        calibrationTools = GameObject.Find("Calibration");
        monitor = GameObject.Find("Actor Monitor");

    }


    void Update()
    {    }


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