using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace FSM
{
    //[RequireComponent(typeof(FSM_Boat))]
    //[RequireComponent(typeof(FSM_Harpoon))]
    [RequireComponent(typeof(HARPOON_Blackboard))]
    public class FSM_BOAT_HARPOON : FiniteStateMachine
    {
        public enum State
        {
            INITIAL, BOAT_WANDER, ATTACK
        };

        public State currentState = State.INITIAL;
        private HARPOON_Blackboard blackboard;
        private GameObject harpoon;

        private FSM_Harpoon fsmHarpoon;
        private FSM_Boat fsmBoat;

        private float elapsedTime = 0.0f;

        // Start is called before the first frame update
        void Start()
        {
            blackboard = GetComponent<HARPOON_Blackboard>();
            fsmHarpoon = GetComponentInChildren<FSM_Harpoon>();
            fsmBoat = GetComponent<FSM_Boat>();

            fsmHarpoon.Exit();
            fsmBoat.Exit();
            //fsmHarpoon.enabled = false;
            //fsmBoat.enabled = false;
        }
        public override void Exit()
        {
            fsmHarpoon.Exit();
            fsmBoat.Exit();
            //fsmHarpoon.enabled = false;
            //fsmBoat.enabled = false;
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
                    ChangeState(State.BOAT_WANDER);
                    break;
                case State.BOAT_WANDER:
                    if (elapsedTime >= blackboard.timeToAttackAgain)
                    {
                        blackboard.canAttack = true;
                    }

                    if (SensingUtils.DistanceToTarget(gameObject, blackboard.shark) <= blackboard.sharkDetectableRadious && blackboard.canAttack)
                    {
                        ChangeState(State.ATTACK);
                        break;
                    }
                    elapsedTime += Time.deltaTime;
                    break;
                case State.ATTACK:
                    if (blackboard.harpoonPicked && blackboard.canAttack == false)
                    {
                        ChangeState(State.BOAT_WANDER);
                        break;
                    }
                    break;
            }
        }

        private void ChangeState(State newState)
        {
            // ENTER STATE LOGIC. Depends on newState
            switch (newState)
            {
                case State.BOAT_WANDER:
                    elapsedTime = 0.0f;
                    fsmBoat.ReEnter();
                    break;
                case State.ATTACK:
                    fsmHarpoon.ReEnter();
                    break;
            }

            // EXIT STATE LOGIC. Depends on current state
            switch (currentState)
            {
                case State.BOAT_WANDER:
                    fsmBoat.Exit();
                    break;
                case State.ATTACK:
                    fsmHarpoon.Exit();
                    break;
            }
            currentState = newState;
        }
    }
}
