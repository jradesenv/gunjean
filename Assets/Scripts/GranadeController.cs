using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GranadeController : BulletController
{
    public ExplosionController explosion;

    protected override void Start()
    {
        base.Start();

        explosion.ownerID = this.ownerID;
        explosion.minDamage = this.minDamage;
        explosion.maxDamage = this.maxDamage;
        explosion.gameObject.layer = this.gameObject.layer;
    }

    protected override void OnRangeTraveled()
    {
        explosion.exploded = true;
    }

    protected override void OnCollisionEnter2D(Collision2D other)
    {
        BaseEntity atkedEntity = other.gameObject.GetComponent<BaseEntity>();
        if (atkedEntity)
        {
            if (atkedEntity.ID == ownerID)
            {
                return;
            }
        }

        explosion.exploded = true;
    }
}
