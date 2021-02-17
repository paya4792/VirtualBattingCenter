using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmScript : MonoBehaviour
{

    public GameObject arm;
    float x,time,time_after,timeForRotate = 0.0f;
    public int rotateSpeed = -720;
    public float rotateTime = 1.0f;
    bool isRotated = false;

    public void SetArmRotate(float pitch_interval)
    {
        time = 0.0f;
        timeForRotate = pitch_interval - rotateTime / 2.0f;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        time_after += Time.deltaTime;

        if (time >= timeForRotate)
        {
            isRotated = true;
        }
        if (isRotated)
        {
            x += Time.deltaTime * rotateSpeed;
            arm.transform.rotation = Quaternion.Euler(x, 0, 0);
        }
        if (time_after >= timeForRotate + rotateTime)
        {
            isRotated = false;
            time_after = time;
            x = 0.0f;
            arm.transform.rotation = Quaternion.identity;
        }

    }
}
