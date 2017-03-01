using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerController : MonoBehaviour {

    public float minTimeBetweenSpawns;
    public float maxTimeBetweenSpawns;
    public EnemyController enemy;

    private float timeBetweenSpawnsCounter;
    private float lastSpawnTime;
    private UIManager uiManager;

    public int maxEnemiesAtSameTime;

	// Use this for initialization
	void Start () {
        ResetTimers();
        uiManager = FindObjectOfType<UIManager>();

        if(uiManager == null)
        {
            Debug.LogError("CANT FIND UIMANAGER");
        }
    }

    private void ResetTimers()
    {
        timeBetweenSpawnsCounter = GetRandomSpawnTime();
        lastSpawnTime = 0;
    }
	
	// Update is called once per frame
	void Update () {
        if (uiManager.isPlaying)
        {
            timeBetweenSpawnsCounter -= Time.deltaTime;
            if (timeBetweenSpawnsCounter <= 0)
            {
                if (CanSpawnMore())
                {
                    Spawn();
                }
            }
        } else
        {
            ResetTimers();
        }
	}

    bool CanSpawnMore()
    {
        return FindObjectsOfType<EnemyController>().Length < maxEnemiesAtSameTime;
    }

    void Spawn()
    {
        Instantiate(enemy, transform.position, transform.rotation);
        while (timeBetweenSpawnsCounter <= 0 || timeBetweenSpawnsCounter == lastSpawnTime)
        {
            timeBetweenSpawnsCounter = GetRandomSpawnTime();
        }
        lastSpawnTime = timeBetweenSpawnsCounter;
    }

    private float GetRandomSpawnTime()
    {
        return Random.Range(minTimeBetweenSpawns, maxTimeBetweenSpawns);
    }
}
