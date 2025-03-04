using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventInfo
{
    public bool GetItem = false;
    public bool SanaeJoin = false;
    public bool SanaeTutorial = false;
    public bool MarisaJoin = false;
    public bool F2 = false;

    public List<string> TriggerList = new List<string>(); //已觸發的踩地事件
}
