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
    public float hideoutDetectionRadius = 50.0f;
    public float hideoutReachedRadius = 5.0f;
    public float harpoonDetectionRadius = 70.0f;
    public float hideTime = 5.0f;

    [Header("Eating")]
    public GameObject FishbowlGameObject;
    public GameObject fishPickedPosition;
    public List<GameObject> fishPositionInFishbowl;

    public float fishReachedRadius = 25.0f; //enough close to pick it
    public float fishBowlReachedRadius = 2.0f; 
    public int maxFishes = 5; 
    public int currentFishes = 0;
    public float maxTimeEatting = 3.0f;


    [Header("General")]
    public bool changeToMovementState = false;
    public GameObject fishPicked;
    public GameObject harpoon;
}
