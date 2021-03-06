using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steerings;

namespace FSM
{
    [RequireComponent(typeof(Arrive))]
    public class FSM_Missile : FiniteStateMachine
    {
        public enum State
        {
            INITIAL,
            ATTACK_SHARK,
            HIDE_MISSILE
        };

        public State currentState = State.INITIAL;

        private SUBMARINE_MISSILE_Blackboard blackboard;
        private Arrive arrive;        
        private GameObject submarine;

        private SpriteRenderer sprite;

        // Start is called before the first frame update
        void Awake()
        {
            submarine = GameObject.FindGameObjectWithTag("SUBMARINE");
            arrive = GetComponent<Arrive>();
            blackboard = submarine.GetComponent<SUBMARINE_MISSILE_Blackboard>();
            sprite = GetComponentInChildren<SpriteRenderer>();

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
            sprite.enabled = true;
            GetComponent<KinematicState>().transform.parent = null;
            GetComponent<KinematicState>().position = submarine.GetComponent<KinematicState>().position;
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
                    if (blackboard.shark.GetComponent<SHARK_Blackboard>().IsHided)
                    {
                        ChangeState(State.HIDE_MISSILE);
                        break;
                    }

                    if (SensingUtils.DistanceToTarget(gameObject, blackboard.shark) <= blackboard.missileRadious)
                    {
                        blackboard.sharkAttacked -= 1;
                        if (blackboard.sharkAttacked == 2)
                        {
                            Destroy(blackboard.sharkLifes[2]);
                            ChangeState(State.HIDE_MISSILE);
                            break;
                        }
                        if (blackboard.sharkAttacked == 1)
                        {
                            Destroy(blackboard.sharkLifes[1]);
                            ChangeState(State.HIDE_MISSILE);
                            break;
                        }
                        if (blackboard.sharkAttacked == 0)
                        {
                            Destroy(blackboard.sharkLifes[0]);
                            ChangeState(State.HIDE_MISSILE);
                            FindObjectOfType<GameManager>().LoseGame();
                            Time.timeScale = 0;

                            break;
                        }

                    }                    
                    break;
                case State.HIDE_MISSILE:                   
                    break;

            }
        }

        private void ChangeState(State newState)
        {
            // ENTER STATE LOGIC. Depends on newState
            switch (newState)
            {
                case State.ATTACK_SHARK:                    
                    arrive.target = blackboard.shark;
                    arrive.enabled = true;
                    break;
                case State.HIDE_MISSILE:                    
                    arrive.enabled = false;
                    GetComponent<KinematicState>().position = submarine.GetComponent<KinematicState>().position;
                    gameObject.transform.position = submarine.transform.position;
                    gameObject.transform.parent = submarine.transform;
                    sprite.enabled = false;                    
                    break;
            }

            // EXIT STATE LOGIC. Depends on current state
            switch (currentState)
            {
                case State.ATTACK_SHARK:
                    arrive.enabled = false;
                    blackboard.canTakeDamage = false;
                    blackboard.canAttack = false;
                    break;
                case State.HIDE_MISSILE:
                    break;
            }
            currentState = newState;
        }
    }
}
