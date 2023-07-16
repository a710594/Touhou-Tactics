using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillButtonGroup : MonoBehaviour
{
    public SPGroup SPGroup;
    public Button BackButton;
    public SkillButton[] SkillButtons;

    public void SetVisible(bool isVisible)
    {
        gameObject.SetActive(isVisible);
    }

    public void SetData(BattleCharacterInfo info, SkillModel.TypeEnum type) 
    {
        for(int i=0; i< SkillButtons.Length; i++) 
        {
            if (i < info.SkillDic[type].Count) 
            {
                SkillButtons[i].gameObject.SetActive(true);
                SkillButtons[i].SetData(info.SkillDic[type][i], info);
            }
            else 
            {
                SkillButtons[i].gameObject.SetActive(false);
            }
        }

        if(type == SkillModel.TypeEnum.Support) 
        {
            SPGroup.gameObject.SetActive(true);
            SPGroup.SetData(info.CurrentSP);
        }
        else 
        {
            SPGroup.gameObject.SetActive(false);
        }
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
