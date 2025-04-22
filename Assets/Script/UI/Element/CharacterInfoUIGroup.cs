using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CharacterInfoUIGroup : MonoBehaviour
{
    public CharacterInfoUI CharacterInfoUI_1;
    public CharacterInfoUI CharacterInfoUI_2;

    private Timer _timer = new Timer();

    public void ShowCharacterInfoUI_1(BattleCharacterInfo info, Vector2Int position)
    {
        CharacterInfoUI_1.SetVisible(true);
        CharacterInfoUI_1.SetData(info, position);
    }

    public void HideCharacterInfoUI_1()
    {
        CharacterInfoUI_1.SetVisible(false);
    }

    public void ShowCharacterInfoUI_2(BattleCharacterInfo info, Vector2Int position)
    {
        CharacterInfoUI_2.SetVisible(true);
        CharacterInfoUI_2.SetData(info, position);
    }

    public void HideCharacterInfoUI_2()
    {
        CharacterInfoUI_2.SetVisible(false);
    }

    public void SetCharacterInfoUIWithTween_2(BattleCharacterController character, int originalHP, Vector2Int position)
    {
        if (character != null)
        {
            if (character.Info != null)
            {
                CharacterInfoUI_2.SetVisible(true);
                CharacterInfoUI_2.SetDataWithTween(character.Info, originalHP, position);
            }
            else
            {
                CharacterInfoUI_2.SetVisible(false);
            }
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

    public void MoveCharacterInfoUI_1() 
    {
        CharacterInfoUI_1.transform.localPosition = new Vector3(1100, CharacterInfoUI_1.transform.localPosition.y, CharacterInfoUI_1.transform.localPosition.z);
        _timer.Start(0.5f, ()=> 
        {
            CharacterInfoUI_1.transform.DOLocalMoveX(-250, 0.5f).SetEase(Ease.OutQuint).OnComplete(() =>
            {
                CharacterInfoUI_1.transform.DOLocalMoveX(-827, 0.5f).SetEase(Ease.OutQuint);
            });
        });
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
