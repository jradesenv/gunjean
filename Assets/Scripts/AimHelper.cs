using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class AimHelper
{
    public enum XAngle
    {
        Right,
        Left
    }

    public enum YAngle
    {
        Top,
        Bottom
    }

    public class AimAngle
    {
        public XAngle x;
        public YAngle y;
    }

    public static AimAngle GetCurrentAimAngle(Vector2 mousePos, Vector2 playerPos)
    {
        Vector2 v = mousePos - playerPos;
        var angleRadians = Mathf.Atan2(v.y, v.x);
        var angleDegrees = angleRadians * Mathf.Rad2Deg;

        if (angleDegrees < 0)
            angleDegrees += 360;

        AimAngle currentAngle = new AimAngle();
        if (angleDegrees > 0 && angleDegrees <= 90)
        {
            currentAngle.x = XAngle.Right;
            currentAngle.y = YAngle.Top;
        }
        else if (angleDegrees > 90 && angleDegrees <= 180)
        {
            currentAngle.x = XAngle.Left;
            currentAngle.y = YAngle.Top;
        }
        else if (angleDegrees > 180 && angleDegrees <= 270)
        {
            currentAngle.x = XAngle.Left;
            currentAngle.y = YAngle.Bottom;
        } else
        {
            currentAngle.x = XAngle.Right;
            currentAngle.y = YAngle.Bottom;
        }

        return currentAngle;
    }
}