using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BattleValueBar : ValueBar //能夠預覽數值
{
    public Image SubBar;

    private Color _originalColor;
    private Color _flashColor;

    public void SetPrediction(int origin, int prediction, int max) //預覽傷害後的血量
    {
        if (prediction < origin)
        {
            Bar.DOColor(_flashColor, 0.5f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
        }
        else
        {
            SubBar.DOColor(_flashColor, 0.5f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
        }
        SubBar.fillAmount = (float)prediction / (float)max;
        Label.text = origin + "→" + prediction;
    }

    public void StopPrediction()
    {
        Bar.DOKill();
        SubBar.DOKill();
        Bar.color = _originalColor;
        SubBar.color = _originalColor;
        SubBar.fillAmount = 0;
        Label.text = string.Empty;
    }

    private void Awake()
    {
        SubBar.fillAmount = 0;
        _originalColor = Bar.color;
        _flashColor = new Color(_originalColor.r, _originalColor.g, _originalColor.b, 0);
    }
}
