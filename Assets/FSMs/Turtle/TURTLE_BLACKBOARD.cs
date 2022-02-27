using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TURTLE_BLACKBOARD : MonoBehaviour
{
    [Header("Wander")]
    public float maxTimeWandering = 15.0f;
    public float minTimeWandering = 6.0f;
    public float maxTimeHavingFun = 5.0f;
    public float maxTimeSayingIt = 3.0f;
    public GameObject Attractor;
    public GameObject textTurtle;

    [Header("Breathe")]
    public GameObject surface;
    public float maxOxigen = 100.0f;    
    public float surfaceReachedRadius = 1.0f;

    private void Start()
    {
        textTurtle = GameObject.Find("TextTurtle");
        textTurtle.SetActive(false);
    }

    public void SayIt(bool on)
    {
        textTurtle.SetActive(on);
    }
}
