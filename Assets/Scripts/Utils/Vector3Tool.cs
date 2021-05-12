using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vector3Tool
{
    public static Vector2 ToVector2(Vector3 vector3)
    {
        return new Vector2(vector3.x, vector3.y);
    }
    public static Vector3 ToVector3(Vector2 vector2,float z)
    {
        return new Vector3(vector2.x, vector2.y, z);
    }
    public static Vector3 SetX(Vector3 vector3, float x)
    {
        vector3.x=x;
        return vector3;
    }
    public static Vector3 SetY(Vector3 vector3, float y)
    {
        vector3.y = y;
        return vector3;
    }
    public static Vector3 SetZ(Vector3 vector3, float z)
    {
        vector3.z = z;
        return vector3;
    }
}
