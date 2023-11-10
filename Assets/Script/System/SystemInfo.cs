using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemInfo
{
    public int MaxFloor;
    public string CurrentScene;

    public void Init() 
    {
        MaxFloor = 1;
        CurrentScene = "Explore";
    }
}
