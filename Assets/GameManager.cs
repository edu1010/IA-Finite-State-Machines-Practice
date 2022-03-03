using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject winGameUI;
    public GameObject loseGameUI;
    public void EndGame()
    {
        Debug.Log("GAME END");
    }

    public void WinGame()
    {
        winGameUI.SetActive(true);
    }
    public void LoseGame()
    {
        loseGameUI.SetActive(true);
    }

}
