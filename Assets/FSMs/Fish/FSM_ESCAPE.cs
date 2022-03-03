using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steerings;
namespace FSM
{
    [RequireComponent(typeof(FSM_FISH_HIDE))]
    [RequireComponent(typeof(FSM_FISH_SHARK_FLEE))]
    public class FSM_ESCAPE : FiniteStateMachine
    {
        public enum State
        {
            INITIAL,FISH_HIDE,FISH_SHARK_FLEE
        };

        public State currentState = State.INITIAL;
        private FISH_Blackboard blackboard;
        private KinematicState kinematicState;
        private FSM_FISH_HIDE fsm_fish_Hide;
        private FSM_FISH_SHARK_FLEE fsm_shark_flee;
        // Start is called before the first frame update
        void Awake()
        {
            blackboard = GetComponent<FISH_Blackboard>();
            kinematicState = GetComponent<KinematicState>();
            fsm_fish_Hide = GetComponent<FSM_FISH_HIDE>();
            fsm_shark_flee = GetComponent<FSM_FISH_SHARK_FLEE>();
        }
        public override void Exit()
        {
            kinematicState.maxSpeed = kinematicState.maxSpeed / blackboard.speedMultiplayer;
            kinematicState.maxAcceleration = kinematicState.maxAcceleration / blackboard.speedMultiplayer;
            fsm_fish_Hide.Exit();
            fsm_shark_flee.Exit();
            base.Exit();
        }

        public override void ReEnter()
        {
            currentState = State.INITIAL;
            kinematicState.maxSpeed = kinematicState.maxSpeed * blackboard.speedMultiplayer;
            kinematicState.maxAcceleration = kinematicState.maxAcceleration * blackboard.speedMultiplayer;
            base.ReEnter();
        }

        // Update is called once per frame
        void Update()
        {
            switch (currentState)
            {
                case State.INITIAL:
                    ChangeState(State.FISH_SHARK_FLEE);
                    break;
                case State.FISH_SHARK_FLEE:
                    if (SensingUtils.DistanceToTarget(gameObject, blackboard.shark) > blackboard.stopFlee)
                    {
                        ChangeState(State.FISH_HIDE);
                        break;
                    }
                    break;
                case State.FISH_HIDE:
                    if (SensingUtils.DistanceToTarget(gameObject, blackboard.shark) < blackboard.stopFlee)
                    {
                        ChangeState(State.FISH_SHARK_FLEE);
                        break;
                    }
                    break;
               
            }
        }


        private void ChangeState(State newState)
        {
            // EXIT STATE LOGIC. Depends on current state
            switch (currentState)
            {
                case State.INITIAL:
                    break;
                case State.FISH_SHARK_FLEE:
                    fsm_shark_flee.Exit();
                    break;
                case State.FISH_HIDE:
                    fsm_fish_Hide.Exit();
                    break;
            }

            // ENTER STATE LOGIC. Depends on newState
            switch (newState)
            {
                case State.INITIAL:
                    break;
                case State.FISH_SHARK_FLEE:
                    fsm_shark_flee.ReEnter();
                    break;
                case State.FISH_HIDE:
                    fsm_fish_Hide.ReEnter();
                    break;
            }
            currentState = newState;
        }
        
    }
   
}
