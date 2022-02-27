using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steerings;

namespace FSM
{
    [RequireComponent(typeof(Arrive))]
    [RequireComponent(typeof(HARPOON_Blackboard))]
    public class FSM_Harpoon : FiniteStateMachine
    {
        
        public enum State
        {
            INITIAL,
            GO_TO_A,
            GO_TO_B,
            ATTACK_SHARK,
            PICK_HARPOON
        };

        public State currentState = State.INITIAL;

        private HARPOON_Blackboard blackboard;
        private Arrive arrive;

        // Start is called before the first frame update
        void Start()
        {
            arrive = GetComponent<Arrive>();
            blackboard = GetComponent<HARPOON_Blackboard>();

            arrive.enabled = false;
        }
        public override void Exit()
        {
            arrive.enabled = false;

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
                    ChangeState(State.GO_TO_A);
                    break;
                case State.GO_TO_A:
                    if ((transform.position - blackboard.attractorA.transform.position).magnitude < blackboard.attractorReachedRadious)
                    {
                        ChangeState(State.GO_TO_B);
                    }
                    if ((transform.position - blackboard.shark.transform.position).magnitude < blackboard.sharkDetectableRadious)
                    {
                        ChangeState(State.ATTACK_SHARK);
                    }
                    break;
                case State.GO_TO_B:
                    if ((transform.position - blackboard.attractorB.transform.position).magnitude < blackboard.attractorReachedRadious)
                    {
                        ChangeState(State.GO_TO_A);
                    }
                    if ((transform.position - blackboard.shark.transform.position).magnitude < blackboard.sharkDetectableRadious)
                    {
                        ChangeState(State.ATTACK_SHARK);
                    }
                    break;
                case State.ATTACK_SHARK:
                    Debug.Log("attack shark");
                    break;
                case State.PICK_HARPOON:
                    break;
            }
        }

        private void ChangeState(State newState)
        {
            // ENTER STATE LOGIC. Depends on newState
            switch (newState)
            {
                case State.GO_TO_A:
                    arrive.target = blackboard.attractorA; //target gusano seleccionado
                    arrive.enabled = true;
                    break;
                case State.GO_TO_B:
                    arrive.target = blackboard.attractorB; //target gusano seleccionado
                    arrive.enabled = true;
                    break;
            }

            // EXIT STATE LOGIC. Depends on current state
            switch (currentState)
            {
                case State.GO_TO_A:
                    //arrive.enabled = false;
                    break;
                case State.GO_TO_B:
                    //arrive.enabled = false;
                    break;
            }

            currentState = newState;
        }
    }
}

