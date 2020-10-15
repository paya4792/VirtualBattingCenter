using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallScript : MonoBehaviour
{
    private GameObject pitcher;
    ThrowBall throwBall;
    private GameObject camcon;
    SwitchCamera switchCamera;
    private GameObject chaseCamera;
    ChaseCam chaseCam;
    private bool collidedBat = false;//バットに当たったか
    private float timecount = 0.0f;//バットに当たってからの時間

    private Rigidbody rig;

    [System.NonSerialized]
    public float dx;//横変化量

    [System.NonSerialized]
    public float dy;//縦変化量

    [System.NonSerialized]
    public float sharpness;//変化球のキレ

    private Vector3 velocity;//変化球

    private float timer = 0.0f;

    private float arrival = 0.0f;

    private SphereCollider col;

    private DrawBallPos drawBallPos;

    void Start()
    {
        pitcher = GameObject.FindGameObjectWithTag("GameController");
        throwBall = pitcher.GetComponent<ThrowBall>();
        camcon = GameObject.FindGameObjectWithTag("CamControll");
        switchCamera = camcon.GetComponent<SwitchCamera>();
        chaseCamera = GameObject.FindGameObjectWithTag("ChaseCam");
        chaseCam = chaseCamera.GetComponent<ChaseCam>();
        switchCamera.SwitchCam(1);
        rig = this.GetComponent<Rigidbody>();
        col = this.GetComponent<SphereCollider>();
        drawBallPos = this.GetComponent<DrawBallPos>();
    }

    void Update()
    {
        timer += Time.deltaTime * 1.0f;
        if (!collidedBat)
        {
            col.material.bounciness = Mathf.Clamp01(1.0f + (rig.velocity.z * 0.02f));
        }
        else
        {
            drawBallPos.DrawPass(this.rig.velocity, this.transform.position);
        }

        if (timecount >= 20.0f)
        {
            timecount = 0.0f;
            Suiside();
        }
    }

    private void FixedUpdate()
    {
        if (!collidedBat)
        {
            rig.AddForce(velocity * 1.0f * Mathf.Clamp((arrival / timer), 0.0f , 1.0f));
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bat" && !collidedBat)
        {
            throwBall.OnBallHitted();
            switchCamera.SwitchCam(2);
            chaseCam.SetChaseBall();
            collidedBat = true;
            col.material.bounciness = 0.42f;
            TrailRenderer tr = this.GetComponent<TrailRenderer>();
            tr.time = 1.0f;
            tr.endColor = new Color(0.5f,0.0f,0.0f,0.5f);
        }
        if (collision.gameObject.tag == "Catcher")
        {
            rig.velocity = Vector3.zero;
            rig.useGravity = false;
            rig.isKinematic = true;
            this.transform.position = collision.transform.position;
            throwBall.OnBallCatched();
        }
        if (collision.gameObject.tag != "Bat" && collision.gameObject.tag != "Untagged" && collision.gameObject.tag != "")
        {
            throwBall.OnBallGrounded();
        }
    }

    public void SetVelocity(float arv)
    {
        arrival = arv;
        velocity.x = dx;
        velocity.y = dy;
    }

    public void OnFoulZoneEnter()
    {
        this.gameObject.tag = "Baseball";
        throwBall.OnBallFouled();
    }

    public void OnFareZoneEnter()
    {
        this.gameObject.tag = "Baseball";
        throwBall.OnBallFared();
    }

    public void Suiside()
    {
        switchCamera.SwitchCam(1);
        throwBall.run = true;
        drawBallPos.OnSuiside();
        GameObject gameObject = this.gameObject;
        Destroy(gameObject);
    }

    private void MoveBall()
    {
        
    }

}