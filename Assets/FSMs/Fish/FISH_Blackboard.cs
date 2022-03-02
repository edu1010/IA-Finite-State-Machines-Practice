using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steerings;
public class FISH_Blackboard : MonoBehaviour
{
    public GameObject shark;
    public float maxDistanceToShark = 20f;
    public float WhenIsfeed = 10f;
    public float eatPlnktonValue = 1f;
    public float timeFloking = 4f;
    public GameObject atractorA;
    public GameObject atractorB;
    public float foodDetectableRadius;
    public float foodReachedRadius=2f;
    public float timeToEat = 2f;
    public float incrementSeekWeight = 0.2f;
    public float frecuencyIncrementWeight = 0.5f;
    public float maxDistanceAtractor = 40;
    public float minWeight = 0.2f;
    public float radiusNearTortoise = 20f;
    public int maxFishInTortoise = 5;
    public float maxTimeSearchAnemona = 4f;
    public float currentHungry;
    public float maxWeitghWander = 0.6f;
    public bool isHiding = false;
    public float maxTimeHiding = 2;
    public float stopFlee = 70f;
    public string tagFishHide = "FISH_HIDE";
    public string tagFisH = "FISH";
    public string tagTurtle= "TORTOISE";
    public float speedMultiplayer = 7;
    public float generalReachedRadius = 10f;
    public GameObject[] targets;
    public List<GameObject> avalibleTargets;
    public List<GameObject> donTargets;
    public GameObject anemona;
    private void Awake()
    {
        targets = GameObject.FindGameObjectsWithTag(tagTurtle);
        anemona = SensingUtils.FindInstance(gameObject, "ANEMONA");
    }
    public float ChangeWeightWander(WanderAround wanderAround, float elapsedTime )
    {
        if (SensingUtils.DistanceToTarget(gameObject, wanderAround.attractor) >= maxDistanceAtractor)
        {
            if (elapsedTime > frecuencyIncrementWeight)
            {
                elapsedTime = 0f;
                wanderAround.SetSeekWeight(wanderAround.seekWeight + incrementSeekWeight);
            }
        }
        else
        {
            if (elapsedTime > frecuencyIncrementWeight)
            {
                elapsedTime = 0f;
                wanderAround.SetSeekWeight(wanderAround.seekWeight - incrementSeekWeight);
                
                if (wanderAround.seekWeight < minWeight)
                {
                    wanderAround.SetSeekWeight(minWeight);
                }
                if (wanderAround.seekWeight > maxWeitghWander)
                {
                    wanderAround.SetSeekWeight(maxWeitghWander);
                }
            }
        }
        return elapsedTime;
    }
    public void IncrementHungry()
    {
        currentHungry += eatPlnktonValue;
    }
    public void DecrementHungry()
    {
        currentHungry -= eatPlnktonValue;
    }
    
    public GameObject GetNearTurtleAvalible(Transform me)
    {
        GameObject nearTurtle = targets[0];
        bool isAllTurtlesFull = false;
        for (int i = 1; i < targets.Length; i++)
        {
            Debug.Log("lo "+i+" " + targets[i].transform.childCount);
            if (targets[i].transform.childCount < maxFishInTortoise)
            {
                if (avalibleTargets.Find(x => x = targets[i]) == null)
                {
                    avalibleTargets.Add(targets[i]);
                    if (donTargets.Find(x => x = targets[i]) != null)
                    {
                        donTargets.Remove(targets[i]);
                    }

                }
                isAllTurtlesFull = true;
                if (( targets[i].transform.position - me.position ).magnitude <
                    (nearTurtle.transform.position - me.position).magnitude
                    )
                {
                    nearTurtle = targets[i];
                }

            }
            else
            {
                if (avalibleTargets.Find(x => x = targets[i]) != null)
                {
                    avalibleTargets.Remove(targets[i]);
                }

                if (donTargets.Find(x => x= targets[i]) == null)
                {
                    donTargets.Add(targets[i]);
                }
                   
                
            }
        }
        if (isAllTurtlesFull)
        {
            return nearTurtle;
        }
        else
        {
           
            return null;
        }
    }
}
