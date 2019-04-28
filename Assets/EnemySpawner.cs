using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public float spawnTime = 3f;
    public badguy bg;
    public Transform[] spawnPoints;
    public Transform target;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("Spawn", spawnTime, spawnTime);
    }

    void Spawn()
    {
        float chance = Mathf.FloorToInt(Random.Range(0.0f, 10.0f));
        chance = 0;
        if (chance == 0)
        {
            int spawnPointIndex = Random.Range(0, spawnPoints.Length);
            Instantiate(bg, spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);
            bg.target = target;
        }
    }

}
