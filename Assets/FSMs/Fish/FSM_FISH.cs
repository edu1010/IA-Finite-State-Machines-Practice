using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steerings;
namespace FSM
{
    [RequireComponent(typeof(FSM_EAT_EAT_PLANKTON))]
    [RequireComponent(typeof(FSM_HIDE))]
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
        private FSM_HIDE hideFSM;
        private float elapsedTime;
        private float elapsedTimeFlocking;
        private FlockingAround flocking;
        public float distance;
        
        void Start()
        {
            eatFSM = GetComponent<FSM_EAT_EAT_PLANKTON>();
            hideFSM = GetComponent<FSM_HIDE>();
            blackboard = GetComponent<FISH_Blackboard>();
            flocking = GetComponent<FlockingAround>();
        }
        public override void Exit()
        {
            eatFSM.Exit();
            hideFSM.Exit();
            flocking.enabled = false;
            base.Exit();
        }

        public override void ReEnter()
        {
            currentState = State.INITIAL;
            hideFSM.enabled = false;
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
                   /* if (blackboard.maxDistanceToShark < SensingUtils.DistanceToTarget(gameObject, blackboard.shark))
                    {
                        
                    }*/
                    break;

                case State.FLOKING:
                    elapsedTimeFlocking += Time.deltaTime;
                    elapsedTime += Time.deltaTime;

                    if (elapsedTime > 1)
                    {
                      blackboard.IncrementHungry();
                        elapsedTime = 0f;
                    }
                    if (blackboard.currentHungry > blackboard.timeFloking)
                    {
                        ChangeState(State.EAT);
                        break;
                    }

                    //SetWeight
                    /*
                    if (SensingUtils.DistanceToTarget(gameObject, flocking.attractor) >= blackboard.maxDistanceAtractor)
                    {
                        if (elapsedTimeFlocking > blackboard.frecuencyIncrementWeight)
                        {
                            elapsedTimeFlocking = 0f;
                            flocking.seekWeight += blackboard.incrementSeekWeight;
                            if (flocking.seekWeight < blackboard.minWeight)
                            {
                                flocking.seekWeight = blackboard.minWeight;
                            }
                        }
                    }
                    else
                    {
                        if (elapsedTime > blackboard.frecuencyIncrementWeight)
                        {
                            elapsedTimeFlocking = 0f;
                            flocking.seekWeight -= blackboard.incrementSeekWeight;
                            if (flocking.seekWeight < blackboard.minWeight)
                            {
                                flocking.seekWeight = blackboard.minWeight;
                            }
                        }
                    }*/
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
                    hideFSM.ReEnter();
                    break;
                case State.FLOKING:
                    elapsedTime = 0f;
                    flocking.idTag = "FISH";
                    // flocking.attractor = (Random.Range(0f, 1f) > 0.5f ? blackboard.atractorA : blackboard.atractorB);
                    flocking.attractor = blackboard.atractorA;
                    flocking.enabled = true;
                    blackboard.currentHungry = 0;
                    break;
            }
            currentState = newState;
        }
    }
    
}
