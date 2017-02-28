using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerController : MonoBehaviour {

    public float minTimeBetweenSpawns;
    public float maxTimeBetweenSpawns;
    public EnemyController enemy;

    private float timeBetweenSpawnsCounter;
    private float lastSpawnTime;

	// Use this for initialization
	void Start () {
        timeBetweenSpawnsCounter = GetRandomSpawnTime();
    }
	
	// Update is called once per frame
	void Update () {
        timeBetweenSpawnsCounter -= Time.deltaTime;
        if(timeBetweenSpawnsCounter <= 0)
        {
            Instantiate(enemy, transform.position, transform.rotation);
            while(timeBetweenSpawnsCounter <= 0 || timeBetweenSpawnsCounter == lastSpawnTime)
            {
                timeBetweenSpawnsCounter = GetRandomSpawnTime();
            }
            lastSpawnTime = timeBetweenSpawnsCounter;
        }
	}

    private float GetRandomSpawnTime()
    {
        return Random.Range(minTimeBetweenSpawns, maxTimeBetweenSpawns);
    }
}
