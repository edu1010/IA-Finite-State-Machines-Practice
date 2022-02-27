using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steerings;
namespace FSM
{
    [RequireComponent(typeof(FSM_EAT_EAT_PLANKTON))]
    [RequireComponent(typeof(FSM_HIDE))]
    
    public class FSM_FISH : FiniteStateMachine
    {
        public enum State
        {
            INITIAL,EAT,HIDE
        };

        public State currentState = State.INITIAL;
        private FISH_Blackboard blackboard;
       
        private FSM_EAT_EAT_PLANKTON eatFSM;
        private FSM_HIDE hideFSM;
        
        // Start is called before the first frame update
        void Start()
        {
            eatFSM = GetComponent<FSM_EAT_EAT_PLANKTON>();
            hideFSM = GetComponent<FSM_HIDE>();
            blackboard = GetComponent<FISH_Blackboard>();
        }
        public override void Exit()
        {
            base.Exit();
        }

        public override void ReEnter()
        {
            currentState = State.INITIAL;
            hideFSM.enabled = false;
            eatFSM.enabled = false;
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
                    if(blackboard.maxDistanceToShark > SensingUtils.DistanceToTarget(gameObject, blackboard.shark)){
                        ChangeState(State.HIDE);
                    }

                    break;
                case State.HIDE:
                    if (blackboard.maxDistanceToShark < SensingUtils.DistanceToTarget(gameObject, blackboard.shark))
                    {
                        ChangeState(State.EAT);
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
                    hideFSM.Exit();
                    eatFSM.Exit();
                    break;
                case State.EAT:
                    eatFSM.Exit();
                    break;
                case State.HIDE:
                    hideFSM.Exit();
                    break;

            }

            // ENTER STATE LOGIC. Depends on newState
            switch (newState)
            {
                case State.EAT:
                    eatFSM.ReEnter();
                    break;
                case State.HIDE:
                    hideFSM.ReEnter();
                    break;
            }
            currentState = newState;
        }
    }
}
