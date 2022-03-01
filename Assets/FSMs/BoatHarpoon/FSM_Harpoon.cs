using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steerings;

namespace FSM
{
    [RequireComponent(typeof(Arrive))]
    //[RequireComponent(typeof(Seek))]
    //[RequireComponent(typeof(HARPOON_Blackboard))]
    //[RequireComponent(typeof(FSM_BOAT_HARPOON))]
    public class FSM_Harpoon : FiniteStateMachine
    {
        public enum State
        {
            INITIAL,
            ATTACK_SHARK,
            PICK_HARPOON,
            DO_NOTHING
        };

        public State currentState = State.INITIAL;

        private HARPOON_Blackboard blackboard;
        private Arrive arrive;
        //private Seek seek;
        //private FSM_BOAT_HARPOON fsmBoatAndHarpoon;

        private float elapsedTime = 0.0f;

        // Start is called before the first frame update
        void Start()
        {
            //seek = GetComponent<Seek>();
            arrive = GetComponent<Arrive>();
            blackboard = GetComponentInParent<HARPOON_Blackboard>();
            //fsmBoatAndHarpoon = GetComponent<FSM_BOAT_HARPOON>();

            arrive.enabled = false;
            //seek.enabled = false;
            //fsmBoatAndHarpoon.Exit();
        }
        public override void Exit()
        {
            arrive.enabled = false;
            //seek.enabled = false;
            //fsmBoatAndHarpoon.Exit();

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
                    blackboard.harpoonPicked = false;
                    if (elapsedTime >= blackboard.timeAttack)
                    {                        
                        ChangeState(State.PICK_HARPOON);
                        Debug.Log("Me retiro");
                    }
                    elapsedTime += Time.deltaTime;
                    break;
                case State.PICK_HARPOON:
                    if (SensingUtils.DistanceToTarget(gameObject, blackboard.boat) <= blackboard.boatCloseEnoughtRadius)
                    {
                        blackboard.harpoonPicked = true;
                        ChangeState(State.ATTACK_SHARK);
                        break;
                    }
                    //elapsedTime += Time.deltaTime;
                    break;

            }
        }

        private void ChangeState(State newState)
        {
            // ENTER STATE LOGIC. Depends on newState
            switch (newState)
            {
                case State.ATTACK_SHARK:                    
                    //blackboard.canAttack = true;
                    //gameObject.transform.parent = null;
                    elapsedTime = 0;
                    //seek.target = blackboard.shark;
                    //seek.enabled = true;
                    arrive.target = blackboard.shark;
                    arrive.enabled = true;
                    break;
                case State.PICK_HARPOON:
                    elapsedTime = 0.0f;
                    //seek.target = blackboard.boat;                    
                    //seek.enabled = true;
                    arrive.target = blackboard.boat;
                    arrive.enabled = true;
                    break;

            }

            // EXIT STATE LOGIC. Depends on current state
            switch (currentState)
            {
                case State.ATTACK_SHARK:
                    //elapsedTime = 0;
                    blackboard.canAttack = false;
                    break;
                case State.PICK_HARPOON:
                    arrive.enabled = false;
                    //blackboard.canAttack = false;
                    break;
            }

            currentState = newState;
        }
    }
}

