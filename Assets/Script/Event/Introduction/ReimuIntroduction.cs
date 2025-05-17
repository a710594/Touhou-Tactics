using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReimuIntroduction : MyEvent
{
    public override void Start()
    {
        TutorialUI.Open(10, null);
    }
}
