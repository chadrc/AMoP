using UnityEngine;
using System.Collections;

public static class MathUtils
{
    public static Vector2 ClosestCardinal(Vector2 vec)
    {
        Vector2 cardinal;
        
        float angle = Vector2.Angle(Vector2.right, vec);
        if (vec.y < 0)
        {
            angle = 360 - angle;
        }

        if (angle >= 45 && angle < 135)
        {
            cardinal = Vector2.up;
        }
        else if (angle >= 135 && angle < 225)
        {
            cardinal = Vector3.left;
        }
        else if (angle >= 225 && angle < 315)
        {
            cardinal = Vector3.down;
        }
        else
        {
            cardinal = Vector3.right;
        }

        return cardinal;
    }
}
