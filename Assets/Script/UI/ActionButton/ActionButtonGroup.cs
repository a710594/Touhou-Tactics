using Battle;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ActionButtonGroup : MonoBehaviour
{
    public Button MoveButton;
    public ActionButton SkillButton;
    public ActionButton SupportButton;
    public ActionButton SpellButton;
    public ActionButton ItemButton;
    public Button IdleButton;
    public Button ResetButton;
    public Text ActionCountLabel;
    public ScrollView ScrollView;
    public TipLabel TipLabel;
    public SkillInfoGroup SkillInfoGroup;

    public void SetButton(BattleCharacterInfo character) 
    {
        SkillButton.SetColor(character);
        SupportButton.SetColor(character);
        ItemButton.SetColor(character);
        SpellButton.SetColor(character);
        ActionCountLabel.text = "剩餘行動次數：" + character.ActionCount.ToString();
        ScrollView.transform.parent.gameObject.SetActive(false);
    }

    public void SetScrollView(List<object> list)
    {
        ScrollView.SetData(list);
    }

    private void MoveOnClick() 
    {
        if (BattleController.Instance.IsTutorialActive && !BattleController.Instance.Tutorial.CanMove())
        {
            return;
        }
        BattleController.Instance.SetState<BattleController.MoveState>();
    }

    private void SkillOnClick() 
    {
        if (BattleController.Instance.IsTutorialActive && !BattleController.Instance.Tutorial.CanSkill())
        {
            return;
        }

        BattleCharacterInfo character = BattleController.Instance.SelectedCharacter.Info;
        if (!character.IsAuto)
        {
            ScrollView.transform.parent.gameObject.SetActive(true);
            ScrollView.SetData(new List<object>(character.SkillList));
        }
    }

    private void SupportOnClick()
    {
        if (BattleController.Instance.IsTutorialActive && !BattleController.Instance.Tutorial.CanSupport())
        {
            return;
        }

        BattleCharacterInfo character = BattleController.Instance.SelectedCharacter.Info;
        if (!character.IsAuto)
        {
            ScrollView.transform.parent.gameObject.SetActive(true);
            ScrollView.SetData(new List<object>(character.SupportList));
        }
    }

    private void SpellOnClick()
    {
        if (BattleController.Instance.IsTutorialActive && !BattleController.Instance.Tutorial.CanSpell())
        {
            return;
        }

        BattleCharacterInfo character = BattleController.Instance.SelectedCharacter.Info;
        if (!character.IsAuto)
        {
            ScrollView.transform.parent.gameObject.SetActive(true);
            ScrollView.SetData(new List<object>(character.SpellList));
        }
    }

    private void ItemOnClick() 
    {
        if (BattleController.Instance.IsTutorialActive && !BattleController.Instance.Tutorial.CanItem())
        {
            return;
        }

        BattleCharacterInfo character = BattleController.Instance.SelectedCharacter.Info;
        if (!character.IsAuto)
        {
            ScrollView.transform.parent.gameObject.SetActive(true);
            ScrollView.SetData(new List<object>(ItemManager.Instance.GetBattleItemList()));
        }
    }

    private void IdleOnClick() 
    {
        if (BattleController.Instance.IsTutorialActive && !BattleController.Instance.Tutorial.CanIdle())
        {
            return;
        }

        BattleController.Instance.SelectedCharacter.Info.ActionCount = 0;
        BattleController.Instance.SetState<BattleController.EndState>();
    }

    private void ResetOnClick() 
    {
        if (BattleController.Instance.IsTutorialActive && !BattleController.Instance.Tutorial.CanReset())
        {
            return;
        }

        BattleController.Instance.ResetAction();
        ResetButton.gameObject.SetActive(false);
    }

    private void ScrollItemOnClick(PointerEventData eventData, ButtonPlus buttonPlus)
    {
        BattleCharacterInfo character = BattleController.Instance.SelectedCharacter.Info;
        Command command = (Command)buttonPlus.Data;
        if (BattleController.Instance.IsTutorialActive && !BattleController.Instance.Tutorial.CheckScrollItem(command))
        {
            return;
        }

        bool canUse = true;
        string tip = "";
        if (command is Skill)
        {
            Skill skill = (Skill)command;
            if (skill.CurrentCD > 0)
            {
                canUse = false;
                tip = "還需要" + skill.CurrentCD + "回合冷卻";
            }
        }
        else if (command is Support)
        {
            Support support = (Support)command;
            if (support.CurrentCD > 0)
            {
                canUse = false;
                tip = "還需要" + support.CurrentCD + "回合冷卻";
            }
        }
        else if (command is Spell)
        {
            Spell spell = (Spell)command;
            if (!character.CanUseSpell)
            {
                canUse = false;
                tip = "下次行動才能使用符卡";
            }
        }

        if (canUse)
        {
            BattleController.Instance.SetSelectedCommand(command);
            BattleController.Instance.SetState<BattleController.TargetState>();
        }
        else
        {
            TipLabel.SetLabel(tip);
        }
    }

    private void ShowInfo(ButtonPlus buttonPlus)
    {
        if(!BattleController.Instance.IsTutorialActive)
        {
            Command command = (Command)buttonPlus.Data;
            if (command is Skill)
            {
                Skill skill = (Skill)command;
                SkillInfoGroup.SetData(skill);
                SkillInfoGroup.gameObject.SetActive(true);
            }
            else if (command is Support)
            {
                Support support = (Support)command;
                SkillInfoGroup.SetData(support);
                SkillInfoGroup.gameObject.SetActive(true);
            }
            else if (command is Spell)
            {
                Spell card = (Spell)command;
                SkillInfoGroup.SetData(card);
                SkillInfoGroup.gameObject.SetActive(true);
            }
            else if (command is Consumables)
            {
                Consumables consumbles = (Consumables)command;
                SkillInfoGroup.SetData(consumbles);
                SkillInfoGroup.gameObject.SetActive(true);
            }
            else if (command is Food)
            {
                Food food = (Food)command;
                SkillInfoGroup.SetData(food);
                SkillInfoGroup.gameObject.SetActive(true);
            }
        }
    }

    private void HideSkillInfo(ButtonPlus buttonPlus)
    {
        SkillInfoGroup.gameObject.SetActive(false);
    }

    private void Awake()
    {
        MoveButton.onClick.AddListener(MoveOnClick);
        SkillButton.ClickHandler += SkillOnClick;
        SupportButton.ClickHandler += SupportOnClick;
        SpellButton.ClickHandler += SpellOnClick;
        ItemButton.ClickHandler += ItemOnClick;
        IdleButton.onClick.AddListener(IdleOnClick);
        ResetButton.onClick.AddListener(ResetOnClick);
        ScrollView.ClickHandler += ScrollItemOnClick;
        ScrollView.EnterHandler += ShowInfo;
        ScrollView.ExitHandler += HideSkillInfo;
        ScrollView.transform.parent.gameObject.SetActive(false);
        SkillInfoGroup.gameObject.SetActive(false);
    }
}
