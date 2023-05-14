using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Vector3Extensions
{
    public static Vector3 SetX(this Vector3 pos, float x)
    {
        return new Vector3(x, pos.y, pos.z);
    }
}
