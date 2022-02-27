using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steerings;
namespace FSM
{
    public class FSM_HIDE : FiniteStateMachine
    {
        public enum State
        {
            INITIAL,GOTO_TORTOISE,SEARCH_ANEMONA,GOTO_ANEMONA,WAIT,SEARCH_TORTOISE
        };

        public State currentState = State.INITIAL;
        private FISH_Blackboard blackboard;
        private WanderAround wanderAround;
        private Arrive arrive;
        private GameObject nearTortoise;
        private float elapsedTime = 0;
        private float elapsedTimeSearch = 0;
        private GameObject anemona;
        // Start is called before the first frame update
        void Start()
        {
            blackboard = GetComponent<FISH_Blackboard>();
            wanderAround = GetComponent<WanderAround>();
            arrive = GetComponent<Arrive>();
        }
        public override void Exit()
        {
            arrive.enabled = false;
            wanderAround.enabled = false;
            transform.parent = null;
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
                    ChangeState(State.SEARCH_TORTOISE);
                    break; 
                case State.GOTO_TORTOISE:
                    if (SensingUtils.DistanceToTarget(gameObject, nearTortoise) <= blackboard.foodReachedRadius)
                    {
                        if (nearTortoise.transform.childCount < blackboard.maxFishInTortoise)
                        {
                            transform.parent = nearTortoise.transform;
                            ChangeState(State.WAIT);
                            break;
                        }
                        else
                        {
                            ChangeState(State.SEARCH_ANEMONA);
                            break;
                        }
                    }
                    break;
                case State.SEARCH_TORTOISE:
                    nearTortoise = SensingUtils.FindInstanceWithinRadius(gameObject, "TORTOISE", blackboard.radiusNearTortoise);
                    if(nearTortoise != null)
                    {
                        ChangeState(State.GOTO_TORTOISE);
                    }
                    elapsedTime += Time.deltaTime;
                    if (elapsedTime > blackboard.frecuencyIncrementWeight)
                    {
                        elapsedTime = 0f;
                        wanderAround.SetSeekWeight(blackboard.incrementSeekWeight + wanderAround.seekWeight);
                    }
                    break;
                case State.SEARCH_ANEMONA:
                    elapsedTime += Time.deltaTime;
                    elapsedTimeSearch += Time.deltaTime;
                    if (elapsedTimeSearch > blackboard.maxTimeSearchAnemona)
                    {
                        ChangeState(State.GOTO_TORTOISE);
                        break;
                    }
                    anemona = SensingUtils.FindInstanceWithinRadius(gameObject, "ANEMONA", blackboard.foodDetectableRadius);
                    if (anemona != null)
                    {
                        ChangeState(State.GOTO_ANEMONA);
                        break;
                    }
                    elapsedTime = blackboard.ChangeWeightWander(wanderAround, elapsedTime);
                    break; 
                case State.GOTO_ANEMONA:
                    if (SensingUtils.DistanceToTarget(gameObject, anemona) <= blackboard.foodReachedRadius)
                    {
                        transform.parent = anemona.transform;
                        ChangeState(State.WAIT);
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
                case State.GOTO_TORTOISE:
                    elapsedTime = 0;
                    arrive.enabled = false;
                    break;
                case State.SEARCH_ANEMONA:
                    elapsedTime = 0;
                    wanderAround.enabled = false;
                    break; 
                case State.SEARCH_TORTOISE:
                    elapsedTime = 0;
                    wanderAround.enabled = false;
                    break;
                case State.GOTO_ANEMONA:
                    elapsedTime = 0;
                    arrive.enabled = false;
                    break;
                case State.WAIT:
                    break;
            }

            // ENTER STATE LOGIC. Depends on newState
            switch (newState)
            {
                case State.INITIAL:
                    break;
                case State.GOTO_TORTOISE:
                  arrive.enabled = true;
                  arrive.target = nearTortoise;

                    break;
                case State.SEARCH_ANEMONA:
                    wanderAround.enabled = true;
                    break;
                case State.SEARCH_TORTOISE:
                    wanderAround.enabled = true;
                    wanderAround.attractor = (Random.Range(0f, 1f) > 0.5f ? blackboard.atractorA : blackboard.atractorB);
                    break;
                case State.GOTO_ANEMONA:
                    arrive.enabled = true;
                    arrive.target = anemona;
                    break;
                case State.WAIT:
                    break;
            }
            currentState = newState;
        }
        
    }
   
}
