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
    public bool isAlwaysResetPosition = false;

    public void SetPosition()
    {
        if (isRandom)
        {
            var x = Random.Range(minRangex, maxRangex);
            var y = Random.Range(minRangey, maxRangey);
            this.transform.position = new Vector3(x, y, this.transform.position.z);
        }
        else if(isAlwaysResetPosition)
        {
            var x = (minRangex + maxRangex) / 2;
            var y = (minRangey + maxRangey) / 2;
            this.transform.position = new Vector3(x, y, this.transform.position.z);
        }
    }

    void Update()
    {

    }

}
