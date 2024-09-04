using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneEvent_1 : MyEvent
{
    public override void Start()
    {
        InputMamager.Instance.IsLock = true;
        ConversationUI.Open(8, true, () =>
        {
            InputMamager.Instance.IsLock = false;
        }, null);
    }
}
