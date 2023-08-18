using Battle;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillGroup : MonoBehaviour
{
    public PointGroup SPGroup;
    public PointGroup PPGroup;
    public ScrollView ScrollView;

    public void SetData(List<Skill> skillList)
    {
        List<object> list = new List<object>(skillList);
        ScrollView.SetData(list);
        SPGroup.gameObject.SetActive(false);
        PPGroup.gameObject.SetActive(false);
    }

    public void SetData(List<Support> supportList, int currentSP)
    {
        List<object> list = new List<object>(supportList);
        ScrollView.SetData(list);
        SPGroup.gameObject.SetActive(true);
        SPGroup.SetData(currentSP);
        PPGroup.gameObject.SetActive(false);
    }

    public void SetData(BattleCharacterInfo character)
    {
        List<object> list = new List<object>(ItemManager.Instance.GetItemList(character));
        ScrollView.SetData(list);
        PPGroup.gameObject.SetActive(true);
        PPGroup.SetData(character.CurrentPP);
        SPGroup.gameObject.SetActive(false);
    }

    private void ScrollItemOnClick(object obj, ScrollItem scrollItem)
    {
        if (obj is Skill)
        {
            BattleController.Instance.SelectSkill((Skill)obj);
        }
        else if (obj is Support)
        {
            BattleController.Instance.SelectSupport((Support)obj);
        }
        else if (obj is Item)
        {
            BattleController.Instance.SelectItem((Item)obj);
        }
    }

    private void Awake()
    {
        ScrollView.Init();
        ScrollView.ItemOnClickHandler += ScrollItemOnClick;
    }
}
