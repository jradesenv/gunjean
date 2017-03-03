using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour {

    public float speed;
    public int minDamage;
    public int maxDamage;
    public float range;
    public string ownerID;

    protected Rigidbody2D myRigidbody;
    protected Vector3 startPosition;

	// Use this for initialization
	protected virtual void Start () {
        myRigidbody = GetComponent<Rigidbody2D>();
        startPosition = gameObject.transform.position;
    }
	
    protected virtual void Update()
    {
        if (Vector3.Distance(startPosition, gameObject.transform.position) >= range)
        {
            OnRangeTraveled();
        }
    }

    protected virtual void OnRangeTraveled()
    {
        Destroy(gameObject);
    }

    // Update is called once per frame
    protected virtual void FixedUpdate()
    {
        myRigidbody.AddForce(gameObject.transform.up * speed);
    }

    protected virtual void OnCollisionEnter2D(Collision2D other)
    {
        BaseEntity atkedEntity = other.gameObject.GetComponent<BaseEntity>();
        if (atkedEntity)
        {
            if (atkedEntity.ID == ownerID)
            {
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
