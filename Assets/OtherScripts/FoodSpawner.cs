using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    public GameObject prefab;
    float time;
    public float frequency;
    public float range;

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (time >= frequency)
        {
            CreateObject();
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
