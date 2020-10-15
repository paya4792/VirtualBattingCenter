using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;


public class ControllerExample : MonoBehaviour
{
    public SteamVR_Input_Sources HandType;
    public SteamVR_Action_Boolean GrabAction;
    public Text text;

    void Update()
    {
        if (GrabAction.GetState(HandType))
        {
            text.text=(HandType.ToString() + GrabAction.ToString());
        }
    }
}