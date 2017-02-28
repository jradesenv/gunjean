using System;
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
    public string ID;

    public Dictionary<Enums.Items.Type, float> inventory;

    public virtual void Start()
    {
        ID = generateID();
        currentHP = maxHP;
        myRigidbody = GetComponent<Rigidbody2D>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        myGun.ownerID = ID;
        inventory = new Dictionary<Enums.Items.Type, float>();
    }

    public string generateID()
    {
        return System.Guid.NewGuid().ToString("N");
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
        }
        else
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
            if (currentAimAngle.y == Enums.Angles.Y.Top)
            {
                mySpriteRenderer.sprite = lookingUpSprite;
            }
            else
            {
                mySpriteRenderer.sprite = lookingDownSprite;
            }

            if (currentAimAngle.x == Enums.Angles.X.Left)
            {
                myGun.SetBehindEntity(true, mySpriteRenderer);
            }
            else
            {
                myGun.SetBehindEntity(false, mySpriteRenderer);
            }

            if ((lastAimAngle == null && currentAimAngle.x == Enums.Angles.X.Left) || (lastAimAngle != null && currentAimAngle.x != lastAimAngle.x))
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

    public void Hit(AttackDTO atk)
    {
        //calc the damage reducer by damage type defense etc
        float finalDamage = atk.damage;
        currentHP -= finalDamage;

        LogWithHP("Was hit by", finalDamage);

        if (currentHP <= 0)
        {
            Dead();
        }
    }

    public void Dead()
    {
        BeforeDead();
        Destroy(gameObject);
    }

    protected virtual void BeforeDead()
    {

    }

    public void Collect(BaseItem item)
    {
        if (item.collectType == Enums.Items.CollectType.Collect)
        {
            if (!inventory.ContainsKey(item.type))
            {
                inventory.Add(item.type, item.quantity);
            }
            else
            {
                inventory[item.type] += item.quantity;
            }
        }
        else if (item.collectType == Enums.Items.CollectType.Instant)
        {
            Use(item);
        }
    }

    public void Use(BaseItem item)
    {
        switch (item.type)
        {
            case Enums.Items.Type.Health:
                Heal(item.quantity);
                break;
            default:
                break;
        }
    }

    public void Heal(float quantity)
    {        
        if (currentHP + quantity > maxHP)
        {
            currentHP = maxHP;
        } else
        {
            currentHP += quantity;
        }

        LogWithHP("Healed", quantity);
    }

    private void LogWithHP(string action, float quantity)
    {
        Debug.Log("Entity " + ID + " " + action + " " + quantity + " - CURRENT HP: " + currentHP);
    }
}
