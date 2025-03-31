using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Battle
{
    public class CommandGroup : DropdownRoot
    {
        public DropdownButton MoveButton;
        public DropdownButton SubButton;
        public DropdownButton MainButton;
        public DropdownButton FinishButton;
        public GridLayoutGroup GridLayout;
        public TipLabel TipLabel;
        public SkillInfoGroup SkillInfoGroup;

        //private DropdownGroup _showedDropdownGroup = null;
        private DropdownNode _lastNode = null;
        private BattleCharacterInfo _character;

        public void SetData(BattleCharacterInfo character) 
        {
            _character = character;

            List<object> subList = new List<object>(_character.SubList);
            SubButton.SetData("次要動作", ConvertToPair(subList), this);
            SubButton.SetHasUse(false);

            List<object> skillList = new List<object>(_character.SkillList);
            List<object> itemList = ItemManager.Instance.GetBattleItemList();
            List<object> spellList = new List<object>(_character.SpellList);
            List<KeyValuePair<string, object>> mainList = new List<KeyValuePair<string, object>>();
            mainList.Add(new KeyValuePair<string, object>("技能", ConvertToPair(skillList)));
            mainList.Add(new KeyValuePair<string, object>("道具", ConvertToPair(itemList)));
            mainList.Add(new KeyValuePair<string, object>("符卡", ConvertToPair(spellList)));
            MainButton.SetData("主要動作", mainList, this);
            MainButton.SetHasUse(false);
        }

        public void Reset() 
        {
            SubButton.DropdownGroup.Clear();
            MainButton.DropdownGroup.Clear();

            SubButton.DropdownGroup.gameObject.SetActive(false);
            MainButton.DropdownGroup.gameObject.SetActive(false);

            SubButton.SetHasUse(_character.HasSub);
            MainButton.SetHasUse(_character.HasMain);

            if (_character.HasMove)
            {
                MoveButton.Button.Label.text = "再次移動";
                if (_character.MoveAgain)
                {
                    MoveButton.SetHasUse(true);
                    SubButton.SetHasUse(true);
                    MainButton.SetHasUse(true);
                }
                else
                {
                    if (_character.HasSub || _character.HasMain)
                    {
                        MoveButton.SetHasUse(true);
                    }
                    else
                    {
                        MoveButton.SetHasUse(false);
                    }
                }
            }
            else
            {
                MoveButton.Button.Label.text = "移動";
                MoveButton.SetHasUse(false);
            }
            FinishButton.SetHasUse(false);
        }

        public override void ButtonOnClick(object data) 
        {
            if(data is string &&  (string)data == "Move") 
            {
                if (_character.MoveAgain)
                {
                    TipLabel.SetLabel("這回合已經使用過再次移動了");
                }
                else if (_character.HasMove)
                {
                    if (_character.HasSub)
                    {
                        TipLabel.SetLabel("這回合已經使用過次要動作了");
                    }
                    else if (_character.HasMain)
                    {
                        TipLabel.SetLabel("這回合已經使用過主要動作了");
                    }
                }
                else
                {
                    BattleController.Instance.SetState<BattleController.MoveState>();
                }
            }
            else if (data is Sub)
            {
                Sub sub = (Sub)data;
                if (_character.HasSub)
                {
                    TipLabel.SetLabel("這回合已經使用過次要動作了");
                }
                else if (sub.CurrentCD > 0)
                {
                    TipLabel.SetLabel("還需要" + sub.CurrentCD + "回合冷卻");
                }
                else if (_character.MoveAgain) 
                {
                    TipLabel.SetLabel("這回合已經使用過再次移動了");
                }
                else
                {
                    BattleController.Instance.SetSelectedCommand(sub);
                }
            }
            else if(data is Skill) 
            {
                Skill skill = (Skill)data;
                if (_character.HasMain) 
                {
                    TipLabel.SetLabel("這回合已經使用過主要動作了");
                }
                else if (skill.CurrentCD > 0) 
                {
                    TipLabel.SetLabel("還需要" + skill.CurrentCD + "回合冷卻");
                }
                else if (_character.MoveAgain)
                {
                    TipLabel.SetLabel("這回合已經使用過再次移動了");
                }
                else
                {
                    BattleController.Instance.SetSelectedCommand(skill);
                }
            }
            else if(data is ItemCommand) 
            {
                if (_character.MoveAgain)
                {
                    TipLabel.SetLabel("這回合已經使用過再次移動了");
                }
                else
                {
                    ItemCommand item = (ItemCommand)data;
                    BattleController.Instance.SetSelectedCommand(item);
                }
            }
            else if(data is Spell) 
            {
                Spell spell = (Spell)data;
                if (_character.HasSpell)
                {
                    TipLabel.SetLabel("這回合已經使用過主要動作了");
                }
                else if (!_character.CanUseSpell)
                {
                    TipLabel.SetLabel("下次行動才能使用符卡");
                }
                else
                {
                    BattleController.Instance.SetSelectedCommand(spell);
                }
            }
            else if(data is string && (string)data == "Finish") 
            {
                BattleController.Instance.SetState<BattleController.DirectionState>();
            }

            SkillInfoGroup.gameObject.SetActive(false);
        }

        public override void ButtonOnEnter(object data, DropdownButton button, DropdownGroup group) 
        {

            //if (_showedDropdownGroup != null && group.transform.parent.parent != _showedDropdownGroup.transform) 
            //{
            //    _showedDropdownGroup.gameObject.SetActive(false);
            //}

            while(_lastNode!= null && _lastNode != button.Parent) 
            {
                if (_lastNode is DropdownButton)
                {
                    ((DropdownButton)_lastNode).DropdownGroup.gameObject.SetActive(false);
                }
                _lastNode = _lastNode.Parent;
            }

            if (data is List<KeyValuePair<string, object>>)
            {
                group.gameObject.SetActive(true);
                group.SetPosition(button.transform, GridLayout);

                bool hasUse = false;
                if(((button == MainButton || button.Parent == MainButton) && _character.HasMain) ||
                    (button == SubButton && _character.HasSub) ||
                    ((button == MainButton || button.Parent == MainButton || button == SubButton) && _character.MoveAgain)) 
                {
                    hasUse = true;
                }
                group.SetData(this, button, hasUse, (List<KeyValuePair<string, object>>)data);

                if (button == SubButton || button == MainButton)
                {
                    List<DropdownButton> buttonList = group.ButtonList;
                    for (int i=0; i<buttonList.Count; i++) 
                    {
                        if(buttonList[i].Data is Sub) 
                        {
                            Sub sub = (Sub)buttonList[i].Data;
                            if (sub.CurrentCD > 0) 
                            {
                                buttonList[i].SetHasUse(true);
                            }
                        }
                        else if (buttonList[i].Data is Skill)
                        {
                            Skill skill = (Skill)buttonList[i].Data;
                            if (skill.CurrentCD > 0)
                            {
                                buttonList[i].SetHasUse(true);
                            }
                        }
                    }
                }

            }
            else if(data is Sub)
            {
                SkillInfoGroup.SetData((Sub)data);
                SkillInfoGroup.gameObject.SetActive(true);
                SkillInfoGroup.transform.position = new Vector3(button.transform.position.x - 285, SkillInfoGroup.transform.position.y, SkillInfoGroup.transform.position.z);
            }
            else if (data is Skill)
            {
                SkillInfoGroup.SetData((Skill)data);
                SkillInfoGroup.gameObject.SetActive(true);
                SkillInfoGroup.transform.position = new Vector3(button.transform.position.x - 285, SkillInfoGroup.transform.position.y, SkillInfoGroup.transform.position.z);
            }
            else if (data is ItemCommand)
            {
                SkillInfoGroup.SetData((ItemCommand)data);
                SkillInfoGroup.gameObject.SetActive(true);
                SkillInfoGroup.transform.position = new Vector3(button.transform.position.x - 285, SkillInfoGroup.transform.position.y, SkillInfoGroup.transform.position.z);
            }
            else if (data is Spell)
            {
                SkillInfoGroup.SetData((Spell)data);
                SkillInfoGroup.gameObject.SetActive(true);
                SkillInfoGroup.transform.position = new Vector3(button.transform.position.x - 285, SkillInfoGroup.transform.position.y, SkillInfoGroup.transform.position.z);
            }

            _lastNode = button;
        }

        public override void ButtonOnExit()
        {
            SkillInfoGroup.gameObject.SetActive(false);
        }

        private List<KeyValuePair<string, object>> ConvertToPair(List<object> list) 
        {
            List<KeyValuePair<string, object>> result = new List<KeyValuePair<string, object>>();
            for (int i=0; i<list.Count; i++) 
            {
                if(list[i] is Sub) 
                {
                    Sub sub = (Sub)list[i];
                    result.Add(new KeyValuePair<string, object>(sub.Name, sub));
                }
                else if (list[i] is Skill)
                {
                    Skill skill = (Skill)list[i];
                    result.Add(new KeyValuePair<string, object>(skill.Name, skill));
                }
                else if (list[i] is ItemCommand)
                {
                    ItemCommand item = (ItemCommand)list[i];
                    result.Add(new KeyValuePair<string, object>(item.Name, item));
                }
                else if (list[i] is Spell)
                {
                    Spell spell = (Spell)list[i];
                    result.Add(new KeyValuePair<string, object>(spell.Name, spell));
                }
            }
            return result;
        }

        private void Awake()
        {
            MoveButton.SetData("移動", "Move", this);
            MoveButton.SetHasUse(false);
            FinishButton.SetData("結束", "Finish", this);
            FinishButton.SetHasUse(false);

            ChildList.Add(MoveButton);
            ChildList.Add(SubButton);
            ChildList.Add(MainButton);
            ChildList.Add(FinishButton);

            MoveButton.Parent = this;
            SubButton.Parent = this;
            MainButton.Parent = this;
            FinishButton.Parent = this;
        }
    }
}