using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steerings;

namespace FSM
{
    //[RequireComponent(typeof(FSM_Boat))]
    //[RequireComponent(typeof(FSM_Harpoon))]
    [RequireComponent(typeof(SUBMARINE_MISSILE_Blackboard))]
    public class FSM_Submarine_Missile : FiniteStateMachine
    {
        public enum State
        {
            INITIAL, SUBMARINE_WANDER, ATTACK
        };

        public State currentState = State.INITIAL;
        private SUBMARINE_MISSILE_Blackboard blackboard;
        //private GameObject harpoon;

        private FSM_Missile fsmMissile;
        private FSM_Submarine fsmSubmarine;

        private float elapsedTime = 0.0f;

        
        // Start is called before the first frame update
        void Start()
        {
            blackboard = GetComponent<SUBMARINE_MISSILE_Blackboard>();
            fsmMissile = GetComponentInChildren<FSM_Missile>();
            fsmSubmarine = GetComponent<FSM_Submarine>();

            fsmMissile.Exit();
            fsmSubmarine.Exit();           
        }
        public override void Exit()
        {
            fsmMissile.Exit();
            fsmSubmarine.Exit();            
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
                    ChangeState(State.SUBMARINE_WANDER);
                    break;
                case State.SUBMARINE_WANDER:
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
                    if (/*blackboard.missileHided &&*/ blackboard.canAttack == false)
                    {
                        ChangeState(State.SUBMARINE_WANDER);
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
                case State.SUBMARINE_WANDER:
                    elapsedTime = 0.0f;
                    fsmSubmarine.ReEnter();
                    break;
                case State.ATTACK:
                    fsmMissile.ReEnter();
                    break;
            }

            // EXIT STATE LOGIC. Depends on current state
            switch (currentState)
            {
                case State.SUBMARINE_WANDER:
                    fsmSubmarine.Exit();
                    break;
                case State.ATTACK:
                    fsmMissile.Exit();
                    break;
            }
            currentState = newState;
        }
    }
}
