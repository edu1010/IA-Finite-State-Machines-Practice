using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SHARK_Blackboard : MonoBehaviour
{
    [Header("Movement")]
    public float currentStamina = 5.0f;
    public float maxStamina = 5.0f;
    public bool canDash = false;
    public GameObject ArriveGameObject;
    public GameObject Attractor;

    [Header("Escape")]
    public float maxTimeTryingToHide = 5f;
    public float hideoutDetectionRadius = 50.0f;
    public float hideoutReachedRadius = 5.0f;
    public float missileDetectionRadius = 70.0f;
    public float hideTime = 5.0f;
    public bool IsHided;

    [Header("Eating")]
    public GameObject FishbowlGameObject;
    public GameObject fishPickedPosition; //position where the fish will be while the shark is transporting it
    public List<GameObject> fishPositionInFishbowl; //the 5 positions where the 5 fish will be placed in the fishbowl

    public float fishReachedRadius = 25.0f; //close enough to pick the fish
    public float fishBowlReachedRadius = 25.0f; //close enough to reach the fishbowl
    public int maxFishes = 5; //max number of fishes in the fishbowl
    public int currentFishes = 0;
    public float maxTimeEatting = 3.0f;
    public int totalEatenFishes = 0;

    [Header("General")]
    public bool changeToMovementState = false;
    public GameObject fishPicked;
    public GameObject missile;

   
}
