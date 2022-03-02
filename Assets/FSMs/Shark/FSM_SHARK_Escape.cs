using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steerings;

namespace FSM
{
    [RequireComponent(typeof(SHARK_Blackboard))]
    [RequireComponent(typeof(Arrive))]
    [RequireComponent(typeof(FSM_SHARK_Eat_Fish))]

    public class FSM_SHARK_Escape : FiniteStateMachine
    {
        
        public enum State {INITIAL, SEARCH_HIDEOUT, REACHING_HIDEOUT, WAIT_IN, FSM_EAT_FISH};
             
        public State currentState = State.INITIAL;

        private Arrive arrive;
        private SHARK_Blackboard blackboard;
        private FSM_SHARK_Eat_Fish fsm_eatFish;

        private GameObject hideout;
        private float elapsedTime = 0.0f;

        void Awake()
        {            
            arrive = GetComponent<Arrive>();
            blackboard = GetComponent<SHARK_Blackboard>();
            fsm_eatFish = GetComponent<FSM_SHARK_Eat_Fish>();

            fsm_eatFish.enabled = false;
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
            base.ReEnter();
        }

        void Update()
        {
            switch (currentState)
            {
                case State.INITIAL:
                    ChangeState(State.SEARCH_HIDEOUT);
                    break;
                case State.SEARCH_HIDEOUT:
                    hideout = SensingUtils.FindInstanceWithinRadius(gameObject, "HIDEOUT", blackboard.hideoutDetectionRadius);
                    if (hideout != null)
                    {
                        ChangeState(State.REACHING_HIDEOUT);
                        Debug.Log("Target encontrado");
                        break;
                    }
                    break;
                case State.REACHING_HIDEOUT:
                    if (SensingUtils.DistanceToTarget(gameObject, hideout) <= blackboard.hideoutReachedRadius)
                    {
                        ChangeState(State.WAIT_IN);
                        break;
                    }
                    break;
                case State.WAIT_IN:
                    if (elapsedTime >= blackboard.hideTime)
                    {
                        //Change to the second level of the FSM: EAT_FISH
                        ChangeState(State.FSM_EAT_FISH);
                        break;
                    }
                    elapsedTime += Time.deltaTime;                    
                    break;
                case State.FSM_EAT_FISH:
                    if (SensingUtils.DistanceToTarget(gameObject, blackboard.missile) < blackboard.missileDetectionRadius)
                    {
                        ChangeState(State.SEARCH_HIDEOUT); break;
                    }
                    break;
            }
        }

        private void ChangeState(State newState)
        {
            // EXIT STATE LOGIC. Depends on current state
            switch (currentState)
            {
                case State.SEARCH_HIDEOUT:                    
                    break;
                case State.REACHING_HIDEOUT:
                    GetComponent<KinematicState>().maxAcceleration /= 7;
                    GetComponent<KinematicState>().maxSpeed /= 7;
                    arrive.enabled = false;
                    break;
                case State.WAIT_IN:
                    break;
                case State.FSM_EAT_FISH:
                    fsm_eatFish.Exit();
                    break;
            }

            // ENTER STATE LOGIC. Depends on newState
            switch (newState)
            {
                case State.SEARCH_HIDEOUT:                                    
                    break;
                case State.REACHING_HIDEOUT:
                    GetComponent<KinematicState>().maxAcceleration *= 7;
                    GetComponent<KinematicState>().maxSpeed *= 7;
                    arrive.enabled = true;
                    arrive.target = hideout;
                    break;
                case State.WAIT_IN:
                    Debug.Log("Hiding");
                    elapsedTime = 0.0f;
                    break;
                case State.FSM_EAT_FISH:
                    fsm_eatFish.ReEnter();
                    break;
            }
            currentState = newState;
        }
    }
}

