using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseCam : MonoBehaviour
{
    private GameObject ball;
    private Transform myTransform;

    public void SetChaseBall() {
        ball = GameObject.FindGameObjectWithTag("Ball");
        myTransform = ball.transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(myTransform);
    }
}
