using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillButton : MonoBehaviour
{
    public Action<Skill> ClickHandler;

    public Button Button;

    private Skill _skill;

    public void SetData(Skill skill) 
    {
        _skill = skill;
    }

    private void OnClick() 
    {
        if(ClickHandler!= null) 
        {
            ClickHandler(_skill);
        }
    }

    private void Awake()
    {
        Button.onClick.AddListener(OnClick);    
    }
}
