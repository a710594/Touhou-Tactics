using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SanaeIntroduction : MyEvent
{
    public override void Start()
    {
        TutorialUI.Open(8, null);
    }
}
