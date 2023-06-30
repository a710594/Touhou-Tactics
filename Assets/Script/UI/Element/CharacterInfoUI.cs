using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterInfoUI : MonoBehaviour
{
    public Text NameLabel;
    public BattleValueBar HpBar;
    public BattleValueBar MpBar;

    public void SetVisible(bool isVisible) 
    {
        gameObject.SetActive(isVisible);
    }

    public void SetData(BattleCharacterInfo info) 
    {
        NameLabel.text = info.Name;
        HpBar.SetValue(info.CurrentHP, info.MaxHP);
        MpBar.SetValue(info.CurrentMP, info.MaxMP);
    }

    public void SetHpPrediction(int origin, int prediction, int max) 
    {
        HpBar.SetPrediction(origin, prediction, max);
    }

    public void StopHpPrediction() 
    {
        HpBar.StopPrediction();
    }

    public void SetMpPrediction(int origin, int prediction, int max)
    {
        MpBar.SetPrediction(origin, prediction, max);
    }

    public void StopMpPrediction()
    {
        MpBar.StopPrediction();
    }
}
