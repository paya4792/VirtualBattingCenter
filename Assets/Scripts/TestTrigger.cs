using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class TestTrigger : MonoBehaviour
{
    public SteamVR_Input_Sources hand;
    public SteamVR_Action_Boolean grabAction;

    void Update()
    {
        if (grabAction.GetState(hand))
        {
            Debug.Log("Grab!");
        }
    }
}