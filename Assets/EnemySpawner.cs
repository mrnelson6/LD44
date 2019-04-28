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
    private int counter;
    public GameObject empty;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("Spawn", spawnTime, spawnTime);
        chance = 0;
        counter = 0;
        badguys = new List<badguy>();
    }

    void Spawn()
    {
        if (counter < 2)
        {
            counter++;
        } else {
            chance = Random.Range(0, 3);
            if (chance == 0 && !target.gameOver)
            {
                int spawnPointIndex = Random.Range(0, spawnPoints.Length);
                Instantiate(bg, spawnPoints[0].transform);
                badguys.Add(bg);
                bg.empty = empty;
                bg.target = target;
            }
        }
    }

}
