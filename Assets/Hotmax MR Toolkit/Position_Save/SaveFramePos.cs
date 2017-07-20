using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveFramePos : MonoBehaviour
{
    /// <summary>
    /// pos file name
    /// </summary>
    private const string pathFilePos = "framePos.cfg";
    GameObject mrToolkit;
    GameObject controller; //get hand and steamVR tracked controller
    SteamVR_TrackedController controllerInput;


    bool calibrationIsOn;
    //[HideInInspector]
    public bool offsetsSaved = false;

    GameObject frameCalibrator;
    GameObject frameSceneCalibrator;


    void Awake()
    {
        LoadFramePosition();
        mrToolkit = GameObject.Find("MR Toolkit");

        controller = mrToolkit.GetComponent<MRManager>().calibrationController;
        controllerInput = controller.GetComponent<SteamVR_TrackedController>();

        controllerInput.TriggerClicked += doTriggerClicked;

    }

    void Update()
    {
        calibrationIsOn = mrToolkit.GetComponent<MRManager>().calibrationToolsOn;

    }
    /// <summary>
    /// Save the position
    /// </summary>
    /// 
    public void doTriggerClicked(object sender, ClickedEventArgs e)
    {
        
        if (calibrationIsOn)
        {
            frameCalibrator = Resources.Load("FrameCalibrator") as GameObject;
            Instantiate(frameCalibrator, controller.transform.position, Quaternion.identity, transform.parent);
            SaveFramePosition();
            LoadFramePosition();
            transform.parent.GetComponent<TextureOverlay>().setFrameFOV();

            offsetsSaved = true;

        }
    }

    public void SaveFramePosition()
    {
        frameSceneCalibrator = GameObject.Find("FrameCalibrator(Clone)");
        frameSceneCalibrator.name = "FrameCalibrator";

        using (System.IO.StreamWriter file = new System.IO.StreamWriter(pathFilePos))
        {
            string tx = "tx=" + frameSceneCalibrator.transform.localPosition.x.ToString();
            string ty = "ty=" + frameSceneCalibrator.transform.localPosition.y.ToString();
            string tz = "tz=" + frameSceneCalibrator.transform.localPosition.z.ToString();

            file.WriteLine(tx);
            file.WriteLine(ty);
            file.WriteLine(tz);
            file.Close();
        }
        Destroy(frameSceneCalibrator);
    }

    /// <summary>
    /// Load the position from a file
    /// </summary>
    public void LoadFramePosition()
    {
        string[] lines = null;
        try
        {
            lines = System.IO.File.ReadAllLines(pathFilePos);
        }
        catch (System.Exception)
        {
        }
        if (lines == null) return;
        Vector3 position = new Vector3(0, 0, 0);

        foreach (string line in lines)
        {
            string[] splittedLine = line.Split('=');
            if (splittedLine.Length == 2)
            {
                string key = splittedLine[0];
                string field = splittedLine[1];
                if (key == "tx")
                {
                    position.x = float.Parse(field);
                }
                else if (key == "ty")
                {
                    position.y = float.Parse(field);
                }
                else if (key == "tz")
                {
                    position.z = float.Parse(field);
                }
            }
        }
        transform.localPosition = new Vector3(0, 0, position.z);
    }
}