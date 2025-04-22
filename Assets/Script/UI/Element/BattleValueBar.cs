using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BattleValueBar : ValueBar //能夠預覽數值
{
    public void SetPrediction(int origin, int prediction, int max) //預覽傷害後的血量
    {
        Bar.fillAmount = (float)prediction / (float)max;
        Label.text = origin + "→" + prediction;
    }

    private void Awake()
    {
    }
}
