using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : BaseEntity
{
    public override void Start()
    {
        base.Start();

        var camera = FindObjectOfType<CameraController>();
        if (camera != null)
        {
            camera.Follow(this);
        } else
        {
            Debug.LogError("CANT FIND CAMERA");
        }
    }

    public override void UpdateInputs()
    {
        horizontalAxis = Input.GetAxisRaw("Horizontal");
        verticalAxis = Input.GetAxisRaw("Vertical");
        targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        fireButtonGotDown = Input.GetMouseButtonDown(0);
        fireButtonGotUp = Input.GetMouseButtonUp(0);

        base.UpdateInputs();
    }
}
