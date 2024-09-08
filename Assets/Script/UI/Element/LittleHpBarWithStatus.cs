using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Battle;

public class LittleHpBarWithStatus : MonoBehaviour
{
    public AnchorValueBar HpBar;
    public StatusIconGroup StatusIconGroup;

    public void SetAnchor(Transform anchor)
    {
        HpBar.SetAnchor(anchor);
    }

    public void SetData(BattleCharacterInfo info) 
    {
        HpBar.SetValue(info.CurrentHP, info.MaxHP);
        StatusIconGroup.SetData(info, false);
    }

    public void SetPrediction(int origin, int prediction, int max) 
    {
        HpBar.SetPrediction(origin, prediction, max);
    }

    public void StopPrediction()
    {
        HpBar.StopPrediction();
    }
}
