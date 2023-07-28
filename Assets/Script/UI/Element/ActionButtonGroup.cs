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
    public Button IdleButton;
    public Button ResetButton;

    public void SetButton(BattleCharacterInfo character) 
    {
        SkillButton.interactable = !character.HasUseSkill;
        SupportButton.interactable = !character.HasUseSupport;
    }

    private void MoveOnClick() 
    {
        BattleController.Instance.SetMoveState();
    }

    private void SkillOnClick() 
    {
        BattleController.Instance.SetSelectSkillState(SkillModel.TypeEnum.Normal);
    }

    private void SupportOnClick()
    {
        BattleController.Instance.SetSelectSkillState(SkillModel.TypeEnum.Support);
    }

    private void IdleOnClick() 
    {
        BattleController.Instance.Idle();
    }

    private void ResetOnClick() 
    {
        BattleController.Instance.ResetAction();
    }

    private void Awake()
    {
        MoveButton.onClick.AddListener(MoveOnClick);
        SkillButton.onClick.AddListener(SkillOnClick);
        SupportButton.onClick.AddListener(SupportOnClick);
        IdleButton.onClick.AddListener(IdleOnClick);
        ResetButton.onClick.AddListener(ResetOnClick);
    }
}
