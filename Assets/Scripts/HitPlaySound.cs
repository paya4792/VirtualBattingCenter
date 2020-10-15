using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitPlaySound : MonoBehaviour
{

    public AudioClip[] sound;
    int num;

    void OnCollisionEnter(Collision collision)
    {
        if ((collision.gameObject.tag == "Ball" && this.gameObject.tag == "Bat")|| (this.gameObject.tag != "Bat"))
        {
            num = Random.Range(0, sound.Length);
            AudioSource.PlayClipAtPoint(sound[num], transform.position);
        }
    }
}