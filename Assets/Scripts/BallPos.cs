using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallPos : MonoBehaviour
{
    public GameObject ball;
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Ball")
        {
            Vector3 hitPos = other.ClosestPointOnBounds(this.transform.position);
            ball.transform.position = hitPos;
        }
    }
}
