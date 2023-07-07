using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionButtonGroup : MonoBehaviour
{
    public Button MoveButton;
    public Button SkillButton;
    public Button IdleButton;

    private void MoveOnClick() 
    {
        BattleController.Instance.SetMoveState();
    }

    private void SkillOnClick() 
    {
        BattleController.Instance.SetSelectSkillState();
    }

    private void IdleOnClick() 
    {
        BattleController.Instance.Idle();
    }

    private void Awake()
    {
        MoveButton.onClick.AddListener(MoveOnClick);
        SkillButton.onClick.AddListener(SkillOnClick);
        IdleButton.onClick.AddListener(IdleOnClick);
    }
}
