using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steerings;
namespace FSM
{
    public class FSM_FISH_SHARK_FLEE : FiniteStateMachine
    {
        public enum State
        {
            INITIAL, SHARK_FLEE
        };

        public State currentState = State.INITIAL;
        private FISH_Blackboard blackboard;
        private GameObject nearTortoise;
        private float elapsedTime = 0;
        private Flee flee;
        // Start is called before the first frame update
        void Awake()
        {
            blackboard = GetComponent<FISH_Blackboard>();
            flee = GetComponent<Flee>();
            flee.target = blackboard.shark;
        }
        public override void Exit()
        {
            flee.enabled = false;
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
                    ChangeState(State.SHARK_FLEE);
                    break;
                case State.SHARK_FLEE:
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
                case State.SHARK_FLEE:
                    elapsedTime = 0;
                    flee.enabled = false;
                    break;
            }

            // ENTER STATE LOGIC. Depends on newState
            switch (newState)
            {
                case State.INITIAL:
                    break;
                case State.SHARK_FLEE:
                    flee.enabled = true;
                    flee.target = blackboard.shark;
                    break;
            }
            currentState = newState;
        }
    }

}

