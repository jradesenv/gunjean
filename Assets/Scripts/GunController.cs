using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{

    public bool isFiring;
    public Enums.Layers BulletLayer;
    public string gunName;

    public BulletController bullet;
    public float bulletSpeed;
    public float bulletRange;
    public int bulletMinDamage;
    public int bulletMaxDamage;

    public float timeBetweenShots;
    private float shotCounter;
    private SpriteRenderer mySpriteRenderer;
    public Transform firePoint;

    public string ownerID;

    // Use this for initialization
    void Start()
    {
        shotCounter = 0;
        mySpriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (shotCounter > 0)
        {
            shotCounter -= Time.deltaTime;
        }
        else if (shotCounter <= 0)
        {
            if (isFiring)
            {
                shotCounter = timeBetweenShots;
                BulletController newBullet = Instantiate(bullet, firePoint.position, transform.rotation);
                newBullet.speed = bulletSpeed;
                newBullet.minDamage = bulletMinDamage;
                newBullet.maxDamage = bulletMaxDamage;
                newBullet.range = bulletRange;
                newBullet.ownerID = ownerID;
                newBullet.gameObject.layer = (int)BulletLayer;
            }
        }
    }

    public void UpdateFlip(bool shouldFlip)
    {
        mySpriteRenderer.flipX = shouldFlip; ;
    }

    public void SetBehindEntity(bool check, SpriteRenderer entitySpriteRenderer)
    {
        if (check)
        {
            mySpriteRenderer.sortingOrder = entitySpriteRenderer.sortingOrder - 1;
        }
        else
        {
            mySpriteRenderer.sortingOrder = entitySpriteRenderer.sortingOrder + 1;
        }
    }
}
