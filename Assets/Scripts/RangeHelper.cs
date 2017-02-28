using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class RangeHelper
{
    public static bool CheckIfTargetIsInRange(GameObject me, GameObject target, float maxDistance)
    {
        bool isInRange = Vector3.Distance(me.transform.position, target.transform.position) <= maxDistance;
        return isInRange;
    }

    public static GameObject GetClosestWithTag(GameObject me, String targetTag)
    {
        GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag(targetTag);

        var distance = -1;
        GameObject closestObject = null;

        for (int i = 0; i < objectsWithTag.Length; ++i)
        {
            if (distance == -1 || distance > Vector3.Distance(me.transform.position, objectsWithTag[i].transform.position))
            {
                closestObject = objectsWithTag[i];
            }
        }

        return closestObject;
    }
}
