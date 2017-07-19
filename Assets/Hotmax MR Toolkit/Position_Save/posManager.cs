using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class posManager : MonoBehaviour
{
    /// <summary>
    /// pos file name
    /// </summary>
    private const string pathFilePos = "pos.cfg";
    GameObject mrToolkit;
    GameObject controller; //get hand and steamVR tracked controller
    SteamVR_TrackedController controllerInput;


    bool calibrationIsOn;


    void Awake()
    {
        LoadPos();
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
            SavePos();
        }
    }

    public void SavePos()
    {
        using (System.IO.StreamWriter file = new System.IO.StreamWriter(pathFilePos))
        {
            string tx = "tx=" + transform.localPosition.x.ToString();
            string ty = "ty=" + transform.localPosition.y.ToString();
            string tz = "tz=" + transform.localPosition.z.ToString();
            string rx = "rx=" + transform.localRotation.eulerAngles.x.ToString();
            string ry = "ry=" + transform.localRotation.eulerAngles.y.ToString();
            string rz = "rz=" + transform.localRotation.eulerAngles.z.ToString();
            file.WriteLine(tx);
            file.WriteLine(ty);
            file.WriteLine(tz);
            file.WriteLine(rx);
            file.WriteLine(ry);
            file.WriteLine(rz);
            file.Close();
        }
    }

    /// <summary>
    /// Load the position from a file
    /// </summary>
    public void LoadPos()
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
        Vector3 eulerRotation = new Vector3(0, 0, 0);
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
                else if (key == "rx")
                {
                    eulerRotation.x = float.Parse(field);
                }
                else if (key == "ry")
                {
                    eulerRotation.y = float.Parse(field);
                }
                else if (key == "rz")
                {
                    eulerRotation.z = float.Parse(field);
                }
            }
        }
        transform.localPosition = position;
        transform.localRotation = Quaternion.Euler(eulerRotation.x, eulerRotation.y, eulerRotation.z);
    }
}