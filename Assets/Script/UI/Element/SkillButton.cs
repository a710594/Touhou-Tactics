using Battle;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Button Button;
    public Text NameLabel;
    public Text CommentLabel;

    private Skill _skill;
    private Support _support;

    public void SetData(Skill skill) 
    {
        _skill = skill;
        _support = null;
        NameLabel.text = skill.Data.Name;
        if (skill.CurrentCD == 0)
        {
            Button.interactable = true;
        }
        else 
        {
            Button.interactable = false;
        }
    }

    public void SetData(Support support, int currentSP) //雖然 support 和 skill 是不一樣的東西,但是 UI 共用
    {
        _support = support;
        _skill = null;
        NameLabel.text = support.Data.Name;
        if (support.Data.SP> currentSP)
        {
            Button.interactable = false;
        }
        else
        {
            Button.interactable = true;
        }
    }

    private void OnClick() 
    {
        if (_skill != null)
        {
            BattleController.Instance.SelectSkill(_skill);
        }
        else if (_support != null) 
        {
            BattleController.Instance.SelectSupport(_support);
        }
    }

    private void Awake()
    {
        //CommentLabel.gameObject.SetActive(false);
        Button.onClick.AddListener(OnClick);    
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //CommentLabel.gameObject.SetActive(true);
        //CommentLabel.text = _skill.Data.Comment;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //CommentLabel.gameObject.SetActive(false);
    }
}
