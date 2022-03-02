using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideOutTurtleController : MonoBehaviour
{
    public static List<GameObject> avalibleTargets;
    public string tagTurtle = "TORTOISE";
    static HideOutTurtleController hideOutTurtleController = null;
    // Start is called before the first frame update
    void Awake()
    {
        if (hideOutTurtleController == null)
        {
            hideOutTurtleController = this;
            GameObject.DontDestroyOnLoad(gameObject);
        }
        else
        {
            GameObject.Destroy(this); // ya existe, no hace falta crearla
        }
        avalibleTargets = new List<GameObject>(GameObject.FindGameObjectsWithTag(tagTurtle));
    }
    public static GameObject GetNearTurtleAvalible(Transform me)
    {
        
        if (avalibleTargets.Count <= 0)
        {
            return null;
        }
        GameObject near = avalibleTargets[0];
        for (int i = 0; i < avalibleTargets.Count; i++)
        {
            if ((avalibleTargets[i].transform.position - me.position).magnitude <
                  (near.transform.position - me.position).magnitude)
            {
                near = avalibleTargets[i];
            }

        }
        avalibleTargets.Remove(near);
        return near;
    }
    public static void AddAvalibleTarget(GameObject hide)
    {
        if (hide.tag.Equals(hideOutTurtleController.tagTurtle))
        {
            avalibleTargets.Add(hide);
        }
    }
    public static void RemoveTarget(GameObject toRemove)
    {
        avalibleTargets.Remove(toRemove);
    }


}
