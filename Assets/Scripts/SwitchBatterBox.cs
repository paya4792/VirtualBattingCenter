using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class SwitchBatterBox : MonoBehaviour
{
  public GameObject player;
  public GameObject batterbox1;
  public GameObject batterbox2;
  private bool switchbatterbox = false;
  public SteamVR_Input_Sources hand;
  public SteamVR_Action_Boolean menubutton;

  private void Start()
  {
    player.transform.position = batterbox2.transform.position;
  }

  void Update()
  {
    if (menubutton.GetStateDown(hand))
    {
        ChangeBatterBox();
    }
  }

  private void ChangeBatterBox()
  {
    if (switchbatterbox)
    {
      player.transform.position = batterbox2.transform.position;
      switchbatterbox = false;
    }
    else if (!switchbatterbox)
    {
      player.transform.position = batterbox1.transform.position;
      switchbatterbox = true;
    }
    return;
  }
}