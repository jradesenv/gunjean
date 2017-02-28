using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyController : BaseEntity
{
    public List<BaseItemEntity> possibleLoot;

    private bool isTargetInRange;
    private bool hasTarget;

    public override void Start()
    {
        base.Start();

        if (possibleLoot == null)
        {
            possibleLoot = new List<BaseItemEntity>();
        }

        possibleLoot = possibleLoot.OrderBy(item => (int)item.rarity).ToList();
    }

    public override void UpdateInputs()
    {
        UpdateTarget();

        if (hasTarget)
        {
            if (isTargetInRange)
            {
                isMoving = false;
                fireButtonGotUp = false;
                if (!isFiring)
                {
                    fireButtonGotDown = true;
                }
                else
                {
                    fireButtonGotDown = false;
                }
            }
            else
            {
                isMoving = true;
                fireButtonGotDown = false;
                if (isFiring)
                {
                    fireButtonGotUp = false;
                }
                else
                {
                    fireButtonGotUp = false;
                }
            }
        }
        else
        {
            fireButtonGotDown = false;
            if (isFiring)
            {
                fireButtonGotUp = true;
            }
            else
            {
                fireButtonGotUp = false;
            }
        }

        base.UpdateInputs();
    }

    public void UpdateTarget()
    {
        GameObject closestPlayer = RangeHelper.GetClosestWithTag(gameObject, Constants.playerTag);
        if (closestPlayer != null)
        {
            hasTarget = true;
            isTargetInRange = RangeHelper.CheckIfTargetIsInRange(gameObject, closestPlayer, myGun.bulletRange);
            targetPosition = closestPlayer.transform.position;
        }
        else
        {
            hasTarget = false;
            isTargetInRange = false;
        }
    }

    public override void UpdateMovement()
    {
        if (!hasTarget || isTargetInRange)
        {
            //stop moving
            horizontalAxis = 0;
            verticalAxis = 0;
        }
        else
        {
            //move towards thé player
            if (targetPosition.x == transform.position.x)
            {
                horizontalAxis = 0;
            }
            else if (targetPosition.x < transform.position.x)
            {
                horizontalAxis = -1;
            }
            else
            {
                horizontalAxis = +1;
            }

            if (targetPosition.y == transform.position.y)
            {
                verticalAxis = 0;
            }
            else if (targetPosition.y < transform.position.y)
            {
                verticalAxis = -1;
            }
            else
            {
                verticalAxis = +1;
            }
        }

        base.UpdateMovement();
    }

    protected override void BeforeDead()
    {
        BaseItemEntity lootItem = GetRandomLoot();
        if (lootItem != null)
        {
            Instantiate(lootItem, gameObject.transform.position, Quaternion.identity);
        }       
    }

    private BaseItemEntity GetRandomLoot()
    {
        var range = 0;
        for (var i = 0; i < possibleLoot.Count; i++)
            range += (int)possibleLoot[i].rarity;

        range += range; //just so it can drop nothing
        var rand = Random.Range(0, range);
        var top = 0;

        for (var i = 0; i < possibleLoot.Count; i++)
        {
            top += (int)possibleLoot[i].rarity;
            if (rand < top)
            {
                return possibleLoot[i];
            }
        }

        return null;
    }
}