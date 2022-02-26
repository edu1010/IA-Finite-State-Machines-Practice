    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SHARK_Blackboard : MonoBehaviour
{
    [Header("Movement")]
    public float currentStamina = 5.0f;
    public float maxStamina = 5.0f;
    public bool newArrivePosition = false;
    public GameObject ArriveGameObject;
    public GameObject Attractor;

    [Header("Escape")]
    
    public float hideoutDetectionRadius = 100.0f;
    public float hideoutReachedRadius = 5.0f;
    public float harpoonDetectionRadius = 20.0f;
    public float hideTime = 5.0f;


    [Header("Eating")]
    public GameObject FishbowlGameObject;
    public float fishDetectionRadius = 20.0f;
    public float fishReachedRadius = 2.0f; //enough close to eat it
    public float fishBowlReachedRadius = 2.0f; 
    public int maxFishes = 5; 
    public int currentFishes = 0;
    public float maxTimeEatting = 3.0f;

}
