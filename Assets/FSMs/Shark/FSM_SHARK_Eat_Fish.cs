using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steerings;

namespace FSM
{
    [RequireComponent(typeof(Arrive))]
    [RequireComponent(typeof(SHARK_Blackboard))]
    public class FSM_SHARK_Eat_Fish : FiniteStateMachine
    {
        private GameObject fish;
        private FSM_SHARK_Movement sharkFsmMovement;
        public enum State
        {
                INITIAL,
                DETECT_FISH,
                GOTO_FISH,
                ARRIVE_AT_FISHBOWL,
                EAT_FISH,
                CHANGE_MOVEMENT
        };
        public State currentState = State.INITIAL;

        private SHARK_Blackboard blackboard;
        private Arrive arrive;
        private float elapsedTime = 0.0f;
        
        void Start()
        {
            arrive = GetComponent<Arrive>();
            blackboard = GetComponent<SHARK_Blackboard>();
            sharkFsmMovement = GetComponent<FSM_SHARK_Movement>();

            arrive.enabled = false;
        }
        
        public override void Exit()
        {
            arrive.enabled = false;
            sharkFsmMovement.enabled = false;
            base.Exit();
        }
        public override void ReEnter()
        {
            currentState = State.INITIAL;
            base.ReEnter();
        } 
        void Update()
        {
               
            switch (currentState)
            {
                case State.INITIAL:
                    ChangeState(State.DETECT_FISH);
                    break;
                case State.DETECT_FISH:
                    fish = SensingUtils.FindInstanceWithinRadius(gameObject, "FISH", blackboard.fishDetectionRadius);
                    if (fish != null)
                    {
                        Debug.Log("fish detected");
                        ChangeState(State.GOTO_FISH);
                    }
                    break;
                case State.GOTO_FISH:
                    Debug.Log("go to fish");
                    if (fish == null || fish.Equals(null))
                    {
                        ChangeState(State.CHANGE_MOVEMENT);
                        break;
                    }
                    if ((SensingUtils.DistanceToTarget(gameObject, fish) <= blackboard.fishReachedRadius))
                    {
                        ChangeState(State.ARRIVE_AT_FISHBOWL);
                        break;
                    }
                    break;
                case State.ARRIVE_AT_FISHBOWL:
                    if ((SensingUtils.DistanceToTarget(gameObject, fish) <= blackboard.fishBowlReachedRadius))
                    {
                        blackboard.currentFishes += 1;
                        if (blackboard.currentFishes >= blackboard.maxFishes)
                        {
                            ChangeState(State.EAT_FISH);
                        }
                        else
                        {
                            ChangeState(State.CHANGE_MOVEMENT);
                        }
                        break;
                    }
                    break;
                case State.EAT_FISH:
                    Debug.Log("eat fish");
                    elapsedTime += Time.deltaTime;
                    GameObject[] fishesEated = GameObject.FindGameObjectsWithTag("FishEated");
                    if (elapsedTime >= blackboard.maxTimeEatting)
                    {
                        //fishes tag "FishEated" delete
                        foreach (GameObject target in fishesEated)
                        {
                            GameObject.Destroy(target);
                        }
                        blackboard.currentFishes = 0;
                        ChangeState(State.CHANGE_MOVEMENT);
                        break;
                    }
                    break;

            }
        }
        private void ChangeState(State newState)
            {
            // ENTER STATE LOGIC. Depends on newState
            switch (newState)
            {
                case State.DETECT_FISH:
                    break;
                case State.GOTO_FISH:
                    arrive.enabled = true;
                    arrive.target = blackboard.FishbowlGameObject;
                    break;
                case State.ARRIVE_AT_FISHBOWL:
                    arrive.enabled = true;
                    arrive.target = blackboard.FishbowlGameObject;
                    fish.transform.parent = gameObject.transform;
                    break;
                case State.EAT_FISH:
                    blackboard.currentFishes = 0;
                    break;
                case State.CHANGE_MOVEMENT:
                    Debug.Log("change FSM movement");
                    sharkFsmMovement.ReEnter();
                    break;

            }
            
            // EXIT STATE LOGIC. Depends on current state
            switch (currentState)
            {
                case State.DETECT_FISH:
                    break;
                case State.GOTO_FISH:
                    arrive.enabled = false;
                    break;
                case State.ARRIVE_AT_FISHBOWL:
                    arrive.enabled = false;
                    fish.transform.parent = null;
                    fish.tag = "FishEated";
                    break;
                case State.EAT_FISH:
                    break;
                case State.CHANGE_MOVEMENT:
                    sharkFsmMovement.Exit();
                    break;

            }
            currentState = newState;
        }
    }
}