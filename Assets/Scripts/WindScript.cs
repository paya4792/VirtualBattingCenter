using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindScript: MonoBehaviour
{
    public float coefficient;   // 空気抵抗係数
    public Vector3 velocity;    // 風速
    private Rigidbody rig;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Rigidbody>() == null)
        {
            return;
        }

        rig = other.gameObject.GetComponent<Rigidbody>();
    }

    void OnTriggerStay(Collider col)
    {
        // 相対速度計算
        var relativeVelocity = velocity - rig.velocity;

        // 空気抵抗を与える
        rig.AddForce(coefficient * relativeVelocity);
    }
}