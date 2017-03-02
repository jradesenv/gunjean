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

        float distance = -1;
        GameObject closestObject = null;

        for (int i = 0; i < objectsWithTag.Length; ++i)
        {
            float thisDistance = Vector3.Distance(me.transform.position, objectsWithTag[i].transform.position);
            if (distance == -1 || distance > thisDistance)
            {
                distance = thisDistance;
                closestObject = objectsWithTag[i];
            }
        }

        return closestObject;
    }
}
