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

    public void SetData(Skill skill, BattleCharacterInfo info) 
    {
        _skill = skill;
        NameLabel.text = skill.Data.Name;
        if (skill.CurrentCD == 0)
        {
            Button.interactable = false;
        }
        else if(skill.Data.Type == SkillModel.TypeEnum.Support && skill.Data.SP> info.CurrentSP)
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
        BattleController.Instance.SelectSkill(_skill);
    }

    private void Awake()
    {
        CommentLabel.gameObject.SetActive(false);
        Button.onClick.AddListener(OnClick);    
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        CommentLabel.gameObject.SetActive(true);
        CommentLabel.text = _skill.Data.Comment;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        CommentLabel.gameObject.SetActive(false);
    }
}
