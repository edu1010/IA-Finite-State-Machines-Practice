using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steerings;

namespace FSM
{
    [RequireComponent(typeof(Arrive))]
    [RequireComponent(typeof(SHARK_Blackboard))]
    [RequireComponent(typeof(FSM_SHARK_Movement))]
    public class FSM_SHARK_Eat_Fish : FiniteStateMachine
    {
        public enum State
        {
                INITIAL,
                ARRIVE_AT_FISHBOWL,
                EAT_FISH_FROM_FISHBOWL,
                FSM_MOVEMENT
        };
        public State currentState = State.INITIAL;

        private SHARK_Blackboard blackboard;
        private Arrive arrive;
        private float elapsedTime = 0.0f;

        private FSM_SHARK_Movement fsm_movement;
        
        void Awake()
        {
            arrive = GetComponent<Arrive>();
            blackboard = GetComponent<SHARK_Blackboard>();
            fsm_movement = GetComponent<FSM_SHARK_Movement>();
            arrive.enabled = false;
        }
        
        public override void Exit()
        {
            elapsedTime = 0.0f;
            arrive.enabled = false;
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
                    ChangeState(State.ARRIVE_AT_FISHBOWL);
                    break;
                case State.ARRIVE_AT_FISHBOWL:
                    if (SensingUtils.DistanceToTarget(gameObject, blackboard.FishbowlGameObject) <= blackboard.fishBowlReachedRadius)
                    {
                        blackboard.currentFishes += 1;
                        if (blackboard.currentFishes >= blackboard.maxFishes)
                        {
                            ChangeState(State.EAT_FISH_FROM_FISHBOWL); break;
                        }
                        else
                        {
                            ChangeState(State.FSM_MOVEMENT); break;
                        }
                    }
                    break;
                case State.EAT_FISH_FROM_FISHBOWL:
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
                        ChangeState(State.FSM_MOVEMENT);
                        break;
                    }
                    break;
                case State.FSM_MOVEMENT:
                    blackboard.fishPicked = SensingUtils.FindInstanceWithinRadius(gameObject, "FISH", blackboard.fishReachedRadius);
                    if (blackboard.fishPicked != null)
                    {
                        ChangeState(State.INITIAL); break;
                    }
                    break;
            }
        }
        private void ChangeState(State newState)
            {
            // ENTER STATE LOGIC. Depends on newState
            switch (newState)
            {
                case State.ARRIVE_AT_FISHBOWL:
                    arrive.enabled = true;
                    arrive.target = blackboard.FishbowlGameObject;
                    blackboard.fishPicked.GetComponent<FSM_FISH>().Exit();
                    blackboard.fishPicked.transform.position = blackboard.fishPickedPosition.transform.position;
                    blackboard.fishPicked.transform.parent = transform;
                    break;
                case State.EAT_FISH_FROM_FISHBOWL:
                    break;
                case State.FSM_MOVEMENT:
                    fsm_movement.ReEnter();
                    break;
            }
            
            // EXIT STATE LOGIC. Depends on current state
            switch (currentState)
            {
                case State.ARRIVE_AT_FISHBOWL:
                    arrive.enabled = false;
                    blackboard.fishPicked.transform.position = blackboard.fishPositionInFishbowl[blackboard.currentFishes - 1].transform.position;
                    blackboard.fishPicked.transform.parent = null;
                    blackboard.fishPicked.tag = "FishEated";
                    break;
                case State.EAT_FISH_FROM_FISHBOWL: 
                    blackboard.totalEatenFishes += blackboard.currentFishes;
                    blackboard.currentFishes = 0;
                    elapsedTime = 0.0f;
                    break;
                case State.FSM_MOVEMENT:
                    fsm_movement.Exit();
                    break;
            }
            currentState = newState;
        }
    }
}