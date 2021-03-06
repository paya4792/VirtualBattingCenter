﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BatScript : MonoBehaviour
{
    public float power = 36.1f;
    private Vector3 vec = Vector3.zero;
    private Rigidbody rig;
    public Vector3 latestPos;
    public Vector3 speed;
    public GameObject particle;

    private GameObject strikezone;
    private StrikeScript strike;
    private CapsuleCollider capsuleCollider;
    private PhysicMaterial physicMaterial;

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Ball")
        {
            rig = other.gameObject.GetComponent<Rigidbody>();

            vec.z = Mathf.Clamp(power * rig.mass * speed.z, -5, 5);
            physicMaterial.bounciness = Mathf.Clamp(vec.z * 0.2f, 0, 1);

            if (vec.z > 1) { rig.AddForce(vec, ForceMode.Impulse); }
            else { rig.AddForce(new Vector3(0, 0, -power), ForceMode.VelocityChange); }
            if (vec.z >= 4) Instantiate(particle, transform.position, transform.rotation);
            //   Debug.Log(vec.z);
            //   Debug.Log(speed.z);
            Debug.Log(physicMaterial.bounciness);
        }
    }

    private void Start()
    {
        capsuleCollider = this.GetComponent<CapsuleCollider>();
        physicMaterial = capsuleCollider.material;
        //strikezone = GameObject.Find("StrikeZone");
        //strike = strikezone.GetComponent<StrikeScript>();
    }

    private void Update()
    {
        speed = ((this.transform.position - latestPos) / Time.deltaTime);
        latestPos = this.transform.position;
        //if (speed.z > 3.0f) { strike.isSwinged = true; Debug.Log(speed.z); }
    }
}
