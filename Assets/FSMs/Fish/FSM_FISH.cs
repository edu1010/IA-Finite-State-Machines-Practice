using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steerings;
namespace FSM
{
    [RequireComponent(typeof(Arrive))]
    [RequireComponent(typeof(Flocking))]
    public class FSM_FISH : FiniteStateMachine
    {
        public enum State
        {
            INITIAL,EAT,HIDE
        };

        public State currentState = State.INITIAL;
        private FISH_Blackboard blackboard;
        private Arrive arrive;
        private Flocking flocking;

        private FSM_EAT_EAT_PLANKTON eatBehievour;
        private FSM_HIDE hideBehievour;
        
        // Start is called before the first frame update
        void Start()
        {
            eatBehievour = GetComponent<FSM_EAT_EAT_PLANKTON>();
            hideBehievour = GetComponent<FSM_HIDE>();
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
            switch (currentState)
            {
                case State.INITIAL:
                    ChangeState(State.EAT);
                    break;
                case State.EAT:
                    break;
                case State.HIDE:
                    break;
            }
        }


        private void ChangeState(State newState)
        {
            // EXIT STATE LOGIC. Depends on current state
            switch (currentState) 
            {
                case State.INITIAL:
                    ChangeState(State.EAT);
                    break;
                case State.EAT:
                    eatBehievour.Exit();
                    break;
                case State.HIDE:
                    hideBehievour.Exit();
                    break;

            }

            // ENTER STATE LOGIC. Depends on newState
            switch (newState)
            {
                case State.EAT:
                    eatBehievour.ReEnter();
                    break;
                case State.HIDE:
                    hideBehievour.ReEnter();
                    break;
            }
            currentState = newState;
        }
    }
}
