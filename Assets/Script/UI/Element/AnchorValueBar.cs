using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class AnchorValueBar : BattleValueBar //顯示在戰場上的小HP條
{
    private Transform _anchor;

    public void SetAnchor(Transform anchor)
    {
        _anchor = anchor;
    }

    protected override void UpdateData()
    {
        base.UpdateData();
        if (_anchor != null)
        {
            this.transform.position = Camera.main.WorldToScreenPoint(_anchor.position) + Vector3.up * 70;
        }
    }
}
