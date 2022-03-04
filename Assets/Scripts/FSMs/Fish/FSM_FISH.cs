using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steerings;
namespace FSM
{
    [RequireComponent(typeof(FSM_EAT_EAT_PLANKTON))]
    [RequireComponent(typeof(FSM_ESCAPE))]
    [RequireComponent(typeof(FlockingAround))]
    public class FSM_FISH : FiniteStateMachine
    {
        public enum State
        {
            INITIAL,EAT,HIDE,FLOKING
        };

        public State currentState = State.INITIAL;
        private FISH_Blackboard blackboard;
       
        private FSM_EAT_EAT_PLANKTON eatFSM;
        private FSM_ESCAPE escapeFSM;
        private float elapsedTime = 0;
        private float elapsedTimeFlocking;
        private FlockingAround flocking;
        public float distance;
        float time;
        KinematicState kinematic;
        void Start()
        {
            eatFSM = GetComponent<FSM_EAT_EAT_PLANKTON>();
            escapeFSM = GetComponent<FSM_ESCAPE>();
            blackboard = GetComponent<FISH_Blackboard>();
            flocking = GetComponent<FlockingAround>();
            kinematic = GetComponent<KinematicState>();
        }
        public override void Exit()
        {
            eatFSM.Exit();
            escapeFSM.Exit();
            flocking.enabled = false;
            base.Exit();
        }

        public override void ReEnter()
        {
            currentState = State.INITIAL;
            escapeFSM.enabled = false;
            eatFSM.enabled = false;
           
            base.ReEnter();
        }

        void Update()
        {
            switch (currentState)
            {
                case State.INITIAL:
                    ChangeState(State.FLOKING);
                    break;
                case State.EAT:
                    distance = SensingUtils.DistanceToTarget(gameObject, blackboard.shark);
                    if(blackboard.maxDistanceToShark > SensingUtils.DistanceToTarget(gameObject, blackboard.shark)){
                        ChangeState(State.HIDE);
                    }
                    if (blackboard.currentHungry <= 0)
                    {
                        ChangeState(State.FLOKING);
                    }
                    break;
                case State.HIDE:
                    if (blackboard.isHiding)
                    {
                        elapsedTime += Time.deltaTime;
                        if (elapsedTime > blackboard.maxTimeHiding)
                        {
                            ChangeState(State.FLOKING);
                            break;
                        }
                    }
                    break;

                case State.FLOKING:
                    distance = SensingUtils.DistanceToTarget(gameObject, blackboard.shark);
                    if (blackboard.maxDistanceToShark > SensingUtils.DistanceToTarget(gameObject, blackboard.shark))
                    {
                        ChangeState(State.HIDE);
                    }
                    elapsedTimeFlocking += Time.deltaTime;
                    elapsedTime += Time.deltaTime;
                    
                    if (elapsedTime > 1f)
                    {
                      blackboard.IncrementHungry();
                        elapsedTime = 0f;
                    }
                    if (blackboard.currentHungry > blackboard.timeFloking)
                    {
                        ChangeState(State.EAT);
                        break;
                    }
                    time = Time.time;
                    break;
            }
        }

        private void LateUpdate()
        {
            kinematic.linearVelocity.z = 0;
            kinematic.position.z = 0;
        }
        private void ChangeState(State newState)
        {
            // EXIT STATE LOGIC. Depends on current state
            switch (currentState) 
            {
                case State.INITIAL:
                    escapeFSM.Exit();
                    eatFSM.Exit();
                    break;
                case State.EAT:
                    eatFSM.Exit();
                    break;
                case State.HIDE:
                    escapeFSM.Exit();
                    break;
                case State.FLOKING:
                    flocking.enabled = false;
                    break;

            }

            // ENTER STATE LOGIC. Depends on newState
            switch (newState)
            {
                case State.EAT:
                    eatFSM.ReEnter();
                    break;
                case State.HIDE:
                    escapeFSM.ReEnter();
                    break;
                case State.FLOKING:
                    time = Time.time;
                    elapsedTime = 0f;
                    elapsedTimeFlocking = 0;
                    blackboard.currentHungry = 0;
                    flocking.idTag = "FISH";
                    flocking.attractor = blackboard.atractorB;
                    flocking.enabled = true;
                    blackboard.currentHungry = 0;
                    break;
            }
            currentState = newState;
        }
    }
    
}
