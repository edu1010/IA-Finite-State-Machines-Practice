using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steerings;
namespace FSM
{
    [RequireComponent(typeof(FSM_SHARK_Movement))]
    [RequireComponent(typeof(FSM_SHARK_Eat_Fish))]
    [RequireComponent(typeof(FSM_SHARK_Escape))]
    public class FSM_SHARK : FiniteStateMachine
    {
        public enum State
        {
            INITIAL, 
            MOVEMENT, 
            EAT_FISH, 
            ESCAPE
        };

        public State currentState = State.INITIAL;
        private SHARK_Blackboard blackboard;
        private FSM_SHARK_Movement movement;
        private FSM_SHARK_Eat_Fish eatFish;
        private FSM_SHARK_Escape escape;

        void Start()
        {
            movement = GetComponent<FSM_SHARK_Movement>();
            eatFish = GetComponent<FSM_SHARK_Eat_Fish>();
            escape = GetComponent<FSM_SHARK_Escape>();
            blackboard = GetComponent<SHARK_Blackboard>();
        }
        public override void Exit()
        {
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
            Debug.Log(currentState);
            switch (currentState)
            {
                case State.INITIAL:
                    ChangeState(State.MOVEMENT);
                    break;
                case State.MOVEMENT:
                    //Change to Escape
                    /*
                    if (SensingUtils.DistanceToTarget(gameObject, blackboard.harpoon) < blackboard.harpoonDetectionRadius)
                    {
                        ChangeState(State.ESCAPE);
                    }
                    */
                    //Change to Eat Fish
                    blackboard.fishPicked = SensingUtils.FindInstanceWithinRadius(gameObject, "FISH", blackboard.fishReachedRadius);
                    if (blackboard.fishPicked != null)
                    {
                        Debug.Log("Fish picked");
                        ChangeState(State.EAT_FISH);
                    }
                    break;
                case State.EAT_FISH:
                    //Change to Escape
                    /*
                    if (SensingUtils.DistanceToTarget(blackboard.harpoon, gameObject) < blackboard.harpoonDetectionRadius)
                    {
                        ChangeState(State.ESCAPE);
                    }
                    */
                    //Change to Movement
                    if (blackboard.changeToMovementState)
                    {
                        ChangeState(State.MOVEMENT);
                    }
                    break;
                case State.ESCAPE:
                    if (blackboard.changeToMovementState)
                    {
                        ChangeState(State.MOVEMENT);
                    }
                    break;
            }
        }

        private void ChangeState(State newState)
        {
            // EXIT STATE LOGIC. Depends on current state
            switch (currentState)
            {
                case State.MOVEMENT:
                    movement.Exit();
                    break;
                case State.EAT_FISH:
                    eatFish.Exit();
                    break;
                case State.ESCAPE:
                    escape.Exit();
                    break;
            }

            // ENTER STATE LOGIC. Depends on newState
            switch (newState)
            {
                case State.MOVEMENT:
                    movement.ReEnter();
                    break;
                case State.EAT_FISH:
                    eatFish.ReEnter();
                    break;
                case State.ESCAPE:
                    escape.ReEnter();
                    break;
            }
            currentState = newState;
        }
    }
}   

