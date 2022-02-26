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
            INITIAL
        };

        public State currentState = State.INITIAL;
        private FISH_Blackboard blackboard;
        private Arrive arrive;
        private Flocking flocking;
        
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
            // EXIT STATE LOGIC. Depends on current state
            switch (currentState) 
            {
            }

            // ENTER STATE LOGIC. Depends on newState
            switch (newState)
            { 
            }
            currentState = newState;
        }
    }
}
