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
    public class FSM_Missile : FiniteStateMachine
    {
        public enum State
        {
            INITIAL,
            RESET_POS,
            ATTACK_SHARK,
            HIDE_MISSILE
        };

        public State currentState = State.INITIAL;

        private SUBMARINE_MISSILE_Blackboard blackboard;
        private Arrive arrive;

        private float elapsedTime = 0.0f;
        private float missileElapsedTime = 0.0f;

        // Start is called before the first frame update
        void Start()
        {
            arrive = GetComponent<Arrive>();
            blackboard = GetComponentInParent<SUBMARINE_MISSILE_Blackboard>();

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
                case State.RESET_POS:
                    if (SensingUtils.DistanceToTarget(gameObject, blackboard.submarine) <= blackboard.submarineCloseEnoughtRadius)
                    {
                        transform.position = blackboard.submarine.transform.position;
                        Debug.Log("hide missile");
                        //blackboard.missileHided = true;
                        ChangeState(State.ATTACK_SHARK);
                        break;
                    }
                    missileElapsedTime += Time.deltaTime;
                    break;
                case State.ATTACK_SHARK:
                    Debug.Log("attack shark");
                    if (elapsedTime >= blackboard.timeAttack)
                    {
                        ChangeState(State.HIDE_MISSILE);
                        Debug.Log("Me retiro");
                    }
                    if(SensingUtils.DistanceToTarget(gameObject, blackboard.shark) <= blackboard.missileRadious)
                    {
                        blackboard.sharkAttacked -= 1;
                        Debug.Log("Tiburon Atacado!!!!!!!!!!!!!!!!!!!!!!");
                        if(blackboard.sharkAttacked == 2)
                        {
                            Destroy(blackboard.sharkLifes[2]);
                            ChangeState(State.HIDE_MISSILE);
                        }
                        if (blackboard.sharkAttacked == 1)
                        {
                            Destroy(blackboard.sharkLifes[1]);
                            ChangeState(State.HIDE_MISSILE);
                        }
                        if (blackboard.sharkAttacked == 0)
                        {
                            Destroy(blackboard.sharkLifes[0]);
                            ChangeState(State.HIDE_MISSILE);
                        }
                        
                    }
                    elapsedTime += Time.deltaTime;
                    break;
                case State.HIDE_MISSILE:
                    ChangeState(State.RESET_POS);
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
                    //gameObject.SetActive(true);
                    elapsedTime = 0;
                    arrive.target = blackboard.shark;
                    arrive.enabled = true;
                    break;
                case State.HIDE_MISSILE:
                    //gameObject.SetActive(false);
                    transform.position = blackboard.submarine.transform.position;
                    elapsedTime = 0.0f;
                    gameObject.transform.position = blackboard.submarine.transform.position;
                    gameObject.transform.rotation = blackboard.submarine.transform.rotation;
                    arrive.target = blackboard.submarine;
                    arrive.enabled = true;
                    break;
                case State.RESET_POS:
                    //gameObject.SetActive(true);
                    missileElapsedTime = 0;
                    arrive.enabled = false;
                    break;

            }

            // EXIT STATE LOGIC. Depends on current state
            switch (currentState)
            {
                case State.ATTACK_SHARK:
                    blackboard.canAttack = false;
                    arrive.enabled = false;
                    blackboard.canTakeDamage = false;
                    break;
                case State.HIDE_MISSILE:
                    arrive.enabled = false;
                    break;
            }
            currentState = newState;
        }
    }
}
