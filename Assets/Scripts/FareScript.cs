using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FareScript : MonoBehaviour
{
    private GameObject ball;
    BallScript ballScript;

    private GameObject homeruns;
    private HRCount hrcount;

    void Start()
    {
        homeruns = GameObject.FindGameObjectWithTag("Homerun");
        hrcount = homeruns.GetComponent<HRCount>();
    }

    public void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == "Ball")
        {
            ball = col.gameObject;
            ballScript = ball.GetComponent<BallScript>();
            ballScript.OnFareZoneEnter();
            hrcount.HIT();
        }
    }
}
