using UnityEngine;

public class ETouch
{
    public static bool TouchBegin()
    {
        if (Input.GetMouseButtonDown(0))
        {
            return true;
        }
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            return true;
        }
        return false;
    }

    public static bool TouchEnd()
    {
        if (Input.GetMouseButtonUp(0))
        {
            return true;
        }
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            return true;
        }
        return false;
    }

    public static bool TouchIng()
    {
        if (Input.GetMouseButton(0))
        {
            return true;
        }
        else if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            return true;
        }
        return false;
    }

}