using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steerings;

namespace FSM
{
    [RequireComponent(typeof(WanderAround))]
    [RequireComponent(typeof(Arrive))]
    [RequireComponent(typeof(SHARK_Blackboard))]
    public class FSM_SHARK_Movement : FiniteStateMachine
    {

        public enum State
        {
            INITIAL,
            WANDER,
            ARRIVE_AT_MARK,
            DASH
        };

        public State currentState = State.INITIAL;
        
        private SHARK_Blackboard blackboard;
        private Arrive arrive;
        private WanderAround wanderAround;

        void Start()
        {
            wanderAround = GetComponent<WanderAround>();
            arrive = GetComponent<Arrive>();
            blackboard = GetComponent<SHARK_Blackboard>();

            wanderAround.attractor = blackboard.Attractor;

            wanderAround.enabled = false;
            arrive.enabled = false;
        }

        public override void Exit()
        {
            arrive.enabled = false;
            wanderAround.enabled = false;
            base.Exit();
        }

        public override void ReEnter()
        {
            currentState = State.INITIAL;
            base.ReEnter();
        }

        void Update()
        {
            //Debug.Log(currentState);
            if(Input.GetMouseButtonDown(0))
            {
                blackboard.ArriveGameObject.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }

            switch (currentState)
            {
                case State.INITIAL:
                    ChangeState(State.WANDER);
                    break;
                case State.WANDER:
                    if (blackboard.currentStamina < blackboard.maxStamina)
                    {
                        blackboard.currentStamina += Time.deltaTime;
                    }
                    Debug.Log("charching " + blackboard.currentStamina);
                    Debug.Log("max stamina " + blackboard.maxStamina);
                    if (Input.GetKeyDown(KeyCode.LeftShift) && blackboard.currentStamina >= 0.0f)
                    {
                        ChangeState(State.DASH); break;
                    }

                    if (blackboard.ArriveGameObject.transform.position != Vector3.zero)
                    {
                        ChangeState(State.ARRIVE_AT_MARK); break;
                    }
                    break;
                case State.ARRIVE_AT_MARK:
                    if(blackboard.currentStamina < blackboard.maxStamina)
                    {
                        blackboard.currentStamina += Time.deltaTime;
                    }

                    Debug.Log("charching " + blackboard.currentStamina);
                    Debug.Log("max stamina " + blackboard.maxStamina);
                    if (Input.GetKeyDown(KeyCode.LeftShift) && blackboard.currentStamina >= 0.0f)
                    {
                        ChangeState(State.DASH); break;
                    }
                    if(SensingUtils.DistanceToTarget(gameObject, arrive.target) < arrive.closeEnoughRadius)
                    {
                        ChangeState(State.WANDER); break;
                    }
                    break;
                case State.DASH:
                    Debug.Log("dash: " + blackboard.currentStamina);
                    blackboard.currentStamina -= Time.deltaTime;
                    if (blackboard.currentStamina <= 0.0f)
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
                    blackboard.ArriveGameObject.transform.position = Vector3.zero;
                    arrive.target = null;
                    break;
                case State.DASH:
                    arrive.enabled = false;
                    blackboard.ArriveGameObject.transform.position = Vector3.zero;
                    arrive.target = null;
                    GetComponent<KinematicState>().maxSpeed /= 5.0f;
                    GetComponent<KinematicState>().maxAcceleration /= 5.0f;
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
                    arrive.target = blackboard.ArriveGameObject; //FALTA CAMBIAR ESTO, AHORA VA AL 0,0.
                    arrive.enabled = true;
                    GetComponent<KinematicState>().maxSpeed *= 5.0f;
                    GetComponent<KinematicState>().maxAcceleration *= 5.0f;
                    break;
            }
            currentState = newState;
        }
    }
}