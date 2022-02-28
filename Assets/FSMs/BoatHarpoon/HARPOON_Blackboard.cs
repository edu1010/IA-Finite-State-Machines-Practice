using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HARPOON_Blackboard : MonoBehaviour
{
    [Header("Attractors")]
    public GameObject attractorA;
    public GameObject attractorB;
    public GameObject shark;
    public GameObject boat;

    [Header("Harpoon")]
    public float sharkDetectableRadious = 10.0f;
    public float timeAttack = 5.0f;
    //public float pickHarpoonTime = 8.0f;
    public float timeToAttackAgain = 10.0f;
    public float boatCloseEnoughtRadius = 10.0f;


    [Header("Boat")]
    public float attractorReachedRadious = 10.0f;
    public float maxTimeStay = 4.0f;

    public bool attack = false;
    public bool harpoonPicked = false;


}

