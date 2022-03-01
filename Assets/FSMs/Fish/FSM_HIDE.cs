using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steerings;
namespace FSM
{
    [RequireComponent(typeof(Flee))]
    public class FSM_HIDE : FiniteStateMachine
    {
        public enum State
        {
            INITIAL,GOTO_TORTOISE,SEARCH_ANEMONA,GOTO_ANEMONA,WAIT,SHARK_FLEE
        };

        public State currentState = State.INITIAL;
        private FISH_Blackboard blackboard;
        private WanderAround wanderAround;
        private Arrive arrive;
        private GameObject nearTortoise;
        private float elapsedTime = 0;
        private float elapsedTimeSearch = 0;
        private GameObject anemona;
        private Flee flee;
        private KinematicState kinematicState;
        // Start is called before the first frame update
        void Awake()
        {
            blackboard = GetComponent<FISH_Blackboard>();
            wanderAround = GetComponent<WanderAround>();
            arrive = GetComponent<Arrive>();
            flee = GetComponent<Flee>();
            flee.target = blackboard.shark;
            kinematicState = GetComponent<KinematicState>();
        }
        public override void Exit()
        {
            arrive.enabled = false;
            wanderAround.enabled = false;
            flee.enabled = false;
            transform.parent = null;
            blackboard.isHiding = false;
            transform.tag = blackboard.tagFisH;
            transform.tag = blackboard.tagFisH;


            kinematicState.maxSpeed = kinematicState.maxSpeed / blackboard.speedMultiplayer;
            kinematicState.maxAcceleration = kinematicState.maxAcceleration / blackboard.speedMultiplayer;
            base.Exit();
        }

        public override void ReEnter()
        {
            currentState = State.INITIAL;
            kinematicState.maxSpeed = kinematicState.maxSpeed * blackboard.speedMultiplayer;
            kinematicState.maxAcceleration = kinematicState.maxAcceleration * blackboard.speedMultiplayer;
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
                case State.GOTO_TORTOISE:
                    nearTortoise = blackboard.GetNearTurtleAvalible(gameObject.transform);
                    if(nearTortoise==null)
                    {
                        ChangeState(State.GOTO_ANEMONA);
                        break;
                    
                    }
                    if (SensingUtils.DistanceToTarget(gameObject, nearTortoise) <= blackboard.generalReachedRadius)
                    {
                        if (nearTortoise.transform.childCount < blackboard.maxFishInTortoise)
                        {
                            transform.parent = nearTortoise.transform;
                            ChangeState(State.WAIT);
                            break;
                        }
                    }
                    break;
                case State.SHARK_FLEE:
                    if (SensingUtils.DistanceToTarget(gameObject, blackboard.shark) > blackboard.stopFlee )
                    {
                        ChangeState(State.GOTO_TORTOISE);
                        break;
                    }
                    nearTortoise = SensingUtils.FindInstanceWithinRadius(gameObject, "TORTOISE", blackboard.radiusNearTortoise);
                    if(nearTortoise != null)
                    {
                        ChangeState(State.GOTO_TORTOISE);
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
                    if (SensingUtils.DistanceToTarget(gameObject, anemona) <= blackboard.generalReachedRadius)
                    {
                        transform.parent = anemona.transform;
                        ChangeState(State.WAIT);
                        break;
                    }
                    break;
                case State.WAIT:
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
                case State.SHARK_FLEE:
                    elapsedTime = 0;
                    wanderAround.enabled = false;
                    flee.enabled = false;
                    break;
                case State.GOTO_ANEMONA:
                    elapsedTime = 0;
                    arrive.enabled = false;
                    break;
                case State.WAIT:
                    transform.tag = blackboard.tagFisH;
                    break;
            }

            // ENTER STATE LOGIC. Depends on newState
            switch (newState)
            {
                case State.INITIAL:
                    break;
                case State.GOTO_TORTOISE:
                    nearTortoise = SensingUtils.FindInstance(gameObject, "TORTOISE");
                    arrive.enabled = true;
                  arrive.target = nearTortoise;

                    break;
                case State.SEARCH_ANEMONA:
                    wanderAround.enabled = true;
                    break;
                case State.SHARK_FLEE:
                    flee.enabled = true;
                    flee.target = blackboard.shark;
                   // wanderAround.enabled = true;
                    wanderAround.attractor = (Random.Range(0f, 1f) > 0.5f ? blackboard.atractorA : blackboard.atractorB);
                    break;
                case State.GOTO_ANEMONA:
                    anemona = SensingUtils.FindInstance(gameObject, "ANEMONA");
                    arrive.enabled = true;
                    arrive.target = anemona;
                    break;
                case State.WAIT:
                  blackboard.isHiding = true;
                    transform.tag = blackboard.tagFishHide;
                    break;
            }
            currentState = newState;
        }
        
    }
   
}
