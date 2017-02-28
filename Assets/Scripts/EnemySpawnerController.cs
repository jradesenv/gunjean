using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerController : MonoBehaviour {

    public float timeBetweenSpawns;
    private float timeBetweenSpawnsCounter;

    public EnemyController enemy;

	// Use this for initialization
	void Start () {
        timeBetweenSpawnsCounter = timeBetweenSpawns;
    }
	
	// Update is called once per frame
	void Update () {
        timeBetweenSpawnsCounter -= Time.deltaTime;
        if(timeBetweenSpawnsCounter <= 0)
        {
            Instantiate(enemy, transform.position, transform.rotation);
            timeBetweenSpawnsCounter = timeBetweenSpawns;
        }
	}
}
