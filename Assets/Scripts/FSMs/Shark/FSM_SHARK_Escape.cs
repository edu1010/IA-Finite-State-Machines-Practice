using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steerings;

namespace FSM
{
    [RequireComponent(typeof(SHARK_Blackboard))]
    [RequireComponent(typeof(Arrive))]
    [RequireComponent(typeof(FSM_SHARK_Movement))]

    public class FSM_SHARK_Escape : FiniteStateMachine
    {
        
        public enum State {INITIAL, SEARCH_HIDEOUT, REACHING_HIDEOUT, WAIT_IN, FSM_MOVEMENT};
             
        public State currentState = State.INITIAL;

        private Arrive arrive;
        private SHARK_Blackboard blackboard;
        private FSM_SHARK_Movement fsm_movement;

        private GameObject hideout;
        private float elapsedTime = 0.0f;        

        void Awake()
        {            
            arrive = GetComponent<Arrive>();
            blackboard = GetComponent<SHARK_Blackboard>();
            fsm_movement = GetComponent<FSM_SHARK_Movement>();

            fsm_movement.enabled = false;
            arrive.enabled = false;
        }
        public override void Exit()
        {
            arrive.enabled = false;
            fsm_movement.enabled = false;
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
                    ChangeState(State.FSM_MOVEMENT);
                    break;
                case State.SEARCH_HIDEOUT:
                    hideout = SensingUtils.FindInstanceWithinRadius(gameObject, "HIDEOUT", blackboard.hideoutDetectionRadius);
                    if (hideout != null)
                    {
                        ChangeState(State.REACHING_HIDEOUT);
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
                        ChangeState(State.FSM_MOVEMENT);
                        break;
                    }
                    elapsedTime += Time.deltaTime;                    
                    break;
                case State.FSM_MOVEMENT:
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
                    GetComponent<KinematicState>().maxAcceleration /= 4;
                    GetComponent<KinematicState>().maxSpeed /= 3;
                    arrive.enabled = false;
                    break;
                case State.WAIT_IN:
                    break;
                case State.FSM_MOVEMENT:
                    fsm_movement.Exit();
                    break;
            }

            // ENTER STATE LOGIC. Depends on newState
            switch (newState)
            {
                case State.SEARCH_HIDEOUT:                                    
                    break;
                case State.REACHING_HIDEOUT:
                    GetComponent<KinematicState>().maxAcceleration *= 4;
                    GetComponent<KinematicState>().maxSpeed *= 3;
                    arrive.enabled = true;
                    arrive.target = hideout;
                    break;
                case State.WAIT_IN:
                    blackboard.IsHided = true;
                    elapsedTime = 0.0f;
                    break;
                case State.FSM_MOVEMENT:
                    blackboard.IsHided = false;
                    fsm_movement.ReEnter();
                    break;
            }
            currentState = newState;
        }
    }
}

