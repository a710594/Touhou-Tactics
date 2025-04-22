using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BattleValueBar : ValueBar //����w���ƭ�
{
    public void SetPrediction(int origin, int prediction, int max) //�w���ˮ`�᪺��q
    {
        Bar.fillAmount = (float)prediction / (float)max;
        Label.text = origin + "��" + prediction;
    }

    private void Awake()
    {
    }
}
