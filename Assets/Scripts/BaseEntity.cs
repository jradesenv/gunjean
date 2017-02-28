using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEntity : MonoBehaviour
{
    public float maxHP;
    public float currentHP;
    public float moveSpeed;
    public GunController myGun;
    public Sprite lookingUpSprite;
    public Sprite lookingDownSprite;
    public bool isMoving;
    public bool isFiring;

    protected Rigidbody2D myRigidbody;
    protected SpriteRenderer mySpriteRenderer;

    protected Vector3 targetPosition;
    protected float horizontalAxis;
    protected float verticalAxis;
    protected bool fireButtonGotDown;
    protected bool fireButtonGotUp;
    public Vector2 currentVelocity;
    protected AimHelper.AimAngle currentAimAngle;
    protected AimHelper.AimAngle lastAimAngle;

    public virtual void Start()
    {
        currentHP = maxHP;
        myRigidbody = GetComponent<Rigidbody2D>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    public virtual void Update()
    {
        UpdateInputs();
        UpdateMovement();
        UpdateAim();
        UpdateFire();
        UpdateSprites();
        UpdateCacheVariables();
        myRigidbody.velocity = currentVelocity;
    }

    public virtual void FixedUpdate()
    {
        
    }

    public virtual void UpdateInputs()
    {
    }

    public virtual void UpdateMovement()
    {
        //move
        if (horizontalAxis != 0 || verticalAxis != 0)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }

        if (isMoving)
        {
            var moveInput = new Vector2(horizontalAxis, verticalAxis);
            currentVelocity = moveInput * moveSpeed;
        } else
        {
            currentVelocity = Vector2.zero;
        }
    }

    public virtual void UpdateAim()
    {
        //aim
        Quaternion rotation = Quaternion.LookRotation(myGun.transform.position
             - targetPosition, Vector3.forward);

        myGun.transform.rotation = rotation;
        myGun.transform.eulerAngles = new Vector3(0, 0, myGun.transform.eulerAngles.z);
    }

    public virtual void UpdateFire()
    {
        //fire
        if (fireButtonGotDown)
        {
            myGun.isFiring = true;
            isFiring = true;
        }
        else if (fireButtonGotUp)
        {
            myGun.isFiring = false;
            isFiring = false;
        }
    }

    private void UpdateSprites()
    {
        currentAimAngle = AimHelper.GetCurrentAimAngle(targetPosition, transform.position);

        if (currentAimAngle != lastAimAngle)
        {
            if (currentAimAngle.y == AimHelper.YAngle.Top)
            {
                mySpriteRenderer.sprite = lookingUpSprite;
            }
            else
            {
                mySpriteRenderer.sprite = lookingDownSprite;
            }

            if (currentAimAngle.x == AimHelper.XAngle.Left)
            {
                myGun.SetBehindEntity(true, mySpriteRenderer);
            }
            else
            {
                myGun.SetBehindEntity(false, mySpriteRenderer);
            }

            if (lastAimAngle != null && currentAimAngle.x != lastAimAngle.x)
            {
                Flip();
            }
        }
    }

    public virtual void UpdateCacheVariables()
    {
        lastAimAngle = currentAimAngle;
    }

    private void Flip()
    {
        Vector3 newScale = gameObject.transform.localScale;
        newScale.x *= -1;
        gameObject.transform.localScale = newScale;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        BulletController bullet = other.gameObject.GetComponent<BulletController>();
        if (bullet)
        {
            currentHP -= Random.Range(bullet.minDamage, bullet.maxDamage + 1);
            Destroy(other.gameObject);

            if (currentHP <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
