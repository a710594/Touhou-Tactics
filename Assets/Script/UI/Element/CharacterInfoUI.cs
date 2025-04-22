using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Battle;

public class CharacterInfoUI : MonoBehaviour
{
    public Text LvLabel;
    public Text NameLabel;
    public Text HitRateLabel;
    public Image Image;
    public BattleValueBar HpBar;
    public StatusIconGroup StatusIconGroup;
    public Button Button;

    private BattleCharacterInfo _character;
    private Vector2Int _position;

    public void SetVisible(bool isVisible) 
    {
        gameObject.SetActive(isVisible);
    }

    public void SetData(BattleCharacterInfo character, Vector2Int position) 
    {
        _character = character;
        _position = position;
        LvLabel.text = "Lv. " + character.Lv;
        NameLabel.text = character.Name;
        HitRateLabel.gameObject.SetActive(false);
        HpBar.SetValue(character.CurrentHP, character.MaxHP);
        Image.sprite = Resources.Load<Sprite>("Image/Character/" + character.FileName + "_Head");

        StatusIconGroup.SetData(character, true, position);
    }

    public void SetDataWithTween(BattleCharacterInfo character, int originalHP, Vector2Int position)
    {
        _character = character;
        _position = position;
        LvLabel.text = "Lv. " + character.Lv;
        NameLabel.text = character.Name;
        HitRateLabel.gameObject.SetActive(false);
        HpBar.SetValueTween(originalHP, character.CurrentHP, character.MaxHP, null);
        Image.sprite = Resources.Load<Sprite>("Image/Character/" + character.FileName + "_Head");

        StatusIconGroup.SetData(character, true, position);
    }

    public void SetHpPrediction(int origin, int prediction, int max) 
    {
        HpBar.SetPrediction(origin, prediction, max);
    }

    //public void StopHpPrediction() 
    //{
    //    HpBar.StopPrediction();
    //}

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
            characterDetailUI.SetData((BattlePlayerInfo)_character, _position);
        }
    }

    private void Awake()
    {
        //PpBar.SetValue(0, BattleCharacterInfo.MaxPP);
        Button.onClick.AddListener(ButtonOnClick);
    }
}
