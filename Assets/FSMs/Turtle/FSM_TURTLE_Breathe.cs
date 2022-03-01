using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steerings;

namespace FSM
{
    [RequireComponent(typeof(TURTLE_BLACKBOARD))]
    [RequireComponent(typeof(Arrive))]
    public class FSM_TURTLE_Breathe : FiniteStateMachine
    {
        public enum State
        {
            INITIAL, REACH_SURFACE, TAKING_BREATH
        };

        public State currentState = State.INITIAL;
        private TURTLE_BLACKBOARD blackboard;

        private Arrive arrive;

        // Start is called before the first frame update
        void Awake()
        {
            arrive = GetComponent<Arrive>();
            blackboard = GetComponent<TURTLE_BLACKBOARD>();

            blackboard.definitiveBreathingPoint = blackboard.posibleBreathingPoints[Random.Range(0, blackboard.posibleBreathingPoints.Count)];

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
            blackboard.changeToWander = false;
            base.ReEnter();
        }
        // Update is called once per frame
        void Update()
        {
            switch (currentState)
            {
                case State.INITIAL:
                    ChangeState(State.REACH_SURFACE);
                    break;
                case State.REACH_SURFACE:
                    if (SensingUtils.DistanceToTarget(gameObject, blackboard.definitiveBreathingPoint) <= blackboard.surfaceReachedRadius)
                    {
                        ChangeState(State.TAKING_BREATH);
                        break;
                    }
                    break;
                case State.TAKING_BREATH:
                    if (blackboard.currentOxigen >= blackboard.maxOxigen)
                    {
                        blackboard.changeToWander = true;
                        break;
                    }
                    else 
                    {
                        blackboard.currentOxigen += 10 * Time.deltaTime;
                    }                   
                break;
            }
        }

        private void ChangeState(State newState)
        {
            // EXIT STATE LOGIC. Depends on current state
            switch (currentState)
            {
                case State.REACH_SURFACE:
                    arrive.enabled = false;
                    break;
                case State.TAKING_BREATH:
                    blackboard.currentOxigen = blackboard.maxOxigen;
                    break;
            }

            // ENTER STATE LOGIC. Depends on newState
            switch (newState)
            {
                case State.REACH_SURFACE:
                    arrive.target = blackboard.definitiveBreathingPoint;
                    arrive.enabled = true;
                    break;
                case State.TAKING_BREATH:                    
                    break;
            }
            currentState = newState;
        }
    }
}

