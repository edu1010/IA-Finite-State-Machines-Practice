using System;
using UnityEngine;
using FSM;
using UnityEngine.UI;

public class StateSpy : MonoBehaviour
{

    public FSM.FiniteStateMachine fsm;
    public string caption;
    public Text text;


    // Update is called once per frame
    void Update()
    {
        String stateName = null;

        if (fsm != null)
        {
            if (fsm.enabled)
            {
                // get the actual class of fsm and from it its current state
                // var a = fsm.GetType().GetProperty("currentState");
                var a = fsm.GetType().GetField("currentState");
                if (a != null)
                {
                    stateName = a.GetValue(fsm).ToString();

                }
                else
                {
                    stateName = "state unreachable";
                }
            }
            else
            {
                stateName = "not running";
            }
        }
        else
        {
            stateName = "FSM not attached";
        }
        if (text != null) text.text = caption + ": " + stateName;
        else Debug.Log(caption + ": " + stateName);
    }
}
