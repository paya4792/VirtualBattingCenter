using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrikeScript : MonoBehaviour
{
    public bool isSwinged;
    public bool isBatThroughed;
    public bool isBallThroughed;
    public bool isPitched;

    private BSOScript bso;

    public AudioClip sound;

    private void Start()
    {
        ResetBool();
        bso = GetComponent<BSOScript>();
    }

    public void ResetBool()
    {
        isSwinged = false;
        isBallThroughed = false;
        isPitched = false;
        isBatThroughed = false;
    }

    private void OnTriggerEnter (Collider collision)
    {
        if (isPitched)
        {
            if (collision.gameObject.tag == "Ball")
            {
                isBallThroughed = true;
            }
            if (collision.gameObject.tag == "Bat")
            {
                isBatThroughed = true;
            }
        }
    }

    public void OnBallCatched()
    {
        if (isBallThroughed || (isSwinged && isBatThroughed))
        {
            AudioSource.PlayClipAtPoint(sound, transform.position);
            bso.AddBSO('S');
        }
        else { bso.AddBSO('B'); }
    }
}
