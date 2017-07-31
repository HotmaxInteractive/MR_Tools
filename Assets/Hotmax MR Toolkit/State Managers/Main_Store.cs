using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main_Store : MonoBehaviour {

    //Actor Monitor is On
    public static bool menuIsOpen = true;
    public delegate void menuIsOpenHandler(bool value);
    public static event menuIsOpenHandler menuIsOpenEvent;


    void Start () {}
	void Update () {}


    // -- mutators
    //
    public void SET_MENU_IS_OPEN(bool value)
    {
        menuIsOpen = value;
        menuIsOpenEvent(value);
    }
}
