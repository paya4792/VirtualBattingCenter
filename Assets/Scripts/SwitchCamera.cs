using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchCamera : MonoBehaviour
{

    public Camera camera1;
    public Camera camera2;
    void Start()
    {
        camera2.enabled = false;
    }

    public void SwitchCam(int cam)
    {
        if (cam == 1)
        {
            camera1.enabled = true;
            camera2.enabled = false;
        }
        else if (cam == 2)
        {
            Invoke("ChangeCam", 1.0f);
        }
    }

    private void ChangeCam()
    {
        camera1.enabled = false;
        camera2.enabled = true;
    }
}
