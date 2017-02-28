using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class BaseItemEntity : MonoBehaviour
{
    public Enums.Items.Type type;
    public Enums.Items.CollectType collectType;
    public bool isSolid;
    public int quantity;

    void OnCollisionEnter2D(Collision2D other)
    {
        PlayerController collectEntity = other.gameObject.GetComponent<PlayerController>();
        if (collectEntity)
        {
            BaseItem item = new BaseItem();
            item.collectType = collectType;
            item.type = type;
            item.quantity = quantity;
            collectEntity.Collect(item);

            Destroy(gameObject);
        }
    }
}
