using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEPlayer : MonoBehaviour
{
    int num;

    public SoundStruct[] Sounds;

    // ぶつかった対象タグとそれに対するサウンドの構造体
    [System.Serializable]
    public struct SoundStruct
    {
        public string collider_tag;
        public AudioClip[] sound;
    }

    void OnCollisionEnter(Collision collision)
    {
        for (int i = 0; i < Sounds.Length; i++)
        {
            if (collision.gameObject.tag == Sounds[num].collider_tag)
            {
                num = Random.Range(0,Sounds[i].sound.Length);
                AudioSource.PlayClipAtPoint(Sounds[i].sound[num], transform.position);
                break;
            }

            if (Sounds[num].collider_tag == "any")
            {
                num = Random.Range(0, Sounds[i].sound.Length);
                AudioSource.PlayClipAtPoint(Sounds[i].sound[num], transform.position);
            }
        }
    }
}
