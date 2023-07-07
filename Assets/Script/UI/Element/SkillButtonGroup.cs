using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillButtonGroup : MonoBehaviour
{
    public Button BackButton;
    public SkillButton[] SkillButtons;

    public void SetVisible(bool isVisible)
    {
        gameObject.SetActive(isVisible);
    }

    public void SetData(BattleCharacterInfo info) 
    {
        for(int i=0; i< SkillButtons.Length; i++) 
        {
            if (i < info.SkillList.Count) 
            {
                SkillButtons[i].gameObject.SetActive(true);
                SkillButtons[i].SetData(info.SkillList[i]);
            }
            else 
            {
                SkillButtons[i].gameObject.SetActive(false);
            }
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
