using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public float spawnTime = 0.5f;
    private float startTime = 28f;
    public badguy bg;
    public Transform[] spawnPoints;
    public player target;
    private int chance;
    private int counter;
    public GameObject empty;
    private bool skip;
    // Start is called before the first frame update
    void Start()
    {
        skip = false;
        InvokeRepeating("Spawn", startTime, spawnTime);
        chance = 0;
        counter = 0;
    }

     void Update()
    {
        if(Input.GetKey("space") && !skip)
        {
            CancelInvoke();
            skip = true;
            InvokeRepeating("Spawn", spawnTime, spawnTime);
        }
    }

    void Spawn()
    {
        if (counter < 2)
        {
            counter++;
        } else {
            chance = Random.Range(0, 1);
            if (chance == 0 && !target.gameOver)
            {
                int spawnPointIndex = Random.Range(0, spawnPoints.Length);
                Instantiate(bg, spawnPoints[spawnPointIndex].transform);
                bg.empty = empty;
                bg.target = target;
            }
        }
    }

}
