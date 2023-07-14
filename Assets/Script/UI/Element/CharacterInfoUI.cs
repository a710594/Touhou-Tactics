using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterInfoUI : MonoBehaviour
{
    public Text NameLabel;
    public BattleValueBar HpBar;
    public StatusIcon StatusIcon;
    public Transform StatusGrid;

    private List<StatusIcon> _statusIconList = new List<StatusIcon>();

    public void SetVisible(bool isVisible) 
    {
        gameObject.SetActive(isVisible);
    }

    public void SetData(BattleCharacterInfo info) 
    {
        StatusIcon icon;

        NameLabel.text = info.Name;
        HpBar.SetValue(info.CurrentHP, info.MaxHP);

        for (int i=0; i<_statusIconList.Count; i++) 
        {
            _statusIconList[i].gameObject.SetActive(false);
        }
        for (int i=0; i<info.StatusList.Count; i++) 
        {
            if (i >= _statusIconList.Count) 
            {
                icon = Instantiate(StatusIcon);
                icon.transform.SetParent(StatusGrid);
                _statusIconList.Add(icon);
            }
            _statusIconList[i].gameObject.SetActive(true);
            _statusIconList[i].SetData(info.StatusList[i]);
        }
    }

    public void SetHpPrediction(int origin, int prediction, int max) 
    {
        HpBar.SetPrediction(origin, prediction, max);
    }

    public void StopHpPrediction() 
    {
        HpBar.StopPrediction();
    }
}
