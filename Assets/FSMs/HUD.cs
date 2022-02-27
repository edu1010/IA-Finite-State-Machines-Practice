using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public SHARK_Blackboard shark_Blackboard;
    public Slider dashSlider;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        dashSlider.value = shark_Blackboard.currentStamina / shark_Blackboard.maxStamina;
    }
}
