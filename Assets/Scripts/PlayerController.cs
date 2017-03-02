using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : BaseEntity
{
    public Enums.ControllerType controllerType;


    public override void Start()
    {
        base.Start();
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
    float _minRValue = 0.20f;
    private void UpdateXBoxControllerInputs()
    {
        //Aim
        horizontalAxis = Input.GetAxisRaw("LHorizontal");
        verticalAxis = Input.GetAxisRaw("LVertical");

        float x = Input.GetAxisRaw("RHorizontal");
        float y = Input.GetAxisRaw("RVertical");

        if (Mathf.Abs(x) < _minRValue && Mathf.Abs(y) < _minRValue)
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
            UpdateAimMouseKeyboard();
        }
        else if (controllerType == Enums.ControllerType.XBoxController)
        {
            UpdateAimXBoxController();
        }
    }

    private void UpdateAimMouseKeyboard()
    {
        Quaternion rotation = Quaternion.LookRotation(myGun.transform.position
             - targetPosition, Vector3.forward);

        myGun.transform.rotation = rotation;
        myGun.transform.eulerAngles = new Vector3(0, 0, myGun.transform.eulerAngles.z);
    }

    private void UpdateAimXBoxController()
    {
        float x = _oldRX * -1;
        float y = _oldRY;

        if (x != 0.0f || y != 0.0f)
        {
            float angle = Mathf.Atan2(y, x) * Mathf.Rad2Deg + 90;

            myGun.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            myGun.transform.eulerAngles = new Vector3(0, 0, myGun.transform.eulerAngles.z);
        }
    }
}
