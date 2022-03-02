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
        private GameObject hideout;
        private GameObject submarine;

        private float elapsedTime = 0.0f;
        private float missileElapsedTime = 0.0f;

        // Start is called before the first frame update
        void Awake()
        {
            submarine = GameObject.FindGameObjectWithTag("SUBMARINE");
            arrive = GetComponent<Arrive>();
            blackboard = submarine.GetComponent<SUBMARINE_MISSILE_Blackboard>();

            arrive.enabled = false;
        }
        public override void Exit()
        {
            blackboard.canAttack = false;
            arrive.enabled = false;
            base.Exit();
        }

        public override void ReEnter()
        {
            currentState = State.INITIAL;
            //GetComponent<KinematicState>().transform.parent = null;
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
                    //hideout = SensingUtils.FindInstanceWithinRadius(gameObject, "HIDEOUT", blackboard.missileHideoutDetection);
                    
                    if (elapsedTime >= blackboard.timeAttack) //mejor mirar si se sale d la pantalla
                    {
                        ChangeState(State.HIDE_MISSILE);
                        break;
                    }
                    
                    if(SensingUtils.DistanceToTarget(gameObject, blackboard.shark) <= blackboard.missileRadious)
                    {
                        blackboard.sharkAttacked -= 1;
                        if(blackboard.sharkAttacked == 2)
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
                            break;
                        }
                        
                    }
                    /*
                    if (SensingUtils.DistanceToTarget(gameObject, hideout) <= blackboard.missileCloseToHideout)
                    {
                        ChangeState(State.HIDE_MISSILE);
                        //Debug.Log("paro por el hideout");
                        break;
                    }*/
                    elapsedTime += Time.deltaTime;
                    break;
                case State.HIDE_MISSILE:
                    /*
                    if (SensingUtils.DistanceToTarget(gameObject, blackboard.submarine) <= blackboard.submarineCloseEnoughtRadius)
                    {
                        ChangeState(State.ATTACK_SHARK);
                        break;
                    }
                    */
                    break;

            }
        }

        private void ChangeState(State newState)
        {
            // ENTER STATE LOGIC. Depends on newState
            switch (newState)
            {
                case State.ATTACK_SHARK:
                    elapsedTime = 0.0f;
                    arrive.target = blackboard.shark;
                    arrive.enabled = true;
                    break;
                case State.HIDE_MISSILE:
                    blackboard.canAttack = true;
                    GetComponent<KinematicState>().transform.position = submarine.GetComponent<KinematicState>().transform.position;
                    break;
            }

            // EXIT STATE LOGIC. Depends on current state
            switch (currentState)
            {
                case State.ATTACK_SHARK:
                    arrive.enabled = false;
                    blackboard.canTakeDamage = false;
                    break;
                case State.HIDE_MISSILE:
                    break;
            }
            currentState = newState;
        }
    }
}
