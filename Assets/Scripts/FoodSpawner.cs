using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    public GameObject prefab;
    float time;
    float resetTimer = 0f;
    public float frequency;
    public float range;
    public int maxNumberOfFoodInTime = 50;
    public float timeToReset = 20f;
    private int currentNumberOfFood = 0 ;

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (time >= frequency)
        {
            currentNumberOfFood++;
            if (currentNumberOfFood < maxNumberOfFoodInTime)
            {
                CreateObject();
            }
            else
            {
                resetTimer += Time.deltaTime;
                if(resetTimer > timeToReset)
                {
                    currentNumberOfFood = 0;
                    resetTimer = 0f;
                }
            }
            time = 0f;
        }
    }
    public void CreateObject()
    {
        Vector2 SpawnPos = Vector2.zero;
        SpawnPos = (Vector2)transform.position + Random.insideUnitCircle* range;
        Instantiate(prefab, SpawnPos, Quaternion.identity);
    }
}
