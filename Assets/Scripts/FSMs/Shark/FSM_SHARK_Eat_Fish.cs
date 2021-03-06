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
        public enum State
        {
                INITIAL,
                ARRIVE_AT_FISHBOWL,
                EAT_FISH_FROM_FISHBOWL,
        };
        public State currentState = State.INITIAL;

        private SHARK_Blackboard blackboard;
        private Arrive arrive;
        private float elapsedTime = 0.0f;

        private float initialAcceleration;
        private float initialSpeed;

        void Awake()
        {
            arrive = GetComponent<Arrive>();
            blackboard = GetComponent<SHARK_Blackboard>();
            arrive.enabled = false;

            initialAcceleration = GetComponent<KinematicState>().maxAcceleration;
            initialSpeed = GetComponent<KinematicState>().maxSpeed;
        }
        
        public override void Exit()
        {
            GetComponent<KinematicState>().maxAcceleration = initialAcceleration;
            GetComponent<KinematicState>().maxSpeed = initialSpeed;
            elapsedTime = 0.0f;
            arrive.enabled = false;
            base.Exit();
        }
        public override void ReEnter()
        {
            GetComponent<KinematicState>().maxAcceleration = initialAcceleration;
            GetComponent<KinematicState>().maxSpeed = initialSpeed;
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
                            //exit       
                            blackboard.fishPicked.transform.position = blackboard.fishPositionInFishbowl[blackboard.currentFishes - 1].transform.position;
                            blackboard.fishPicked.transform.parent = null;
                            blackboard.fishPicked.tag = "FishEated";
                            arrive.enabled = false;
                            //Change to Wander in FSM SHARK Movement
                            blackboard.changeToMovementState = true;
                        }
                    }
                    break;
                case State.EAT_FISH_FROM_FISHBOWL:
                    elapsedTime += Time.deltaTime;
                    GameObject[] fishesEated = GameObject.FindGameObjectsWithTag("FishEated");
                    if (elapsedTime >= blackboard.maxTimeEatting)
                    {
                        //fishes tag "FishEated" delete
                        foreach (GameObject target in fishesEated)
                        {
                            GameObject.Destroy(target);
                        }
                        blackboard.totalEatenFishes += blackboard.currentFishes;
                        blackboard.currentFishes = 0;
                        blackboard.changeToMovementState = true; //Change to Wander in FSM SHARK Movement
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
                case State.ARRIVE_AT_FISHBOWL:
                    GetComponent<KinematicState>().maxAcceleration *= 2;
                    blackboard.fishPicked.GetComponent<FSM_FISH>().Exit();
                    blackboard.fishPicked.transform.position = blackboard.fishPickedPosition.transform.position;
                    blackboard.fishPicked.transform.parent = transform;
                    arrive.enabled = true;
                    arrive.target = blackboard.FishbowlGameObject;
                    break;
                case State.EAT_FISH_FROM_FISHBOWL:
                    elapsedTime = 0.0f;
                    break;
            }
            
            // EXIT STATE LOGIC. Depends on current state
            switch (currentState)
            {
                case State.ARRIVE_AT_FISHBOWL:
                    GetComponent<KinematicState>().maxAcceleration /= 2;
                    arrive.enabled = false;
                    blackboard.fishPicked.transform.position = blackboard.fishPositionInFishbowl[blackboard.currentFishes - 1].transform.position;
                    blackboard.fishPicked.transform.parent = null;
                    blackboard.fishPicked.tag = "FishEated";
                    break;
                case State.EAT_FISH_FROM_FISHBOWL: 
                    break;
            }
            currentState = newState;
        }
    }
}