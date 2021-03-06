using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steerings;
namespace FSM
{
    [RequireComponent(typeof(Flee))]
    public class FSM_FISH_HIDE : FiniteStateMachine
    {
        public enum State
        {
            INITIAL, GOTO_TORTOISE, GOTO_ANEMONA, WAIT
        };

        public State currentState = State.INITIAL;
        private FISH_Blackboard blackboard;
        private WanderAround wanderAround;
        private Arrive arrive;
        private GameObject nearTortoise;
        private KinematicState kinematicState;
        // Start is called before the first frame update
        void Awake()
        {
            blackboard = GetComponent<FISH_Blackboard>();
            wanderAround = GetComponent<WanderAround>();
            arrive = GetComponent<Arrive>();
            kinematicState = GetComponent<KinematicState>();
        }
        public override void Exit()
        {
            arrive.enabled = false;
            wanderAround.enabled = false;
            if (transform.parent != null && transform.parent.tag.Equals(blackboard.tagTurtle))
            {
                kinematicState.position = nearTortoise.transform.parent.GetComponent<KinematicState>().position;
                gameObject.transform.position = nearTortoise.transform.position;
                transform.parent = null;
            }
            if(nearTortoise != null)
            {
                HideOutTurtleController.hideOutTurtleController.AddAvalibleTarget(nearTortoise);
            }

            transform.parent = null;

            blackboard.isHiding = false;
            transform.tag = blackboard.tagFisH;
            transform.tag = blackboard.tagFisH;

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
                    ChangeState(State.GOTO_TORTOISE);
                    break;
                case State.GOTO_TORTOISE:
                    if (nearTortoise == null || nearTortoise.Equals(null))
                    {
                        ChangeState(State.GOTO_ANEMONA);
                        break;
                    }
                    if (nearTortoise.transform.childCount > blackboard.maxFishInTortoise)
                    {
                        nearTortoise = HideOutTurtleController.hideOutTurtleController.GetNearTurtleAvalible(gameObject.transform);
                        if (nearTortoise == null || nearTortoise.Equals(null))
                        {
                            ChangeState(State.GOTO_ANEMONA);
                            break;
                        }
                    }
                   
                    if (SensingUtils.DistanceToTarget(gameObject, nearTortoise) <= blackboard.turtleReachedRadiusStopArraive)
                    {
                        transform.parent = nearTortoise.transform;
                        ChangeState(State.WAIT);
                        break;
                    }
                    break;
                case State.GOTO_ANEMONA:
                    if (SensingUtils.DistanceToTarget(gameObject, blackboard.anemona) <= blackboard.generalReachedRadius)
                    {
                        transform.parent = blackboard.anemona.transform;
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
                    arrive.enabled = false;
                    break;
                case State.GOTO_ANEMONA:
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
                    nearTortoise = HideOutTurtleController.hideOutTurtleController.GetNearTurtleAvalible(gameObject.transform);
                    arrive.enabled = true;
                    arrive.target = nearTortoise;
                    arrive.closeEnoughRadius = blackboard.turtleReachedRadius;
                    break;
                case State.GOTO_ANEMONA:
                    arrive.enabled = true;
                    arrive.target = blackboard.anemona;
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
