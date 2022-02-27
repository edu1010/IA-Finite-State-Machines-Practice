using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TURTLE_Blackboard : MonoBehaviour
{
    [Header("Wander")]
    public float timeout = 5.0f;

    [Header("Breathe")]
    public GameObject surface;
    public float maxOxigen = 100.0f;    
    public float surfaceReachedRadius = 1.0f;
}
