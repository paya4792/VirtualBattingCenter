using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeRunScript : MonoBehaviour
{
    private GameObject homeruns;
    private HRCount hrcount;
    private GameObject ball;
    BallScript ballScript;

    // Start is called before the first frame update
    void Start()
    {
        homeruns = GameObject.FindGameObjectWithTag("Homerun");
        hrcount = homeruns.GetComponent<HRCount>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Ball")
        {
            ball = other.gameObject;
            ballScript = ball.GetComponent<BallScript>();
            ballScript.OnFareZoneEnter();
            hrcount.ITSGONE();
        }
    }
}
