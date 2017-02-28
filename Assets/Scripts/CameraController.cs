using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private float moveSpeed;
    private BaseEntity targetToFollow;
    private Vector3 targetPosition;

    private void Start()
    {

    }

    private void Update()
    {
        if (targetToFollow != null)
        {
            targetPosition = new Vector3(targetToFollow.transform.position.x, targetToFollow.transform.position.y, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }
    }

    public void Follow(BaseEntity target)
    {
        moveSpeed = target.moveSpeed - 1;
        targetToFollow = target;
    }
}