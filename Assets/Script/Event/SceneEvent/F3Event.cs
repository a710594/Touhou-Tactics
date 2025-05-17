using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class F3Event : MyEvent
{
    public override void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        InputMamager.Instance.IsLock = true;
        ExploreUI exploreUI = GameObject.Find("ExploreUI").GetComponent<ExploreUI>();
        exploreUI.SetVisible(false);
        TutorialUI.Open(14, () =>
        {
            InputMamager.Instance.IsLock = false;
            exploreUI.SetVisible(true);
            Cursor.lockState = CursorLockMode.Locked;
        });
    }
}
