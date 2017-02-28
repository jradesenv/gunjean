using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : BaseEntity
{
    private bool isTargetInRange;
    private bool hasTarget;

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
}