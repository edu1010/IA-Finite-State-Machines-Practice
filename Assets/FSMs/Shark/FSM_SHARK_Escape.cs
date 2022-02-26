using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steerings;

namespace FSM
{
    [RequireComponent(typeof(SHARK_Blackboard))]
    [RequireComponent(typeof(Arrive))]
    //[RequireComponent(typeof(FSM_SHARK))]

    public class FSM_SHARK_Escape : FiniteStateMachine
    {
        
        public enum State {INITIAL, SEARCH_HIDEOUT, REACHING_HIDEOUT, WAIT_IN, MOVEMENT};
             

        public State currentState = State.INITIAL;

        //private FSM_SHARK;
        //private FSM_SHARK_Movement;
        private Arrive arrive;
        private SHARK_Blackboard blackboard;

        private GameObject harpoon;
        private GameObject hideout;

        private float elapsedTime;

        // Start is called before the first frame update
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
            base.ReEnter();
        }
        // Update is called once per frame
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
                        //ChangeState(State.MOVEMENT);
                        Debug.Log("I'm not hiding");
                        break;
                    }
                    elapsedTime += Time.deltaTime;                    
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
                    arrive.enabled = false;
                    break;
                case State.WAIT_IN:

                    break;

            }

            // ENTER STATE LOGIC. Depends on newState
            switch (newState)
            {
                case State.SEARCH_HIDEOUT:                                    
                    break;
                case State.REACHING_HIDEOUT:
                    arrive.enabled = true;
                    arrive.target = hideout;
                    break;
                case State.WAIT_IN:
                    Debug.Log("Hiding");
                    elapsedTime = 0.0f;
                    break;

            }
            currentState = newState;
        }
    }
}

