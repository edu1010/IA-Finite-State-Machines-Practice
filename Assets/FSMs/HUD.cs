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
        shark_Blackboard.timeToWin = shark_Blackboard.timeToWin - Time.deltaTime;
        if (shark_Blackboard.timeToWin <= 0)
        {
            FindObjectOfType<GameManager>().LoseGame();
        }

        gameTime.text = (shark_Blackboard.timeToWin.ToString());
        textEatenFishes.text = "Eaten fishes: " + shark_Blackboard.totalEatenFishes;
        textCapturedFishes.text = "Captured fishes: " + shark_Blackboard.currentFishes + "/5";
        dashSlider.value = shark_Blackboard.currentStamina / shark_Blackboard.maxStamina;
    }
}