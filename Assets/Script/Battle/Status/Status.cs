using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status
{
    public StatusModel Data;
    public int RemainTime;

    public Status() { }

    public Status(StatusModel data)
    {
        Data = data;
        RemainTime = data.Time;
    }

    public void ResetTurn()
    {
        RemainTime = Data.Time;
    }
}
