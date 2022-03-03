using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUD : MonoBehaviour
{
    public SHARK_Blackboard shark_Blackboard;
    public Slider dashSlider;

    public TextMeshProUGUI textEatenFishes;
    public TextMeshProUGUI textCapturedFishes;
    public TextMeshProUGUI gameTime;
    
    void Start()
    {

    }

    void Update()
    {
        if (shark_Blackboard.totalEatenFishes >= 15)
        {
            FindObjectOfType<GameManager>().WinGame();
        }

        gameTime.text = ( "FISHES: " + shark_Blackboard.totalEatenFishes.ToString() + " / 15");
        textEatenFishes.text = "Eaten fishes: " + shark_Blackboard.totalEatenFishes;
        textCapturedFishes.text = "Captured fishes: " + shark_Blackboard.currentFishes + "/5";
        dashSlider.value = shark_Blackboard.currentStamina / shark_Blackboard.maxStamina;
    }
}