using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour {

    public float speed;
    public int minDamage;
    public int maxDamage;
    public float range;
    public string ownerID;

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

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    Debug.Log("BULLET COLLIDE");
    //}

    void OnCollisionEnter2D(Collision2D other)
    {
        BaseEntity atkedEntity = other.gameObject.GetComponent<BaseEntity>();
        if (atkedEntity)
        {
            if (atkedEntity.ID == ownerID)
            {
                //Debug.Log("collide on owner!");
                return;
            }

            AttackDTO atk = new AttackDTO();
            atk.damage = Random.Range(minDamage, maxDamage + 1);
            atk.type = Enums.Damage.Type.Normal;
            atkedEntity.Hit(atk);
        }

        Destroy(gameObject);
    }
}
