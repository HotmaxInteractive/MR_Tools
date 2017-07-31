using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(Camera))]
public class TextureOverlay : MonoBehaviour
{
    /// <summary>
    /// The screen is the quad where the textures are displayed
    /// </summary>
    public GameObject canvas;
    public GameObject monitorCanvas;
    /// <summary>
    /// It's the main material, used to set the color and the depth
    /// </summary>
    private Material matRGB;

    /// <summary>
    /// All the textures displayed are in 16/9
    /// </summary>
    private float aspect = 16.0f/9.0f;

    /// <summary>
    /// The main camera is the camera controlled by the ZED
    /// </summary>
    private Camera mainCamera;
    private Camera monitorCamera;

    //[Tooltip("Set the video type at the initialization")]
    private sl.VIEW videoType;

    [HideInInspector]
    public Texture2D camZedLeft;

    Texture2D depthXYZZed;
    Texture2D depthZed;
    CommandBuffer buffer;

    //public RenderTexture mask;
    void Awake()
    {
        mainCamera = GetComponent<Camera>();
        monitorCamera = GameObject.Find("Monitor Camera").GetComponent<Camera>();
        Hide();
        mainCamera.aspect = aspect;
        monitorCamera.aspect = aspect;
    }

    /// <summary>
    /// Hide the screen to any other cameras
    /// </summary>
    private void Hide()
    {
        gameObject.transform.GetChild(0).gameObject.layer = 20;
        foreach (Camera c in Camera.allCameras)
        {
            if (c != mainCamera && c != monitorCamera)
            {
                c.cullingMask &= ~(1 << 20);
            }
        }
    }



    void Start()
    {
        setFrameFOV();
    }

    public void setFrameFOV() {
        //Set textures to the shader
        matRGB = canvas.GetComponent<Renderer>().material;
        sl.ZEDCamera zedCamera = sl.ZEDCamera.GetInstance();

        //Create two textures and fill them with the ZED computed images
        zedCamera.SetGrabParametersThreadingMode(sl.SENSING_MODE.FILL, true);

        camZedLeft = zedCamera.CreateTextureImageType(videoType); //this gets set to LEFT from some magic...
        depthXYZZed = zedCamera.CreateTextureMeasureType(sl.MEASURE.XYZ);
        matRGB.SetTexture("_CameraTex", camZedLeft);
        matRGB.SetTexture("_DepthXYZTex", depthXYZZed);

        if (zedCamera.CameraIsReady)
        {
            mainCamera.fieldOfView = zedCamera.GetFOV() * Mathf.Rad2Deg;
            mainCamera.projectionMatrix = zedCamera.Projection;

            monitorCamera.fieldOfView = zedCamera.GetFOV() * Mathf.Rad2Deg;
            monitorCamera.projectionMatrix = zedCamera.Projection;

            scale(canvas.gameObject, GetFOVFromProjectionMatrix(mainCamera.projectionMatrix), false);
            scale(monitorCanvas.gameObject, GetFOVFromProjectionMatrix(monitorCamera.projectionMatrix), true);
        }
        else
        {
            scale(canvas.gameObject, mainCamera.fieldOfView, false);
            scale(monitorCanvas.gameObject, monitorCamera.fieldOfView, true);
        }

    }

    //use this if you need to scale the texture on runtime..
    public void scaleOnRuntime()
    {
        scale(canvas.gameObject, mainCamera.fieldOfView, false);
        scale(monitorCanvas.gameObject, monitorCamera.fieldOfView, true);
    }

    /// <summary>
    /// Get back the FOV from the Projection matrix, to bypass a round number
    /// </summary>
    /// <param name="projection"></param>
    /// <returns></returns>
    float GetFOVFromProjectionMatrix(Matrix4x4 projection)
    {
        return Mathf.Atan(1 / projection[1, 1]) * 2.0f;
    }


    /// <summary>
    /// Scale a screen in front of the camera, where all the textures will be rendered.
    /// </summary>
    /// <param name="screen"></param>
    /// <param name="fov"></param>
    /// <param name="isMonitor"></param>
    private void scale(GameObject screen, float fov, bool isMonitor)
    {
        float height = Mathf.Tan(0.5f * fov) * 2.0f;

        //-- for quad texture, these values are : new Vector3(height * aspect, height, 1)  // x and y are multiplied by screen.transform.localPosition.z
        //-- for highvert texture, these are    : new Vector3(height * aspect, 1, height)  // x and z are multiplied by screen.transform.localPosition.z
        if (!isMonitor)
        {
            screen.transform.localScale = new Vector3((height * aspect) * screen.transform.localPosition.z, height * screen.transform.localPosition.z, 1);
        } else
        {
            screen.transform.localScale = new Vector3(height * aspect, height, 1);
        }
    }
}

