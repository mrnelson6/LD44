using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private float spawnTime = 1.0f;
    public badguy bg;
    public Transform[] spawnPoints;
    public player target;
    private int chance;
    private int counter;
    public GameObject empty;
    private bool skip;
    private float time;
    // Start is called before the first frame update
    void Start()
    {
        time = 0.0f;
        skip = false;
        chance = 0;
        counter = 0;
    }

     void Update()
    {
        time+= Time.deltaTime;
        if (Input.GetKey("space") && !skip)
        {
            //CancelInvoke();
            skip = true;
            InvokeRepeating("Spawn", spawnTime, spawnTime);
        }
        if(time > 28f && !skip)
        {
            InvokeRepeating("Spawn", spawnTime, spawnTime);
            skip = true;
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
