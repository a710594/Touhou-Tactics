using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillButton : MonoBehaviour
{
    public Button Button;
    public Text Label;

    private Skill _skill;

    public void SetData(Skill skill) 
    {
        _skill = skill;
        Label.text = skill.Data.Name;
    }

    private void OnClick() 
    {
        BattleController.Instance.SelectSkill(_skill);
    }

    private void Awake()
    {
        Button.onClick.AddListener(OnClick);    
    }
}
