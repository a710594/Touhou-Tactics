using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventInfo
{
    public bool GetItem = false;
    public bool ReimuJoin = false;
    public bool SanaeTutorial = false;
    public bool MarisaJoin = false;
    public bool F2 = false;
    public bool F3 = false;
    public bool SpellTutorial = false;
    public bool UnlockCook = false;
    public bool UnlockShop = false;
    public bool ReimuIntroduction = false;
    public bool MarisaIntroduction = false;
    public bool ItemIntroduction = false;
    public bool EquipIntroduction = false;

    public List<string> TriggerList = new List<string>(); //已觸發的踩地事件
}
