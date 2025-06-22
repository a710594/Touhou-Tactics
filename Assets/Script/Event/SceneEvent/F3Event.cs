using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class F3Event : MyEvent
{
    public override void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Explore.ExploreManager.Instance.Player.Enable = false;
        ExploreUI exploreUI = GameObject.Find("ExploreUI").GetComponent<ExploreUI>();
        exploreUI.SetVisible(false);
        TutorialUI.Open(14, () =>
        {
            Explore.ExploreManager.Instance.Player.Enable = true;
            exploreUI.SetVisible(true);
            Cursor.lockState = CursorLockMode.Locked;
        });
    }
}
