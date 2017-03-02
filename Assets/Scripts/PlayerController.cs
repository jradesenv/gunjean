using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : BaseEntity
{
    public Enums.ControllerType controllerType;


    public override void Start()
    {
        base.Start();

        controllerType = Enums.ControllerType.XBoxController;

        var camera = FindObjectOfType<CameraController>();
        if (camera != null)
        {
            camera.Follow(this);
        } else
        {
            Debug.LogError("CANT FIND CAMERA");
        }

        var uiManager = FindObjectOfType<UIManager>();
        if (uiManager != null)
        {
            uiManager.Follow(this);
        }
        else
        {
            Debug.LogError("CANT FIND UIMANAGER");
        }
    }

    public override void UpdateInputs()
    {
        if (controllerType == Enums.ControllerType.MouseKeyboard)
        {
            UpdateMouseKeyboardInputs();
        } else
        {
            UpdateXBoxControllerInputs();
        }

        base.UpdateInputs();
    }

    private void UpdateMouseKeyboardInputs()
    {
        horizontalAxis = Input.GetAxisRaw("Horizontal");
        verticalAxis = Input.GetAxisRaw("Vertical");
        targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        fireButtonGotDown = Input.GetMouseButtonDown(0);
        fireButtonGotUp = Input.GetMouseButtonUp(0);
    }

    float _oldRX;
    float _oldRY;
    private void UpdateXBoxControllerInputs()
    {
        //Aim
        horizontalAxis = Input.GetAxisRaw("Horizontal");
        verticalAxis = Input.GetAxisRaw("Vertical");

        float x = Input.GetAxisRaw("RHorizontal");
        float y = Input.GetAxisRaw("RVertical");
        float R_analog_threshold = 0.20f;

        if (Mathf.Abs(x) < R_analog_threshold && Mathf.Abs(y) < R_analog_threshold)
        {
            x = _oldRX;
            y = _oldRY;
        }
        else
        {
            _oldRX = x;
            _oldRY = y;
        }

        var targetX = transform.position.x + ((x * 1) * 2);
        var targetY = transform.position.y + ((y * -1) * 2);
        targetPosition = new Vector3(targetX, targetY, transform.position.z);

        fireButtonGotDown = Input.GetKeyDown(KeyCode.Joystick1Button5);
        fireButtonGotUp = Input.GetKeyUp(KeyCode.Joystick1Button5);
    }

    public override void UpdateAim()
    {
        //aim
        if (controllerType == Enums.ControllerType.MouseKeyboard)
        {
            Quaternion rotation = Quaternion.LookRotation(myGun.transform.position
             - targetPosition, Vector3.forward);

            myGun.transform.rotation = rotation;
            myGun.transform.eulerAngles = new Vector3(0, 0, myGun.transform.eulerAngles.z);
        }
        else if (controllerType == Enums.ControllerType.XBoxController)
        {
            float x = _oldRX * -1;
            float y = _oldRY;
            float aim_angle = 0.0f;

            //float R_analog_threshold = 0.20f;
            //if (Mathf.Abs(x) < R_analog_threshold) { x = 0.0f; }
            //if (Mathf.Abs(y) < R_analog_threshold) { y = 0.0f; }

            if (x != 0.0f || y != 0.0f)
            {
                aim_angle = Mathf.Atan2(y, x) * Mathf.Rad2Deg + 90;

                myGun.transform.rotation = Quaternion.AngleAxis(aim_angle, Vector3.forward);
                myGun.transform.eulerAngles = new Vector3(0, 0, myGun.transform.eulerAngles.z);
            }
        }
    }
}
