using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steerings;

namespace FSM
{
    public class FSM_Harpoon : FiniteStateMachine
    {
        public enum State
        {
            INITIAL
        };

        public State currentState = State.INITIAL;
        private FISH_Blackboard blackboard;

        // Start is called before the first frame update
        void Start()
        {

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
                    ChangeState(State.INITIAL);
                    break;
            }
        }

        private void ChangeState(State newState)
        {
            // ENTER STATE LOGIC. Depends on newState
            switch (newState)
            {
            }

            // EXIT STATE LOGIC. Depends on current state
            switch (currentState)
            {
            }

            currentState = newState;
        }
    }
}

