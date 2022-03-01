using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SUBMARINE_MISSILE_Blackboard : MonoBehaviour
{
    [Header("Attractors")]
    public GameObject attractorA;
    public GameObject attractorB;
    public GameObject shark;
    public GameObject submarine;

    [Header("Missile")]
    public float sharkDetectableRadious = 10.0f;
    public float timeAttack = 5.0f;
    public float timeToAttackAgain = 10.0f;
    public float submarineCloseEnoughtRadius = 10.0f;


    [Header("Submarine")]
    public float attractorReachedRadious = 10.0f;
    public float maxTimeStay = 4.0f;

    //public bool attack = false;
    public bool missileHided = false;
    public bool canAttack;

}
