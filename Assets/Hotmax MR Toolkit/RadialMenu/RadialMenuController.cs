using UnityEngine;

// Radial Menu input from Vive Controller
[RequireComponent(typeof(RadialMenu))]
public class RadialMenuController : MonoBehaviour
{

    SteamVR_TrackedController controllerInput;
    SteamVR_TrackedObject controllerInput_o;
    SteamVR_Controller.Device device;

    GameObject MRToolkit; //use to grab calibration settings
    GameObject controller; //get hand and steamVR tracked object and controller components

    protected RadialMenu menu;
    private float currentAngle; //Keep track of angle for when we click
    float PadX;
    float PadY;
    private bool touchpadTouched;

   void Start()
    {

        menu = GetComponent<RadialMenu>();
        MRToolkit = GameObject.Find("MR Toolkit");
        controller = MRToolkit.GetComponent<MRManager>().calibrationController;

        controllerInput = controller.GetComponent<SteamVR_TrackedController>();
        controllerInput_o = controller.GetComponent<SteamVR_TrackedObject>();

        Initialize();
    }
    protected virtual void Initialize()
    {
        //do nothing
    }


    private void Update()
    {
        device = SteamVR_Controller.Input((int)controllerInput_o.index); //get index of trackedObject on the controller gameobject
        PadX = device.GetAxis().x;
        PadY = device.GetAxis().y;


        //work around for PadTouched Event, which doesn't fire on update
        if (PadX != 0 || PadY != 0)
        {
            touchpadTouched = true;
            DoChangeAngle(calculateAngle(PadX, PadY));
            DoShowMenu(calculateAngle(PadX, PadY));
        } else
        {
            touchpadTouched = false;
            DoHideMenu(false);
        }


    }


    protected virtual void OnEnable()
    {
        if (controllerInput == null)
        {
           // Debug.LogError("set up the controller input!");
            return;
        }
        else
        {
            //NOTE: steamVR event system doesn't work correctly for padTouched on Oculus, so you have to use clicks to rotate
            controllerInput.PadClicked += DoPadClicked;
            controllerInput.PadUnclicked += DoPadUnclicked;

            Debug.Log("Controller input is set up");

        }
    }

    protected virtual void OnDisable()
    {
        controllerInput.PadClicked -= DoPadClicked;
        controllerInput.PadUnclicked -= DoPadUnclicked;
    }


    /*
    private void DoPadTouched(object sender, ClickedEventArgs e)
    {
        touchpadTouched = true;

        DoChangeAngle(calculateAngle(PadX,PadY));
        DoShowMenu(calculateAngle(PadX, PadY));
    }
    private void DoPadUnTouched(object sender, ClickedEventArgs e)
    {
        touchpadTouched = false;
        DoHideMenu(false);
    }
    */


    private void DoPadClicked(object sender, ClickedEventArgs e)
    {
        //Debug.Log("click - y: " + e.padY + ", x: " + e.padX);
        DoChangeAngle(calculateAngle(e.padX, e.padY));
        menu.ClickButton(calculateAngle(e.padX, e.padY));
    }
    private void DoPadUnclicked(object sender, ClickedEventArgs e)
    {
        menu.UnClickButton(currentAngle);
    }





    protected void DoClickButton(object sender = null) // The optional argument reduces the need for middleman functions in subclasses whose events likely pass object sender
    {
        menu.ClickButton(currentAngle);
    }

    protected void DoUnClickButton(object sender = null)
    {
        menu.UnClickButton(currentAngle);
    }

    protected void DoShowMenu(float initialAngle, object sender = null)
    {
        menu.ShowMenu();
        DoChangeAngle(initialAngle); // Needed to register initial touch position before the touchpad axis actually changes
    }

    protected void DoHideMenu(bool force, object sender = null)
    {
        menu.StopTouching();
        menu.HideMenu(force);
    }

    protected void DoChangeAngle(float angle, object sender = null)
    {
        currentAngle = angle;

        menu.HoverButton(currentAngle);
    }

       
    private float calculateAngle(float x, float y)
    {
        float angle = Mathf.Atan2(y, x) * Mathf.Rad2Deg;
        angle = 90.0f - angle;
        if (angle < 0)
        {
            angle += 360.0f;
        }
        return 360 - angle;
    }



    protected virtual void AttemptHapticPulse(float strength)
    {
        //do nothing
    }

    public void LogButton(int butt)
    {
        MRToolkit.GetComponent<MRManager>().SwitchConstraint(butt);
    }

}
 