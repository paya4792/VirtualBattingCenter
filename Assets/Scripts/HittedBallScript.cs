using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HittedBallScript : MonoBehaviour
{
    private GameObject pitcher;
    ThrowBall throwBall;
    private GameObject camcon;
    SwitchCamera switchCamera;
    private GameObject chaseCamera;
    ChaseCam chaseCam;
    private bool collidedBat = false;
    private float timecount = 0.0f;

    void Start()
    {
        pitcher = GameObject.FindGameObjectWithTag("GameController");
        throwBall = pitcher.GetComponent<ThrowBall>();
        camcon = GameObject.FindGameObjectWithTag("CamControll");
        switchCamera = camcon.GetComponent<SwitchCamera>();
        chaseCamera = GameObject.FindGameObjectWithTag("ChaseCam");
        chaseCam = chaseCamera.GetComponent<ChaseCam>();
    }

    void Update()
    {
        if (collidedBat)
        {
            timecount += Time.deltaTime;
        }
        if (timecount >= 20.0f)
        {
            timecount = 0.0f;
            Suiside();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bat")
        {
            throwBall.OnBallHitted();
            switchCamera.SwitchCam(2);
            chaseCam.SetChaseBall();
            collidedBat = true;
        }
        if (collision.gameObject.tag != "Bat" && collision.gameObject.tag != "Untagged")
        {
            this.gameObject.tag = "Baseball";
            throwBall.OnBallGrounded();
            Invoke("Suiside", 3.0f);
        }
    }

    public void OnFoulZoneEnter()
    {
        this.gameObject.tag = "Baseball";
        throwBall.OnBallGrounded();
        Invoke("Suiside", 3.0f);
    }

    private void Suiside()
    {
        switchCamera.SwitchCam(1);
        throwBall.run = true;
        GameObject gameObject = this.gameObject;
        Destroy(gameObject);
    }

}
