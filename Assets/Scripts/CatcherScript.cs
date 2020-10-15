using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class CatcherScript : MonoBehaviour
{
    public float minRangex;
    public float maxRangex;
    public float minRangey;
    public float maxRangey;

    public bool isRandom = false;

    private bool pitched;
    private GameObject ball;
    public float mittMoveDistance = 2f;

    public SteamVR_Input_Sources HandType;
    public SteamVR_Action_Boolean action;

    public void SetPosition()
    {
        if (isRandom)
        {
            var x = Random.Range(minRangex, maxRangex);
            var y = Random.Range(minRangey, maxRangey);
            this.transform.position = new Vector3(x, y, this.transform.position.z);
        }
        else
        {
            var x = (minRangex + maxRangex) / 2;
            var y = (minRangey + maxRangey) / 2;
            this.transform.position = new Vector3(x, y, this.transform.position.z);
        }
    }

    public void OnPitch()
    {
        ball = GameObject.FindGameObjectWithTag("Ball");
        pitched = true;
    }

    void Update()
    {
        if (action.GetStateDown(HandType) && ball)
        {
            var b = ball.GetComponent<BallScript>();
            b.Suiside();
        }

        if (pitched)
        {
            float dis = this.transform.position.z + ball.transform.position.z;
            if (mittMoveDistance > dis)
            {
                this.transform.position = new Vector3(ball.transform.position.x, ball.transform.position.y, this.transform.position.z);
            }
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
            pitched = false;
    }

    public void OnHitted()
    {
        pitched = false;
    }
}
