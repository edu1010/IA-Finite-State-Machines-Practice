using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steerings;
namespace FSM
{
    [RequireComponent(typeof(FlockingAroundPlusAvoid))]
    [RequireComponent(typeof(WanderAround))]
    [RequireComponent(typeof(Arrive))]
    public class FSM_EAT_EAT_PLANKTON : FiniteStateMachine
    {
        public enum State
        {
            INITIAL,SEARCH,GOTO_PLANKTON,EAT,FLOACKING
        };

        public State currentState = State.INITIAL;
        private FISH_Blackboard blackboard;
        private float hungry = 0f;
        private float elapsedTime = 0;
        private float elapsedTimeFlocking = 0;
        private FlockingAroundPlusAvoid flocking;
        private WanderAround wanderAround;
        private Arrive arrive;
        private GameObject food;

        // Start is called before the first frame update
        void Start()
        {
            blackboard = GetComponent<FISH_Blackboard>();
            flocking = GetComponent<FlockingAroundPlusAvoid>();
            wanderAround = GetComponent<WanderAround>();
            arrive = GetComponent<Arrive>();
        }
        public override void Exit()
        {
            arrive.enabled = false;
            wanderAround.enabled = false;
            flocking.enabled = false;
            base.Exit();
        }

        public override void ReEnter()
        {
            hungry = 0f;
            currentState = State.INITIAL;
            base.ReEnter();
        }
        // Update is called once per frame
        void Update()
        {
            switch (currentState)
            {
                case State.INITIAL:
                    ChangeState(State.FLOACKING);
                    break;
                case State.SEARCH:
                    elapsedTime += Time.deltaTime;
                    food = SensingUtils.FindInstanceWithinRadius(gameObject, "FISH_FOOD", blackboard.foodDetectableRadius);
                    if (food != null)
                    {
                        ChangeState(State.GOTO_PLANKTON);
                        break;
                    }
                    elapsedTime = blackboard.ChangeWeightWander(wanderAround, elapsedTime);
                    break;
                case State.GOTO_PLANKTON:
                    // check if the food has vanished
                    if (food == null || food.Equals(null))
                    {
                        ChangeState(State.SEARCH);
                        break;
                    }
                    // check if the food has been reached 
                    if (SensingUtils.DistanceToTarget(gameObject, food) <= blackboard.foodReachedRadius)
                    {
                        food.tag = "FISH_FOOD_EATEN";
                        ChangeState(State.EAT);
                        break;
                    }
                    break;
                case State.EAT:
                    elapsedTime += Time.deltaTime;
                    if (elapsedTime>= blackboard.timeToEat)
                    {
                        Destroy(food);
                        hungry--;
                        if (hungry <= 0)
                        {
                            ChangeState(State.FLOACKING);
                            break;
                        }
                        else
                        {
                            ChangeState(State.SEARCH);
                            break;
                        }
                        
                    }
                    break;
                case State.FLOACKING:
                    elapsedTime += Time.deltaTime;
                    elapsedTimeFlocking += Time.deltaTime;
                    if (elapsedTime > 1)
                    {
                        hungry += blackboard.eatPlnktonValue;
                        elapsedTime = 0f;
                    }
                    if (hungry > blackboard.timeFloking)
                    {
                        ChangeState(State.SEARCH);
                        break;
                    }

                    if (SensingUtils.DistanceToTarget(gameObject, flocking.attractor) >= blackboard.maxDistanceAtractor)
                    {
                        if (elapsedTimeFlocking > blackboard.frecuencyIncrementWeight)
                        {
                            elapsedTimeFlocking = 0f;
                            flocking.seekWeight += blackboard.incrementSeekWeight;
                            if(flocking.seekWeight < blackboard.minWeight)
                            {
                                flocking.seekWeight = blackboard.minWeight;
                            }
                        }
                    }
                    else
                    {
                        if (elapsedTimeFlocking > blackboard.frecuencyIncrementWeight)
                        {
                            elapsedTimeFlocking = 0f;
                            flocking.seekWeight -= blackboard.incrementSeekWeight;
                            if (flocking.seekWeight < blackboard.minWeight)
                            {
                                flocking.seekWeight = blackboard.minWeight;
                            }
                        }
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
                    flocking.enabled = false;
                    wanderAround.enabled = false;
                    arrive.enabled = false;
                    elapsedTime = 0f;
                    break;
                case State.SEARCH:
                    wanderAround.enabled = false;
                    break;
                case State.GOTO_PLANKTON:
                    arrive.enabled = false;
                    break;
                case State.EAT:
                    elapsedTime = 0f;
                    break;
                case State.FLOACKING:
                    flocking.enabled = false;
                    break;

            }

            // ENTER STATE LOGIC. Depends on newState
            switch (newState)
            {
                case State.INITIAL:
                   // ChangeState(State.INITIAL);
                    break;
                case State.SEARCH:
                    wanderAround.enabled = true;
                    break;
                case State.GOTO_PLANKTON:
                    elapsedTime = 0f;
                    arrive.target = food;
                    arrive.enabled = true;
                    break;
                case State.EAT:
                    elapsedTime = 0f;
                    break;
                case State.FLOACKING:
                    elapsedTime = 0f;
                    flocking.enabled = true;
                    flocking.idTag = "FISH";
                    flocking.attractor = (Random.Range(0f, 1f) > 0.5f ? blackboard.atractorA : blackboard.atractorB);

                    break;

            }
            currentState = newState;
        }

    }
}

