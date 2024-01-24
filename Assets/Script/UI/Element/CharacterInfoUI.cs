using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterInfoUI : MonoBehaviour
{
    public Text LvLabel;
    public Text NameLabel;
    public Text HitRateLabel;
    public BattleValueBar HpBar;
    public ValueBar PpBar;
    public StatusIcon StatusIcon;
    public Transform StatusGrid;
    public Button Button;

    private BattleCharacterInfo _character;
    private List<StatusIcon> _statusIconList = new List<StatusIcon>();

    public void SetVisible(bool isVisible) 
    {
        gameObject.SetActive(isVisible);
    }

    public void SetData(BattleCharacterInfo character) 
    {
        StatusIcon icon;

        _character = character;
        LvLabel.text = "Lv. " + character.Lv;
        NameLabel.text = character.Name;
        HitRateLabel.gameObject.SetActive(false);
        HpBar.SetValue(character.CurrentHP, character.MaxHP);
        if (character.Faction == BattleCharacterInfo.FactionEnum.Player)
        {
            PpBar.gameObject.SetActive(true);
            PpBar.SetValue(character.CurrentPP, BattleCharacterInfo.MaxPP);
        }
        else
        {
            PpBar.gameObject.SetActive(false);
        }

        for (int i=0; i<_statusIconList.Count; i++) 
        {
            _statusIconList[i].gameObject.SetActive(false);
        }
        for (int i=0; i<character.StatusList.Count; i++) 
        {
            if (i >= _statusIconList.Count) 
            {
                icon = Instantiate(StatusIcon);
                icon.transform.SetParent(StatusGrid);
                _statusIconList.Add(icon);
            }
            _statusIconList[i].gameObject.SetActive(true);
            _statusIconList[i].SetData(character.StatusList[i]);
        }
    }

    public void SetData(CharacterInfo character)
    {
        LvLabel.text = "Lv. " + CharacterManager.Instance.Info.Lv;
        NameLabel.text = character.Name;
        HitRateLabel.gameObject.SetActive(false);
        HpBar.SetValue(character.CurrentHP, character.MaxHP);
        PpBar.gameObject.SetActive(false);
    }

    public void SetHpPrediction(int origin, int prediction, int max) 
    {
        HpBar.SetPrediction(origin, prediction, max);
    }

    public void StopHpPrediction() 
    {
        HpBar.StopPrediction();
    }

    public void SetHitRate(int hitRate) 
    {
        HitRateLabel.text = "©R¤¤²v:" + hitRate + "%";
        HitRateLabel.gameObject.SetActive(true);
    }

    public void HideHitRate()
    {
        HitRateLabel.gameObject.SetActive(false);
    }

    private void ButtonOnClick() 
    {
        if (_character != null)
        {
            CharacterDetailUI characterDetailUI = CharacterDetailUI.Open(false);
            characterDetailUI.SetData(_character);
        }
    }

    private void Awake()
    {
        PpBar.SetValue(0, BattleCharacterInfo.MaxPP);
        Button.onClick.AddListener(ButtonOnClick);
    }
}
