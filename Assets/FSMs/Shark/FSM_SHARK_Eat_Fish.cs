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
        public enum State
        {
                INITIAL,
                DETECT_FISH,
                GO_TO_FISH,
                ARRIVE_AT_FISHBOWL,
                EAT_FISH,
        };
        public State currentState = State.INITIAL;

        private SHARK_Blackboard blackboard;
        private Arrive arrive;
        private float elapsedTime = 0.0f;
        
        void Start()
        {
            arrive = GetComponent<Arrive>();
            blackboard = GetComponent<SHARK_Blackboard>();

            arrive.enabled = false;
        }
        
        public override void Exit()
        {
            arrive.enabled = false;
            base.Exit();
        }
        public override void ReEnter()
        {
            currentState = State.INITIAL;
            blackboard.changeToMovementState = false;
            base.ReEnter();
        } 
        void Update()
        {
               
            switch (currentState)
            {
                case State.INITIAL:
                    ChangeState(State.ARRIVE_AT_FISHBOWL);
                    break;
                case State.GO_TO_FISH:
                    if (fish == null || fish.Equals(null))
                    {
                        // change movement?
                        break;
                    }
                    if (SensingUtils.DistanceToTarget(gameObject, fish) <= blackboard.fishReachedRadius)
                    {
                        blackboard.changeToMovementState = true;
                        break;
                    }
                    break;
                case State.ARRIVE_AT_FISHBOWL:
                    if (SensingUtils.DistanceToTarget(gameObject, fish) <= blackboard.fishBowlReachedRadius)
                    {
                        blackboard.currentFishes += 1;
                        if (blackboard.currentFishes >= blackboard.maxFishes)
                        {
                            ChangeState(State.EAT_FISH);
                        }
                        else
                        {
                            blackboard.changeToMovementState = true;
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
                        blackboard.changeToMovementState = true;
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
                case State.GO_TO_FISH:
                    arrive.enabled = true;
                    arrive.target = blackboard.FishbowlGameObject;
                    break;
                case State.ARRIVE_AT_FISHBOWL:
                    arrive.enabled = true;
                    arrive.target = blackboard.FishbowlGameObject;
                    blackboard.fishPicked.transform.parent = gameObject.transform;
                    break;
                case State.EAT_FISH:
                    blackboard.currentFishes = 0;
                    break;
            }
            
            // EXIT STATE LOGIC. Depends on current state
            switch (currentState)
            {
                case State.DETECT_FISH:
                    break;
                case State.GO_TO_FISH:
                    arrive.enabled = false;
                    break;
                case State.ARRIVE_AT_FISHBOWL:
                    arrive.enabled = false;
                    blackboard.fishPicked.transform.parent = null;
                    blackboard.fishPicked.tag = "FishEated";
                    break;
                case State.EAT_FISH:
                    break;
            }
            currentState = newState;
        }
    }
}