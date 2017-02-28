using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour {

    public float speed;
    public int minDamage;
    public int maxDamage;
    public float range;

    private Rigidbody2D myRigidbody;
    private Vector3 startPosition;

	// Use this for initialization
	void Start () {
        myRigidbody = GetComponent<Rigidbody2D>();
        startPosition = gameObject.transform.position;
    }
	
    void Update()
    {
        if (Vector3.Distance(startPosition, gameObject.transform.position) >= range)
        {
            Destroy(gameObject);
        }
    }

	// Update is called once per frame
    void FixedUpdate()
    {
        myRigidbody.AddForce(gameObject.transform.up * speed);
    }
}
