using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steerings;

namespace FSM
{
    [RequireComponent(typeof(WanderAround))]
    [RequireComponent(typeof(Arrive))]
    [RequireComponent(typeof(SHARK_Blackboard))]
    [RequireComponent(typeof(FSM_SHARK_Eat_Fish))]
    public class FSM_SHARK_Movement : FiniteStateMachine
    {
        public enum State
        {
            INITIAL,
            WANDER,
            ARRIVE_AT_MARK,
            DASH,
            FSM_EAT_FISH
        };

        public State currentState = State.INITIAL;
        
        private SHARK_Blackboard blackboard;
        private Arrive arrive;
        private WanderAround wanderAround;
        private FSM_SHARK_Eat_Fish fsm_eat_fish;

        private float initialAcceleration;
        private float initialSpeed;

        void Awake()
        {
            wanderAround = GetComponent<WanderAround>();
            arrive = GetComponent<Arrive>();
            blackboard = GetComponent<SHARK_Blackboard>();
            fsm_eat_fish = GetComponent<FSM_SHARK_Eat_Fish>();

            initialAcceleration = GetComponent<KinematicState>().maxAcceleration;
            initialSpeed = GetComponent<KinematicState>().maxSpeed;

            wanderAround.attractor = blackboard.Attractor;

            fsm_eat_fish.enabled = false;
            wanderAround.enabled = false;
            arrive.enabled = false;
            arrive.target = null;
        }

        public override void Exit()
        {
            GetComponent<KinematicState>().maxAcceleration = initialAcceleration;
            GetComponent<KinematicState>().maxSpeed = initialSpeed;
            fsm_eat_fish.enabled = false;
            arrive.enabled = false;
            wanderAround.enabled = false;
            base.Exit();
        }

        public override void ReEnter()
        {
            GetComponent<KinematicState>().maxAcceleration = initialAcceleration;
            GetComponent<KinematicState>().maxSpeed = initialAcceleration;
            currentState = State.INITIAL;
            base.ReEnter();
        }

        void Update()
        {
            //New Arrive Position
            if(Input.GetMouseButtonDown(0) && currentState!=State.FSM_EAT_FISH)
            {
                Vector3 cameraPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                blackboard.ArriveGameObject.transform.position = new Vector3(cameraPos.x, cameraPos.y, 0.0f);
                arrive.target = blackboard.ArriveGameObject;
            }
            //Dash
            if(Input.GetKey(KeyCode.LeftShift))
            {
                blackboard.canDash = true;
            }
            else
            {
                blackboard.canDash = false;
            }

            switch (currentState)
            {
                case State.INITIAL:
                    ChangeState(State.WANDER);
                    break;
                case State.WANDER:
                    //change to eat fsm
                    blackboard.fishPicked = SensingUtils.FindInstanceWithinRadius(gameObject, "FISH", blackboard.fishReachedRadius);
                    if (blackboard.fishPicked != null)
                    {
                        ChangeState(State.FSM_EAT_FISH); break;
                    }

                    if (blackboard.currentStamina < blackboard.maxStamina)
                    {
                        blackboard.currentStamina += Time.deltaTime;
                    }
                    if (blackboard.canDash && blackboard.currentStamina >= 0.0f)
                    {
                        ChangeState(State.DASH); break;
                    }
                    if (arrive.target != null)
                    {
                        ChangeState(State.ARRIVE_AT_MARK); break;
                    }
                    break;
                case State.ARRIVE_AT_MARK:
                    //change to eat fsm
                    blackboard.fishPicked = SensingUtils.FindInstanceWithinRadius(gameObject, "FISH", blackboard.fishReachedRadius);
                    if (blackboard.fishPicked != null)
                    {
                        ChangeState(State.FSM_EAT_FISH); break;
                    }

                    if (blackboard.currentStamina < blackboard.maxStamina)
                    {
                        blackboard.currentStamina += Time.deltaTime;
                    }

                    if (blackboard.canDash && blackboard.currentStamina >= 0.0f)
                    {
                        ChangeState(State.DASH); break;
                    }
                    if(SensingUtils.DistanceToTarget(gameObject, arrive.target) < arrive.closeEnoughRadius)
                    {
                        ChangeState(State.WANDER); break;
                    }
                    break;
                case State.DASH:
                    //change to eat fsm
                    blackboard.fishPicked = SensingUtils.FindInstanceWithinRadius(gameObject, "FISH", blackboard.fishReachedRadius);
                    if (blackboard.fishPicked != null)
                    {
                        ChangeState(State.FSM_EAT_FISH); break;
                    }

                    blackboard.currentStamina -= Time.deltaTime*2;
                    if (blackboard.currentStamina <= 0.0f || blackboard.canDash == false)
                    {
                        ChangeState(State.WANDER); break;
                    }
                    break;
                case State.FSM_EAT_FISH:
                    if(blackboard.changeToMovementState)
                    {
                        ChangeState(State.WANDER); break;
                    }
                    break;
            }
        }

        private void ChangeState(State newState)
        {
            // EXIT STATE LOGIC. Depends on current state
            switch (currentState)
            {
                case State.WANDER:
                    wanderAround.enabled = false;
                    break;
                case State.ARRIVE_AT_MARK:
                    arrive.enabled = false;
                    arrive.target = null;
                    break;
                case State.DASH:
                    arrive.enabled = false;
                    arrive.target = null;
                    GetComponent<KinematicState>().maxSpeed /= 7.0f;
                    GetComponent<KinematicState>().maxAcceleration /= 7.0f;
                    blackboard.canDash = false;
                    break;
                case State.FSM_EAT_FISH:
                    blackboard.changeToMovementState = false;
                    fsm_eat_fish.Exit();
                    break;
            }

            // ENTER STATE LOGIC. Depends on newState
            switch (newState)
            {
                case State.WANDER:
                    wanderAround.enabled = true;
                    break;
                case State.ARRIVE_AT_MARK:
                    arrive.target = blackboard.ArriveGameObject;
                    arrive.enabled = true;
                    break;
                case State.DASH:
                    arrive.target = blackboard.ArriveGameObject;
                    arrive.enabled = true;
                    GetComponent<KinematicState>().maxSpeed *= 7.0f;
                    GetComponent<KinematicState>().maxAcceleration *= 7.0f;
                    break;
                case State.FSM_EAT_FISH:
                    fsm_eat_fish.ReEnter();
                    break;
            }
            currentState = newState;
        }
    }
}