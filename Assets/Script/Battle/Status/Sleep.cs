using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sleep : Status
{
    public Sleep(StatusModel data)
    {
        Data = data;
        RemainTime = data.Time;
    }
}
