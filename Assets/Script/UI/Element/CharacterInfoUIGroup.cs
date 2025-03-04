using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInfoUIGroup : MonoBehaviour
{
    public CharacterInfoUI CharacterInfoUI_1;
    public CharacterInfoUI CharacterInfoUI_2;

    public void SetCharacterInfoUI_1(BattleCharacterInfo info)
    {
        if (info != null)
        {
            CharacterInfoUI_1.SetVisible(true);
            CharacterInfoUI_1.SetData(info);
        }
        else
        {
            CharacterInfoUI_1.SetVisible(false);
        }
    }

    public void SetCharacterInfoUI_2(BattleCharacterInfo info)
    {
        if (info != null)
        {
            CharacterInfoUI_2.SetVisible(true);
            CharacterInfoUI_2.SetData(info);
        }
        else
        {
            CharacterInfoUI_2.SetVisible(false);
        }
    }

    public void SetPredictionInfo_1(BattleCharacterInfo info, int predictionHp)
    {
        CharacterInfoUI_1.SetHpPrediction(info.CurrentHP, predictionHp, info.MaxHP);
    }

    public void SetPredictionInfo_2(BattleCharacterInfo info, int predictionHp)
    {
        CharacterInfoUI_2.SetHpPrediction(info.CurrentHP, predictionHp, info.MaxHP);
    }

    public void StopPredictionInfo()
    {
        CharacterInfoUI_2.StopHpPrediction();
    }

    public void SetHitRate(int hitRate)
    {
        CharacterInfoUI_2.SetHitRate(hitRate);
    }

    public void HideHitRate()
    {
        CharacterInfoUI_2.HideHitRate();
    }
}
