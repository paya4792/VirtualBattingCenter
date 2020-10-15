using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class PlayerMove : MonoBehaviour
{
  public SteamVR_Input_Sources HandType;
  public SteamVR_Action_Boolean TouchPadClick;
  public SteamVR_Action_Vector2 TouchPad;
  public float speed = 2;


  private void Start()
  {
  }

  private void Update()
  {
    if (TouchPadClick.GetStateDown(HandType))
    {
      if (TouchPad.axis.y >= 0.2f) { transform.Translate(0, 0, speed * TouchPad.axis.y); }
      if (TouchPad.axis.y <= -0.2f) { transform.Translate(0, 0, speed * TouchPad.axis.y); }
      if (TouchPad.axis.x >= 0.2f) { transform.Translate(speed * TouchPad.axis.x, 0, 0); }
      if (TouchPad.axis.x <= -0.2f) { transform.Translate(speed * TouchPad.axis.x, 0, 0); }
    }
  }
}