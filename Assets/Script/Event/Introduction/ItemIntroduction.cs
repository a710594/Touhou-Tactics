using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemIntroduction : MyEvent
{
    public override void Start()
    {
        TutorialUI.Open(13, null);
    }
}
