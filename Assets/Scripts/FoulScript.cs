using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoulScript : MonoBehaviour
{
    private GameObject ball;
    BallScript ballScript;

    public void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Ball")
        {
            ball = col.gameObject;
            ballScript = ball.GetComponent<BallScript>();
            ballScript.OnFoulZoneEnter();
        }
    }
}
