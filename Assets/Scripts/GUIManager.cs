using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIManager : MonoBehaviour
{

    public Slider slider;

    // Start is called before the first frame update
    void Start()
    {
        slider.value = 0.0f;
    }

    public void SetSliderValue(float max)
    {
        slider.maxValue = max;
        slider.value = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if ( slider.value < slider.maxValue)
        {
            slider.value += Time.deltaTime;
        }
    }
}
