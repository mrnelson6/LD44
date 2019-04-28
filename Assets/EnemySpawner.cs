using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public float spawnTime = 5f;
    public badguy bg;
    public Transform[] spawnPoints;
    public player target;
    private List<badguy> badguys;
    private int chance;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("Spawn", spawnTime, spawnTime);
        chance = 0;
        badguys = new List<badguy>();
    }

    void Spawn()
    {
        chance = Random.Range(0, 3);
        chance = 0;
        if (chance == 0 && badguys.Count < 50)
        {
            int spawnPointIndex = Random.Range(0, spawnPoints.Length);
            Instantiate(bg, spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);
           Transform a = bg.transform;
            badguys.Add(bg);
            bg.target = target;
        }
    }

}
