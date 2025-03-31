using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommandMainGroup : MonoBehaviour
{
    /*public DropdownButton SkillButton;
    public DropdownButton ItemButton;
    public DropdownButton SpellButton;
    public GridLayoutGroup GridLayout;

    private DropdownGroup _showedDropdownGroup = null;

    public void Clear() 
    {
        if (_showedDropdownGroup != null)
        {
            _showedDropdownGroup.Clear();
        }
    }

    private void SkillOnEnter() 
    {
        if (_showedDropdownGroup != null && _showedDropdownGroup != SkillButton.DropdownGroup)
        {
            _showedDropdownGroup.gameObject.SetActive(false);
        }

        _showedDropdownGroup = SkillButton.DropdownGroup;
        SkillButton.DropdownGroup.gameObject.SetActive(true);
        SkillButton.DropdownGroup.SetPosition(SkillButton.transform, GridLayout);

        List<object> list = new List<object>();
        list.Add("Skill 1");
        list.Add("Skill 2");
        SkillButton.DropdownGroup.SetData(list);
    }

    private void ItemOnEnter()
    {
        if (_showedDropdownGroup != null && _showedDropdownGroup != ItemButton.DropdownGroup)
        {
            _showedDropdownGroup.gameObject.SetActive(false);
        }

        _showedDropdownGroup = ItemButton.DropdownGroup;
        ItemButton.DropdownGroup.gameObject.SetActive(true);
        ItemButton.DropdownGroup.SetPosition(ItemButton.transform, GridLayout);

        List<object> list = new List<object>();
        list.Add("Item 1");
        list.Add("Item 2");
        ItemButton.DropdownGroup.SetData(list);
    }

    private void SpellOnEnter()
    {
        if (_showedDropdownGroup != null && _showedDropdownGroup != SpellButton.DropdownGroup)
        {
            _showedDropdownGroup.gameObject.SetActive(false);
        }

        _showedDropdownGroup = SpellButton.DropdownGroup;
        SpellButton.DropdownGroup.gameObject.SetActive(true);
        SpellButton.DropdownGroup.SetPosition(SpellButton.transform, GridLayout);

        List<object> list = new List<object>();
        list.Add("Spell 1");
        list.Add("Spell 2");
        SpellButton.DropdownGroup.SetData(list);
    }

    private void Awake()
    {
        SkillButton.EnterHandler += SkillOnEnter;
        ItemButton.EnterHandler += ItemOnEnter;
        SpellButton.EnterHandler += SpellOnEnter;
    }*/
}
