using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steerings;

namespace FSM
{
    [RequireComponent(typeof(TURTLE_BLACKBOARD))]
    [RequireComponent(typeof(Arrive))]
    //[RequireComponent(typeof(FSM_TURTLE_Wander))]
    public class FSM_TURTLE_Breathe : FiniteStateMachine
    {
        public enum State
        {
            INITIAL, SEARCH_SURFACE, REACH_SURFACE, TAKING_BREATH, WANDER
        };

        public State currentState = State.INITIAL;
        private TURTLE_BLACKBOARD blackboard;

        //private FSM_TURTLE_Wander turtleWanderFsm;
        private Arrive arrive;

        private GameObject surface;

        public float currentOxigen = 10.0f;

        // Start is called before the first frame update
        void Start()
        {
            arrive = GetComponent<Arrive>();
            blackboard = GetComponent<TURTLE_BLACKBOARD>();
            //turtleWanderFsm = GetComponent<FSM_TURTLE_Wander>();

            arrive.enabled = false;
            //turtleWanderFsm.enabled = false;
        }
        public override void Exit()
        {
            arrive.enabled = false;
            //turtleWanderFsm.enabled = false;
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
                    ChangeState(State.SEARCH_SURFACE);
                    break;
                case State.SEARCH_SURFACE:                    
                    if(surface != null)
                    {
                        ChangeState(State.REACH_SURFACE);
                        break;
                    }
                    break;
                case State.REACH_SURFACE:
                    if (SensingUtils.DistanceToTarget(gameObject, surface) <= blackboard.surfaceReachedRadius)
                    {
                        ChangeState(State.TAKING_BREATH);
                        break;
                    }
                    break;
                case State.TAKING_BREATH:
                    if (currentOxigen >= blackboard.maxOxigen)
                    {
                        ChangeState(State.WANDER);
                        break;
                    }
                    else 
                    {
                        currentOxigen += 10 * Time.deltaTime;
                    }                   
                    Debug.Log("taking breath");
                    break;
                case State.WANDER:
                    Debug.Log("Wandering");
                    break;
            }
        }

        private void ChangeState(State newState)
        {
            // EXIT STATE LOGIC. Depends on current state
            switch (currentState)
            {
                case State.SEARCH_SURFACE:                    
                    break;
                case State.REACH_SURFACE:
                    arrive.enabled = false;
                    break;
                case State.TAKING_BREATH:
                    currentOxigen = 100.0f;
                    break;
                case State.WANDER:
                    // turtleWanderFsm.Exit();
                    break;
            }

            // ENTER STATE LOGIC. Depends on newState
            switch (newState)
            {
                case State.SEARCH_SURFACE:
                    surface = blackboard.surface;                                                          
                    break;
                case State.REACH_SURFACE:
                    arrive.target = surface;
                    arrive.enabled = true;
                    break;
                case State.TAKING_BREATH:                    
                    break;
                case State.WANDER:
                    // turtleWanderFsm.ReEnter();
                    break;
            }
            currentState = newState;
        }
    }
}

