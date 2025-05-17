using Battle;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/*public class ActionButtonGroup : MonoBehaviour
{
    public ButtonPlus MoveButton;
    public ButtonPlus SubButton;
    public ButtonPlus MainButton;
    public Button FinishButton;
    public ActionButton SkillButton;
    public ActionButton SpellButton;
    public ActionButton ItemButton;
    public GameObject MainGroup;
    public ScrollView ScrollView;
    public ScrollView SubScrollView;
    public SkillInfoGroup SkillInfoGroup;
    public TipLabel TipLabel;

    public void SetButton(BattleCharacterInfo character) 
    {
        SkillButton.SetColor(character);
        ItemButton.SetColor(character);
        SpellButton.SetColor(character);
        ScrollView.transform.parent.gameObject.SetActive(false);
    }

    public void SetScrollView(List<object> list)
    {
        ScrollView.SetData(list);
    }

    private void MoveOnClick(PointerEventData eventData, ButtonPlus buttonPlus) 
    {
        if (BattleController.Instance.IsTutorialActive && !BattleController.Instance.Tutorial.CheckMove())
        {
            return;
        }
        BattleController.Instance.SetState<BattleController.MoveState>();
    }

    private void SkillOnClick() 
    {
        if (BattleController.Instance.IsTutorialActive && !BattleController.Instance.Tutorial.CheckSkill())
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

    private void SubOnEnter(ButtonPlus buttonPlus)
    {
        if (BattleController.Instance.IsTutorialActive && !BattleController.Instance.Tutorial.CheckSupport())
        {
            return;
        }

        BattleCharacterInfo character = BattleController.Instance.SelectedCharacter.Info;
        if (!character.IsAuto)
        {
            SubScrollView.transform.gameObject.SetActive(true);
            SubScrollView.SetData(new List<object>(character.SubList));
        }
    }

    private void SubOnExit(ButtonPlus buttonPlus) 
    {
        SubScrollView.transform.gameObject.SetActive(false);
    }

    private void SpellOnClick()
    {
        if (BattleController.Instance.IsTutorialActive && !BattleController.Instance.Tutorial.CheckSpell())
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
        if (BattleController.Instance.IsTutorialActive && !BattleController.Instance.Tutorial.CheckItem())
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
        if (BattleController.Instance.IsTutorialActive && !BattleController.Instance.Tutorial.CheckIdle())
        {
            return;
        }

        BattleController.Instance.SetState<BattleController.EndState>();
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
        else if (command is Sub)
        {
            Sub support = (Sub)command;
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
            //BattleController.Instance.SetState<BattleController.TargetState>();
        }
        else
        {
            TipLabel.SetLabel(tip);
        }
    }

    private void ShowInfo(ButtonPlus buttonPlus)
    {
        //if(!BattleController.Instance.IsTutorialActive)
        //{
        //    Command command = (Command)buttonPlus.Data;
        //    if (command is Skill)
        //    {
        //        Skill skill = (Skill)command;
        //        SkillInfoGroup.SetData(skill);
        //        SkillInfoGroup.gameObject.SetActive(true);
        //    }
        //    else if (command is Sub)
        //    {
        //        Sub support = (Sub)command;
        //        SkillInfoGroup.SetData(support);
        //        SkillInfoGroup.gameObject.SetActive(true);
        //    }
        //    else if (command is Spell)
        //    {
        //        Spell card = (Spell)command;
        //        SkillInfoGroup.SetData(card);
        //        SkillInfoGroup.gameObject.SetActive(true);
        //    }
        //    else if (command is Battle.Item)
        //    {
        //        Battle.Item consumbles = (Battle.Item)command;
        //        SkillInfoGroup.SetData(consumbles);
        //        SkillInfoGroup.gameObject.SetActive(true);
        //    }
        //    else if (command is Food)
        //    {
        //        Food food = (Food)command;
        //        SkillInfoGroup.SetData(food);
        //        SkillInfoGroup.gameObject.SetActive(true);
        //    }
        //}
    }

    private void HideSkillInfo(ButtonPlus buttonPlus)
    {
        SkillInfoGroup.gameObject.SetActive(false);
    }

    private void Awake()
    {
        MoveButton.ClickHandler += MoveOnClick;
        SubButton.EnterHandler += SubOnEnter;
        SubButton.ExitHandler += SubOnExit;
        SkillButton.ClickHandler += SkillOnClick;
        SpellButton.ClickHandler += SpellOnClick;
        ItemButton.ClickHandler += ItemOnClick;
        FinishButton.onClick.AddListener(IdleOnClick);
        ScrollView.ClickHandler += ScrollItemOnClick;
        ScrollView.EnterHandler += ShowInfo;
        ScrollView.ExitHandler += HideSkillInfo;
        MainGroup.SetActive(false);
        ScrollView.transform.parent.gameObject.SetActive(false);
        SkillInfoGroup.gameObject.SetActive(false);
        SubScrollView.Init();
    }
}*/
