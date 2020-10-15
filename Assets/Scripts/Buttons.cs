using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class Buttons : MonoBehaviour
{
    public string buttonrole;
    private GameObject pitcher;
    ThrowBall throwBall;
    public SteamVR_Input_Sources HandType;
    public SteamVR_Action_Boolean action;
    public Material onDefault;
    public Material onSelect;
    public AudioClip audioFocus;
    public AudioClip audioSelect;
    private Renderer _Renderer;

    private void Start()
    {
        pitcher = GameObject.FindGameObjectWithTag("GameController");
        throwBall = pitcher.GetComponent<ThrowBall>();
        throwBall.OnPutMenuButton("stop");
        _Renderer = GetComponent<Renderer>();
        _Renderer.material = onDefault;
    }

    private void OnCollisionEnter(Collision collision)
    {
        _Renderer.material = onSelect;
        AudioSource.PlayClipAtPoint(audioFocus, transform.position);
    }

    private void OnCollisionStay(Collision collision)
    {
        if (action.GetStateDown(HandType))
        {
            throwBall.OnPutMenuButton(buttonrole);
            AudioSource.PlayClipAtPoint(audioSelect, transform.position);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        _Renderer.material = onDefault;
    }
}
