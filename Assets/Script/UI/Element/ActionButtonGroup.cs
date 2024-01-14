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
    public SkillInfoGroup SkillInfoGroup;

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
        if (BattleController.Instance.Info.IsTutorial && !BattleTutorialController.Instance.CanMove())
        {
            return;
        }
        BattleController.Instance.SetMoveState();
    }

    private void SkillOnClick() 
    {
        if (BattleController.Instance.Info.IsTutorial && !BattleTutorialController.Instance.CanSkill())
        {
            return;
        }

        BattleCharacterInfo character = BattleController.Instance.SelectedCharacter;
        if (!character.IsAuto)
        {
            ScrollView.transform.parent.gameObject.SetActive(true);
            ScrollView.SetData(new List<object>(character.SkillList));
        }
    }

    private void SupportOnClick()
    {
        if (BattleController.Instance.Info.IsTutorial && !BattleTutorialController.Instance.CanSupport())
        {
            return;
        }

        BattleCharacterInfo character = BattleController.Instance.SelectedCharacter;
        if (!character.IsAuto)
        {
            ScrollView.transform.parent.gameObject.SetActive(true);
            ScrollView.SetData(new List<object>(character.SupportList));
        }
    }

    private void ItemOnClick() 
    {
        if (BattleController.Instance.Info.IsTutorial && !BattleTutorialController.Instance.CanItem())
        {
            return;
        }

        BattleCharacterInfo character = BattleController.Instance.SelectedCharacter;
        if (!character.IsAuto)
        {
            ScrollView.transform.parent.gameObject.SetActive(true);
            ScrollView.SetData(new List<object>(ItemManager.Instance.GetBattleItemList(character)));
        }
    }

    private void IdleOnClick() 
    {
        if (BattleController.Instance.Info.IsTutorial && !BattleTutorialController.Instance.CanIdle())
        {
            return;
        }

        BattleController.Instance.Idle();
    }

    private void ResetOnClick() 
    {
        if (BattleController.Instance.Info.IsTutorial && !BattleTutorialController.Instance.CanReset())
        {
            return;
        }

        BattleController.Instance.ResetAction();
        ResetButton.gameObject.SetActive(false);
    }

    private void ScrollItemOnClick(ScrollItem scrollItem)
    {
        object obj = scrollItem.Data;
        if (BattleController.Instance.Info.IsTutorial && !BattleTutorialController.Instance.CheckScrollItem(obj))
        {
            return;
        }

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

    private void ShowInfo(ScrollItem scrollItem)
    {
        object obj = scrollItem.Data;
        if (obj is Skill)
        {
            Skill skill = (Skill)obj;
            SkillInfoGroup.SetData(skill);
            SkillInfoGroup.gameObject.SetActive(true);
        }
        else if (obj is Support)
        {
            Support support = (Support)obj;
            SkillInfoGroup.SetData(support);
            SkillInfoGroup.gameObject.SetActive(true);
        }
        else if (obj is Card)
        {
            Card card = (Card)obj;
            SkillInfoGroup.SetData(card);
            SkillInfoGroup.gameObject.SetActive(true);
        }
        else if (obj is Consumables)
        {
            Consumables consumbles = (Consumables)obj;
            SkillInfoGroup.SetData(consumbles);
            SkillInfoGroup.gameObject.SetActive(true);
        }
        else if (obj is Food)
        {
            Food food = (Food)obj;
            SkillInfoGroup.SetData(food);
            SkillInfoGroup.gameObject.SetActive(true);
        }
    }

    private void HideSkillInfo(ScrollItem scrollItem)
    {
        SkillInfoGroup.gameObject.SetActive(false);
    }

    private void Awake()
    {
        MoveButton.onClick.AddListener(MoveOnClick);
        SkillButton.onClick.AddListener(SkillOnClick);
        SupportButton.onClick.AddListener(SupportOnClick);
        ItemButton.onClick.AddListener(ItemOnClick);
        IdleButton.onClick.AddListener(IdleOnClick);
        ResetButton.onClick.AddListener(ResetOnClick);
        ScrollView.ClickHandler += ScrollItemOnClick;
        ScrollView.EnterHandler += ShowInfo;
        ScrollView.ExitHandler += HideSkillInfo;
        ScrollView.transform.parent.gameObject.SetActive(false);
        SkillInfoGroup.gameObject.SetActive(false);
    }
}
