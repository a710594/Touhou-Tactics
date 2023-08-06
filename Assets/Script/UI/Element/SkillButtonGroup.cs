using Battle;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillButtonGroup : MonoBehaviour
{
    public PointGroup SPGroup;
    public Button BackButton;
    public SkillButton[] SkillButtons;

    public void SetVisible(bool isVisible)
    {
        gameObject.SetActive(isVisible);
    }

    public void SetData(List<Skill> skillList) 
    {
        for(int i=0; i< SkillButtons.Length; i++) 
        {
            if (i < skillList.Count) 
            {
                SkillButtons[i].gameObject.SetActive(true);
                SkillButtons[i].SetData(skillList[i]);
            }
            else 
            {
                SkillButtons[i].gameObject.SetActive(false);
            }
        }
        SPGroup.gameObject.SetActive(false);
    }

    public void SetData(List<Support> supportList, int currentSP)
    {
        for (int i = 0; i < SkillButtons.Length; i++)
        {
            if (i < supportList.Count)
            {
                SkillButtons[i].gameObject.SetActive(true);
                SkillButtons[i].SetData(supportList[i], currentSP);
            }
            else
            {
                SkillButtons[i].gameObject.SetActive(false);
            }
        }
        SPGroup.gameObject.SetActive(true);
        SPGroup.SetData(currentSP);
    }

    private void BackOnClick() 
    {
        gameObject.SetActive(false);
        BattleController.Instance.SetActionState();
    }

    private void Awake()
    {
        BackButton.onClick.AddListener(BackOnClick);
    }
}
