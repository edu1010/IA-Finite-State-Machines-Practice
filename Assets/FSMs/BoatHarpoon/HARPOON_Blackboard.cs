using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HARPOON_Blackboard : MonoBehaviour
{
    [Header("Attractors")]
    public GameObject attractorA;
    public GameObject attractorB;
    public GameObject shark;

    [Header("Harpoon")]
    public float sharkDetectableRadious = 10.0f;
    public float timeAttack = 5.0f;

    [Header("Boat")]
    public float attractorReachedRadious = 10.0f;

    public bool attack = false;


}

