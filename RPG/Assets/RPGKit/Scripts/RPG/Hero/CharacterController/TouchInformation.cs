using UnityEngine;
using System.Collections;

public class TouchInformation
{
    public int LeftTopX;
    public int LeftTopY;

    public int RightBottomX;
    public int RightBottomY;

    public Vector2 StartPosition;
    public int FingerID = -5;

    public TouchTypeEnum TouchType;

    public float TouchIncrementY = 25;
    public float TouchIncrementX = 25;

    public bool IsInRange(Vector2 touchPosition)
    {
        touchPosition.y = Screen.height - touchPosition.y;
        if (touchPosition.x < LeftTopX || touchPosition.x > RightBottomX)
        {
            return false;
        }

        if (touchPosition.y < LeftTopY || touchPosition.y > RightBottomY)
        {
            return false;
        }

        return true;
    }

    public void Prepare()
    {
        FingerID = -5;
        LeftTopY = Screen.height - LeftTopY;
        RightBottomY = Screen.height - RightBottomY;
    }
}

public enum TouchTypeEnum
{ 
    Move,
    Rotation
}