using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionController : MonoBehaviour {

    public int minDamage;
    public int maxDamage;
    public float explosionRate = 1f;
    public float explosionMaxSize = 10f;
    public float currentRadius = 0f;
    public string ownerID;

    public bool exploded = false;

    CircleCollider2D explosionRadius;

    // Use this for initialization
    void Start () {
        explosionRadius = gameObject.GetComponent<CircleCollider2D>();
    }

    private void FixedUpdate()
    {
        if(exploded)
        {
            if (currentRadius < explosionMaxSize)
            {
                currentRadius += explosionRate;
            } else
            {
                Object.Destroy(this.gameObject.transform.parent.gameObject);
            }

            explosionRadius.radius = currentRadius;
        }
    }

    //private void OnTriggerEnter2D(Collider2D other)
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (exploded == true)
        {
            Rigidbody2D otherRigidBody = other.gameObject.GetComponent<Rigidbody2D>();
            if (otherRigidBody != null)
            {
                Vector2 target = other.gameObject.transform.position;
                Vector2 explosion = gameObject.transform.position;

                Vector2 direction = 70f * (target - explosion);

               // otherRigidBody.AddForce(direction);

                BaseEntity atkedEntity = other.gameObject.GetComponent<BaseEntity>();
                if (atkedEntity)
                {
                    if (atkedEntity.ID == ownerID)
                    {
                        return;
                    } else
                    {
                        AttackDTO atk = new AttackDTO();
                        atk.damage = Random.Range(minDamage, maxDamage + 1);
                        atk.type = Enums.Damage.Type.Explosion;
                        atkedEntity.Hit(atk);
                    }
                }
            }
        }
    }

}
