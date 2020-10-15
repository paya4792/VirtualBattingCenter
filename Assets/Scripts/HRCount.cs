using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HRCount : MonoBehaviour
{
    public Text HRText;
    public int homerun;
    public int hit;
    public AudioSource audioSource;

    void Start()
    {
        HRText.text = "HR" + homerun + "\n" + "Hit" + hit ;
    }

    private void Update()
    {

    }

    // Update is called once per frame
    public void ITSGONE()
    { 
        homerun++;
        HRText.text = "HR" + homerun + "\n" + "Hit" + hit;
        audioSource.Play();
    }

    public void HIT()
    {
        hit++;
        HRText.text = "HR" + homerun + "\n" + "Hit" + hit;
    }
}
