using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steerings;
namespace FSM
{
    
    [RequireComponent(typeof(WanderAround))]
    [RequireComponent(typeof(Arrive))]
    public class FSM_EAT_EAT_PLANKTON : FiniteStateMachine
    {
        public enum State
        {
            INITIAL,SEARCH,GOTO_PLANKTON,EAT
        };

        public State currentState = State.INITIAL;
        private FISH_Blackboard blackboard;
        private float elapsedTime = 0;
        private float elapsedTimeFlocking = 0;
        private FlockingAround flocking;
        private WanderAround wanderAround;
        private Arrive arrive;
        private GameObject food;
        private FSM_FISH fsm_fish;
        // Start is called before the first frame update
        void Awake()
        {
            blackboard = GetComponent<FISH_Blackboard>();
            flocking = GetComponent<FlockingAround>();
            wanderAround = GetComponent<WanderAround>();
            arrive = GetComponent<Arrive>();
            fsm_fish = GetComponent<FSM_FISH>();
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
            currentState = State.INITIAL;
            base.ReEnter();
        }
        // Update is called once per frame
        void Update()
        {
            switch (currentState)
            {
                case State.INITIAL:
                    ChangeState(State.SEARCH);
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
                        blackboard.DecrementHungry();
                        if (blackboard.currentHungry <= 0)
                        {
                            //FISH MACHINE CHANGE THE STATE
                        }
                        else
                        {
                            ChangeState(State.SEARCH);
                            break;
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

            }
            currentState = newState;
        }

    }
}

