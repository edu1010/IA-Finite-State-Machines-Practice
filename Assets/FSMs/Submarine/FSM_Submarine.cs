using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steerings;

namespace FSM
{
    //[RequireComponent(typeof(ArriveBoat))]
    //[RequireComponent(typeof(FSM_Harpoon))]
    [RequireComponent(typeof(SUBMARINE_MISSILE_Blackboard))]
    public class FSM_Submarine : FiniteStateMachine
    {
        public enum State
        {
            INITIAL,
            GO_TO_A,
            GO_TO_B,
            STAYA,
            STAYB           

        };

        public State currentState = State.INITIAL;

        private SUBMARINE_MISSILE_Blackboard blackboard;
        private ArriveBoat arriveBoat;

        private float elapsedTime = 0.0f;


        private float posY;

        // Start is called before the first frame update
        void Start()
        {
            posY = transform.position.y;
            arriveBoat = GetComponent<ArriveBoat>();
            blackboard = GetComponent<SUBMARINE_MISSILE_Blackboard>();
            //fsmHarpoon = GetComponent<FSM_Harpoon>();

            arriveBoat.enabled = false;
            //fsmHarpoon.Exit();
        }
        public override void Exit()
        {
            arriveBoat.enabled = false;
            //fsmHarpoon.Exit();
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
            transform.position = new Vector2(transform.position.x, posY);
            switch (currentState)
            {
                case State.INITIAL:
                    ChangeState(State.GO_TO_A);
                    break;
                case State.GO_TO_A:
                    if ((transform.position - blackboard.attractorA.transform.position).magnitude <= blackboard.attractorReachedRadious)
                    {
                        ChangeState(State.STAYA);
                        break;
                    }

                    elapsedTime += Time.deltaTime;
                    break;
                case State.GO_TO_B:
                    if ((transform.position - blackboard.attractorB.transform.position).magnitude <= blackboard.attractorReachedRadious)
                    {
                        ChangeState(State.STAYA);
                        break;
                    }

                    break;
                case State.STAYA:
                    if (elapsedTime >= blackboard.maxTimeStay)
                    {
                        ChangeState(State.GO_TO_B);
                        break;
                    }

                    elapsedTime += Time.deltaTime;
                    break;
                case State.STAYB:
                    if (elapsedTime >= blackboard.maxTimeStay)
                    {
                        ChangeState(State.GO_TO_A);
                        break;
                    }

                    elapsedTime += Time.deltaTime;
                    break;
            }
        }

        private void ChangeState(State newState)
        {
            // ENTER STATE LOGIC. Depends on newState
            switch (newState)
            {
                case State.GO_TO_A:
                    arriveBoat.target = blackboard.attractorA;
                    arriveBoat.enabled = true;
                    break;
                case State.GO_TO_B:
                    arriveBoat.target = blackboard.attractorB;
                    arriveBoat.enabled = true;
                    break;
                case State.STAYA:
                    elapsedTime = 0.0f;
                    arriveBoat.enabled = false;
                    break;
                case State.STAYB:
                    elapsedTime = 0.0f;
                    arriveBoat.enabled = false;
                    break;

            }

            // EXIT STATE LOGIC. Depends on current state
            switch (currentState)
            {
                case State.GO_TO_A:
                    arriveBoat.enabled = false;
                    break;
                case State.GO_TO_B:
                    arriveBoat.enabled = false;
                    break;
                case State.STAYA:
                    elapsedTime = 0.0f;
                    break;
                case State.STAYB:
                    elapsedTime = 0.0f;
                    break;

            }

            currentState = newState;
        }

    }
}

