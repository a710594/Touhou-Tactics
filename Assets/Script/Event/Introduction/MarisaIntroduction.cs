using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarisaIntroduction : MyEvent
{
    public override void Start()
    {
        TutorialUI.Open(12, null);
    }
}
