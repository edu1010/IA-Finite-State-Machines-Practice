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
            ATTACK_SHARK,
            PICK_HARPOON
        };

        public State currentState = State.INITIAL;

        private HARPOON_Blackboard blackboard;
        private Arrive arrive;

        private float elapsedTime = 0.0f;

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
                    ChangeState(State.ATTACK_SHARK);
                    break;
                case State.ATTACK_SHARK:
                    Debug.Log("attack shark");
                    if (elapsedTime >= blackboard.timeAttack)
                    {
                        ChangeState(State.PICK_HARPOON);
                        Debug.Log("Me retiro");
                    }
                    elapsedTime += Time.deltaTime;
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
                
                case State.ATTACK_SHARK:
                    elapsedTime = 0;
                    arrive.target = blackboard.shark; //target gusano seleccionado
                    arrive.enabled = true;
                    break;
            }

            // EXIT STATE LOGIC. Depends on current state
            switch (currentState)
            {
                
                case State.ATTACK_SHARK:
                    elapsedTime = 0;
                    break;
            }

            currentState = newState;
        }
    }
}

