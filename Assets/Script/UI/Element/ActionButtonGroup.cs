using Battle;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionButtonGroup : MonoBehaviour
{
    public Button MoveButton;
    public Button SkillButton;
    public Button SupportButton;
    public Button ItemButton;
    public Button IdleButton;
    public Button ResetButton;
    public Text ActionCountLabel;
    public ScrollView ScrollView;
    public TipLabel TipLabel;

    public void SetButton(BattleCharacterInfo character) 
    {
        SkillButton.interactable = !character.HasUseSkill;
        SupportButton.interactable = !character.HasUseSupport;
        ItemButton.interactable = !character.HasUseItem;
        ActionCountLabel.text = "Action Count: " + character.ActionCount.ToString();
        ScrollView.transform.parent.gameObject.SetActive(false);
    }

    public void SetScrollView(List<object> list)
    {
        ScrollView.SetData(list);
    }

    private void MoveOnClick() 
    {
        BattleController.Instance.SetMoveState();
    }

    private void SkillOnClick() 
    {
        BattleCharacterInfo character = BattleController.Instance.SelectedCharacter;
        if (!character.IsAuto)
        {
            ScrollView.transform.parent.gameObject.SetActive(true);
            ScrollView.SetData(new List<object>(character.SkillList));
        }
    }

    private void SupportOnClick()
    {
        BattleCharacterInfo character = BattleController.Instance.SelectedCharacter;
        if (!character.IsAuto)
        {
            ScrollView.transform.parent.gameObject.SetActive(true);
            ScrollView.SetData(new List<object>(character.SupportList));
        }
    }

    private void ItemOnClick() 
    {
        BattleCharacterInfo character = BattleController.Instance.SelectedCharacter;
        if (!character.IsAuto)
        {
            ScrollView.transform.parent.gameObject.SetActive(true);
            ScrollView.SetData(new List<object>(ItemManager.Instance.GetBattleItemList(character)));
        }
    }

    private void IdleOnClick() 
    {
        BattleController.Instance.Idle();
    }

    private void ResetOnClick() 
    {
        BattleController.Instance.ResetAction();
    }

    private void ScrollItemOnClick(object obj, ScrollItem scrollItem)
    {
        bool canUse = true;
        string tip = "";
        if (obj is Skill)
        {
            Skill skill = (Skill)obj;
            if (skill.CurrentCD > 0)
            {
                canUse = false;
                tip = "還需要" + skill.CurrentCD + "回合冷卻";
            }
        }
        else if (obj is Support)
        {
            Support support = (Support)obj;
            if (support.CurrentCD > 0)
            {
                canUse = false;
                tip = "還需要" + support.CurrentCD + "回合冷卻";
            }
        }
        else if (obj is Card)
        {
            Card card = (Card)obj;
            if (BattleController.Instance.SelectedCharacter.CurrentPP < card.CardData.PP)
            {
                canUse = false;
                tip = "PP不足";
            }
        }

        if (canUse)
        {
            BattleController.Instance.SelectObject(obj);
        }
        else
        {
            TipLabel.SetLabel(tip);
        }
    }

    private void Awake()
    {
        MoveButton.onClick.AddListener(MoveOnClick);
        SkillButton.onClick.AddListener(SkillOnClick);
        SupportButton.onClick.AddListener(SupportOnClick);
        ItemButton.onClick.AddListener(ItemOnClick);
        IdleButton.onClick.AddListener(IdleOnClick);
        ResetButton.onClick.AddListener(ResetOnClick);
        ScrollView.ItemOnClickHandler += ScrollItemOnClick;
        ScrollView.transform.parent.gameObject.SetActive(false);
    }
}
