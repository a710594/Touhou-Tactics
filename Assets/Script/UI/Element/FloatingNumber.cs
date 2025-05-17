using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Battle;

public class FloatingNumber : MonoBehaviour
{
    public Text Label;
    public float Height;

    public void Play(float duration, Vector2 position, Log log)
    {
        if (int.TryParse(log.Text, out int n))
        {
            Label.fontSize = 50;
        }
        else
        {
            Label.fontSize = 30;
        }

        this.transform.position = position;
        Label.text = log.Text;

        if (log.Type == EffectModel.TypeEnum.MagicAttack || log.Type == EffectModel.TypeEnum.PhysicalAttack || log.Type == EffectModel.TypeEnum.RatioDamage)
        {
            if (log.HitType == HitType.Critical)
            {
                Label.color = Color.yellow;
            }
            else if (log.HitType == HitType.Hit)
            {
                Label.color = Color.red;
            }
            else
            {
                Label.color = Color.blue;
            }
        }
        else if (log.Type == EffectModel.TypeEnum.Poison)
        {
            Label.color = new Color32(180, 0, 180, 255);
        }
        else if (log.Type == EffectModel.TypeEnum.Recover || log.Type == EffectModel.TypeEnum.Medicine || log.Type == EffectModel.TypeEnum.Purify || log.Type == EffectModel.TypeEnum.RecoverAll)
        {
            Label.color = Color.green;
        }
        else if (log.Type == EffectModel.TypeEnum.Sleep)
        {
            Label.color = new Color32(0, 150, 200, 255);
        }
        else
        {
            Label.color = Color.black;
        }

        transform.DOMoveY(position.y + Height, duration).SetEase(Ease.OutCubic);
        Label.DOFade(0, duration).SetEase(Ease.InCubic);
    }

    void Awake()
    {
        Label.color = Color.clear;
    }
}
