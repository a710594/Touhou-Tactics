using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Battle;

public class FloatingNumber : MonoBehaviour
{
    public Text Label;
    public float Height;
    public float Duration;

    public void SetValue(float height, float duration)
    {
        Height = height;
        Duration = duration;
    }

    /*public void Play(string text, FloatingNumberData.TypeEnum type, Vector2 position)
    {
        if (int.TryParse(text, out int n))
        {
            Label.fontSize = 50;
        }
        else
        {
            Label.fontSize = 30;
        }

        this.transform.position = position;
        Label.text = text;

        if (type == FloatingNumberData.TypeEnum.Damage)
        {
            Label.color = Color.red;
        }
        else if (type == FloatingNumberData.TypeEnum.Poison)
        {
            Label.color = new Color32(180, 0, 180, 255);
        }
        else if (type == FloatingNumberData.TypeEnum.Recover)
        {
            Label.color = Color.green;
        }
        else if (type == FloatingNumberData.TypeEnum.Miss)
        {
            Label.color = Color.blue;
        }
        else if (type == FloatingNumberData.TypeEnum.Critical)
        {
            Label.color = Color.yellow;
        }
        else if (type == FloatingNumberData.TypeEnum.Paralysis)
        {
            Label.color = new Color32(180, 140, 0, 255);
        }
        else if (type == FloatingNumberData.TypeEnum.Sleeping)
        {
            Label.color = new Color32(0, 150, 200, 255);
        }
        else if (type == FloatingNumberData.TypeEnum.Confusion)
        {
            Label.color = new Color32(100, 0, 200, 255);
        }
        else if (type == FloatingNumberData.TypeEnum.Other)
        {
            Label.color = Color.black;
        }

        this.transform.DOMoveY(position.y + Height, Duration).SetEase(Ease.OutCubic).OnComplete(() =>
        {
        });

        Label.DOFade(0, Duration).SetEase(Ease.InCubic).OnComplete(() =>
        {
        });
    }*/

    public void Play(Battle.Log log, Vector2 position)
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

        if (log.Effect.Type == EffectModel.TypeEnum.MagicAttack || log.Effect.Type == EffectModel.TypeEnum.PhysicalAttack || log.Effect.Type == EffectModel.TypeEnum.RatioDamage)
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
        else if (log.Effect.Type == EffectModel.TypeEnum.Poison)
        {
            Label.color = new Color32(180, 0, 180, 255);
        }
        else if (log.Effect.Type == EffectModel.TypeEnum.Recover || log.Effect.Type == EffectModel.TypeEnum.Medicine || log.Effect.Type == EffectModel.TypeEnum.Purify)
        {
            Label.color = Color.green;
        }
        else if (log.Effect.Type == EffectModel.TypeEnum.Sleep)
        {
            Label.color = new Color32(0, 150, 200, 255);
        }
        else
        {
            Label.color = Color.black;
        }

        this.transform.DOMoveY(position.y + Height, Duration).SetEase(Ease.OutCubic).OnComplete(() =>
        {
        });

        Label.DOFade(0, Duration).SetEase(Ease.InCubic).OnComplete(() =>
        {
        });
    }

    void Awake()
    {
        Label.color = Color.clear;
    }
}
