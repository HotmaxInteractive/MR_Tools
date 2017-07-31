using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using System;

[CustomEditor(typeof(MRManager)), CanEditMultipleObjects] 
public class MRManagerEditor : Editor
{
	//private posManager positionManager;
    //private SaveFramePos framePosManager;

	private MRManager mrManager;

	Texture2D headerTexture;
	static GUIStyle blackBack;
	static GUIStyle titleLabelStyle;
	static Color titleColor;

	public bool loadSteamVR;
	public bool loadNVR;

	protected virtual void OnEnable()
	{
		mrManager = (MRManager)target;

		// These methods essentially save the state of the chosen VR plugin on the start of the game.
		if (mrManager.steamVRLoadedInInspector) {
			loadSteamVR = true;
		}
		if (mrManager.NVRLoadedInInspector) {
			loadNVR = true;
		}

		//positionManager = GameObject.Find("Zed Offset").GetComponent<posManager>();
		headerTexture = Resources.Load<Texture2D>("hotmaxLogo/logo-big");
		blackBack = new GUIStyle();
		blackBack.normal.background = MakeTex(4, 4, Color.black);
		titleColor = new Color(0.12f, 0.16f, 0, 4f);

	}

	public override void OnInspectorGUI(){
		
		if (GameObject.FindGameObjectWithTag ("Player") == null) {
			loadSteamVR = false;
			loadNVR = false;
			mrManager.RemoveOldInteractionComponents ();
		}

		EditorGUILayout.Separator();
		GUI.skin.label.alignment = TextAnchor.MiddleCenter;
		GUILayout.BeginHorizontal(blackBack);

		GUILayout.Label(headerTexture, GUILayout.ExpandWidth(true));
		GUI.skin.label.alignment = TextAnchor.MiddleLeft;
		GUILayout.EndHorizontal();


		//begin normal settings

		EditorGUILayout.Separator();
		EditorGUILayout.BeginHorizontal();
		DrawLabel("General Settings");
		EditorGUILayout.EndHorizontal();

		//------------------------------------------------------------------------------------------------------------------------------------------


		loadSteamVR = EditorGUILayout.Toggle("Load SteamVR", loadSteamVR);
		if (loadSteamVR) {

			loadNVR = false;

			mrManager.steamVRLoadedInInspector = true;

			// load the corresponding prefab from the Resources folder.
			mrManager.getSteamVRFromResourcesFolder ();
			mrManager.loadVRComponent (mrManager.steamVRComponents);

		} else {
			mrManager.steamVRLoadedInInspector = false;
			DestroyImmediate (GameObject.Find ("[CameraRig]"));
		}


		loadNVR = EditorGUILayout.Toggle("Load NVR", loadNVR);
		if (loadNVR) {

			loadSteamVR = false;

			mrManager.NVRLoadedInInspector = true;

			// load the corresponding prefab from the Resources folder.
			mrManager.getNVRFromResourcesFolder ();
			mrManager.loadVRComponent (mrManager.NVRComponents);

		} else {
			mrManager.NVRLoadedInInspector = false;
			DestroyImmediate(GameObject.Find("NVRPlayer"));
		}

		//------------------------------------------------------------------------------------------------------------------------------------------

		DrawDefaultInspector();

	}

	Texture2D MakeTex(int width, int height, Color col)
	{
		Color[] pix = new Color[width * height];

		for (int i = 0; i < pix.Length; i++)
			pix[i] = col;

		TextureFormat tf = SystemInfo.SupportsTextureFormat(TextureFormat.RGBAFloat) ? TextureFormat.RGBAFloat : TextureFormat.RGBA32;
		Texture2D result = new Texture2D(width, height, tf, false);
		result.hideFlags = HideFlags.DontSave;
		result.SetPixels(pix);
		result.Apply();

		return result;
	}

	void DrawLabel(string s)
	{
		if (titleLabelStyle == null)
		{
			GUIStyle skurikenModuleTitleStyle = "ShurikenModuleTitle";
			titleLabelStyle = new GUIStyle(skurikenModuleTitleStyle);
			titleLabelStyle.contentOffset = new Vector2(5f, -2f);
			titleLabelStyle.normal.textColor = titleColor;
			titleLabelStyle.fixedHeight = 22;
			titleLabelStyle.fontStyle = FontStyle.Bold;
		}

		GUILayout.Label(s, titleLabelStyle);
	}
}
