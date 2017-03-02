using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyController : BaseEntity
{
    public List<BaseItemEntity> possibleLoot;
    public float minTimeBetweenMessages;
    public float maxTimeBetweenMessages;

    private bool isTargetInRange;
    private bool hasTarget;

    private float timeBetweenMessagesCounter;
    private EnemyMessage lastMessage;

    public class EnemyMessage
    {
        public Enums.EnemyMessagesRarity rarity;
        public string message;
    }

    private List<EnemyMessage> possibleMessages;

    public void ResetMessageTimer()
    {
        timeBetweenMessagesCounter = Random.Range(minTimeBetweenMessages, maxTimeBetweenMessages + 1);
    }

    public override void Start()
    {
        base.Start();

        if (possibleLoot == null)
        {
            possibleLoot = new List<BaseItemEntity>();
        }

        if (possibleMessages == null)
        {
            possibleMessages = new List<EnemyMessage>()
                {
                new EnemyMessage()
                    {
                        rarity = Enums.EnemyMessagesRarity.Common,
                        message = "Tu vai desce num saco!"
                    },
                new EnemyMessage()
                    {
                        rarity = Enums.EnemyMessagesRarity.Common,
                        message = "Passa o ferro nele!"
                    },
                new EnemyMessage()
                    {
                        rarity = Enums.EnemyMessagesRarity.Common,
                        message = "Perdeu, perdeu!"
                    },
                    new EnemyMessage() {
                        rarity = Enums.EnemyMessagesRarity.LessCommon,
                        message = "Passa dois real!"
                    },
                    new EnemyMessage()
                    {
                        rarity = Enums.EnemyMessagesRarity.Rare,
                        message = "Ae mlk, passa tudo!"
                    },
                    new EnemyMessage()
                    {
                        rarity = Enums.EnemyMessagesRarity.Rare,
                        message = "Iiiiih óh o cara ai!"
                    },
                    new EnemyMessage()
                    {
                        rarity = Enums.EnemyMessagesRarity.LessCommon,
                        message = "Ta de palhaçada playboy?"
                    },
                    new EnemyMessage()
                    {
                        rarity = Enums.EnemyMessagesRarity.Rare,
                        message = "Que isso cumpadi?"
                    }
                };
        }

        possibleLoot = possibleLoot.OrderBy(item => (int)item.rarity).ToList();
        possibleMessages = possibleMessages.OrderBy(message => (int)message.rarity).ToList();
    }

    public override void UpdateAim()
    {
        //aim
        float angle = Vector3.Angle(Vector3.up, targetPosition - transform.position);
        angle *= gameObject.transform.localScale.x * -1;
       // Debug.Log("angle: " + angle);
        //float angle = Mathf.Atan2(y, x) * Mathf.Rad2Deg + 90;

        myGun.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        myGun.transform.eulerAngles = new Vector3(0, 0, myGun.transform.eulerAngles.z);
    }

    protected override void Flip()
    {
        Vector3 newScale = gameObject.transform.localScale;
        newScale.x *= -1;
        gameObject.transform.localScale = newScale;
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

            if (isTargetInRange)
            {
                timeBetweenMessagesCounter -= Time.deltaTime;
                if (timeBetweenMessagesCounter < 0)
                {
                    ResetMessageTimer();
                    RandomSendMessage();
                }
            }

        }
        else
        {
            hasTarget = false;
            isTargetInRange = false;
        }
    }

    public void RandomSendMessage()
    {
        EnemyMessage message = GetRandomMesssage();
        if (message != null)
        {
            if (lastMessage != null)
            {
                while (lastMessage == message)
                {
                    message = GetRandomMesssage();
                }
            }

            lastMessage = message;
            DisplayFloatingText(message.message);
        }
    }

    public override void UpdateMovement()
    {
        if (!hasTarget || isTargetInRange)
        {
            //stop moving
            horizontalAxis = 0;
            if (hasTarget)
            {
                if (System.Math.Abs(targetPosition.y - transform.position.y) < 0.5)
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
            else
            {
                verticalAxis = 0;
            }
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

    private EnemyMessage GetRandomMesssage()
    {
        var range = 0;
        for (var i = 0; i < possibleMessages.Count; i++)
            range += (int)possibleMessages[i].rarity;

        range += range; //just so it can drop nothing
        var rand = Random.Range(0, range);
        var top = 0;

        for (var i = 0; i < possibleMessages.Count; i++)
        {
            top += (int)possibleMessages[i].rarity;
            if (rand < top)
            {
                return possibleMessages[i];
            }
        }

        return null;
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