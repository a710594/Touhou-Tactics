using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    public partial class BattleController
    {
        private class EffectState : BattleControllerState
        {
            int _maxFloatingCount;
            private List<BattleCharacterInfo> _targetList;
            private Timer _timer = new Timer();
            private Effect effect;

            public EffectState(StateContext context) : base(context)
            {
            }

            public override void Begin()
            {
                Instance._battleUI.SetCharacterInfoUI_2(null);
                Instance.ClearQuad();
                _characterList = Instance.CharacterList;
                _character = Instance.SelectedCharacter;
                _targetList = new List<BattleCharacterInfo>();
                for (int i = 0; i < _characterList.Count; i++)
                {
                    if (CheckEffectArea(Instance._areaList, Utility.ConvertToVector2Int(_characterList[i].Position)))
                    {
                        _targetList.Add(_characterList[i]);
                    }

                }
                for (int i=0; i<Instance.DyingList.Count; i++) 
                {
                    if (CheckEffectArea(Instance._areaList, Utility.ConvertToVector2Int(Instance.DyingList[i].Position)))
                    {
                        _targetList.Add(Instance.DyingList[i]);
                    }
                }
                if (_targetList.Count > 0)
                {
                    _maxFloatingCount = 0;
                    if (_character.SelectedObject is Skill)
                    {
                        Skill skill = (Skill)_character.SelectedObject;
                        for (int i = 0; i < _targetList.Count; i++)
                        {
                            if (skill.Effect.Type == EffectModel.TypeEnum.Summon)
                            {
                                Vector3 v3 = new Vector3(Instance._selectedPosition.x, Instance.Info.TileAttachInfoDic[Instance._selectedPosition].Height, Instance._selectedPosition.y);
                                UseEffect(skill.Effect, v3);
                            }
                            else
                            {
                                UseEffect(skill.Effect, _targetList[i]);
                            }
                        }
                        _character.HasUseSkill = true;
                        _character.ActionCount--;
                        if (_character.CurrentPP < BattleCharacterInfo.MaxPP)
                        {
                            _character.CurrentPP++;
                        }
                        if (skill.Data.CD > 0)
                        {
                            skill.CurrentCD = skill.Data.CD + 1; //要加一的原因是為了抵銷本回合的 CheckCD
                        }
                    }
                    else if(_character.SelectedObject is Support) 
                    {
                        Support support = (Support)_character.SelectedObject;
                        for (int i = 0; i < _targetList.Count; i++)
                        {
                            UseEffect(support.Effect, _targetList[i]);
                        }
                        _character.HasUseSupport = true;
                        if (_character.CurrentPP < BattleCharacterInfo.MaxPP)
                        {
                            _character.CurrentPP++;
                        }
                        if (support.Data.CD > 0)
                        {
                            support.CurrentCD = support.Data.CD + 1; //要加一的原因是為了抵銷本回合的 CheckCD
                        }
                    }
                    else if(_character.SelectedObject is Card) 
                    {
                        Card card = (Card)_character.SelectedObject;
                        for (int i = 0; i < _targetList.Count; i++)
                        {
                            UseEffect(card.Effect, _targetList[i]);
                        }
                        _character.HasUseItem = true;
                        _character.ActionCount--;
                        _character.CurrentPP -= card.CardData.PP;
                        ItemManager.Instance.MinusItem(card.ID, 1);
                    }
                    else if(_character.SelectedObject is Consumables) 
                    {
                        Consumables consumables = (Consumables)_character.SelectedObject;
                        for (int i = 0; i < _targetList.Count; i++)
                        {
                            UseEffect(consumables.Effect, _targetList[i]);
                        }
                        _character.HasUseItem = true;
                        _character.ActionCount--;
                        ItemManager.Instance.MinusItem(consumables.ID, 1);
                    }
                    else if(_character.SelectedObject is Food) 
                    {
                        Food food = (Food)_character.SelectedObject;
                        for (int i = 0; i < _targetList.Count; i++)
                        {
                            List<FloatingNumberData> floatingList = new List<FloatingNumberData>();
                            food.Effect.Use(_character, _targetList[i], floatingList, _characterList);
                            SetUI(_targetList[i], floatingList);
                        }
                        _character.HasUseItem = true;
                        _character.ActionCount--;
                        ItemManager.Instance.MinusItem(food.ID, 1);
                    }

                    _timer.Start(_maxFloatingCount, CheckResult);
                }
                else
                {
                    if (_character.SelectedObject is Skill && ((Skill)_character.SelectedObject).Effect.Type == EffectModel.TypeEnum.Summon)
                    {
                        Skill skill = (Skill)_character.SelectedObject;
                        Vector3 v3 = new Vector3(Instance._selectedPosition.x, Instance.Info.TileAttachInfoDic[Instance._selectedPosition].Height, Instance._selectedPosition.y);
                        UseEffect(skill.Effect, v3);
                        _character.HasUseSkill = true;
                        _character.ActionCount--;
                        if (_character.CurrentPP < BattleCharacterInfo.MaxPP)
                        {
                            _character.CurrentPP++;
                        }
                        if (skill.Data.CD > 0)
                        {
                            skill.CurrentCD = skill.Data.CD + 1; //要加一的原因是為了抵銷本回合的 CheckCD
                        }
                        _timer.Start(_maxFloatingCount, CheckResult);
                    }
                    else
                    {
                        Instance._battleUI.TipLabel.SetLabel("毫無反應...");
                        if (_character.SelectedObject is Skill)
                        {
                            _character.HasUseSkill = true;
                            _character.ActionCount--;
                        }
                        else if (_character.SelectedObject is Support)
                        {
                            _character.HasUseSupport = true;
                        }
                        else if (_character.SelectedObject is Card || _character.SelectedObject is Consumables || _character.SelectedObject is Food)
                        {
                            _character.HasUseItem = true;
                            _character.ActionCount--;
                        }
                        _context.SetState<EndState>();
                    }
                }
            }

            private void UseEffect(Effect effect, BattleCharacterInfo target) 
            {
                List<FloatingNumberData> floatingList = new List<FloatingNumberData>();
                effect.Use(_character, target, floatingList, _characterList);
                SetUI(target, floatingList);
            }

            public void UseEffect(Effect effect, Vector3 position) 
            {
                effect.Use(_character, position);
            }

            private void SetUI(BattleCharacterInfo target, List<FloatingNumberData> floatingList) 
            {
                Instance._battleUI.SetLittleHpBarValue(target.Index, target);
                for (int j = 0; j < floatingList.Count; j++)
                {
                    Instance._battleUI.PlayFloatingNumberPool(target.Index, floatingList[j].Type, floatingList[j].Text);
                }
                if (floatingList.Count > _maxFloatingCount)
                {
                    _maxFloatingCount = floatingList.Count;
                }
            }

            private void CheckResult()
            {
                for (int i = 0; i < _targetList.Count; i++)
                {
                    if (_targetList[i].CurrentHP <= 0)
                    {
                        if (_targetList[i].Faction == BattleCharacterInfo.FactionEnum.Player)
                        {
                            if (Instance.DyingList.Contains(_targetList[i]))
                            {
                                Instance._controllerDic[_targetList[i].Index].gameObject.SetActive(false);
                                Instance.DyingList.Remove(_targetList[i]);
                                Instance.DeadList.Add(_targetList[i]);
                            }
                            else
                            {
                                _characterList.Remove(_targetList[i]);
                                Instance.DyingList.Add(_targetList[i]);
                                Instance._controllerDic[_targetList[i].Index].SetGray(true);
                            }
                        }
                        else
                        {
                            _characterList.Remove(_targetList[i]);
                            Instance._controllerDic[_targetList[i].Index].gameObject.SetActive(false);
                        }
                    }
                    else if (Instance.DyingList.Contains(_targetList[i])) 
                    {
                        _characterList.Add(_targetList[i]);
                        Instance.DyingList.Remove(_targetList[i]);
                        Instance._controllerDic[_targetList[i].Index].SetGray(false);
                    }
                }

                int playerCount = 0;
                int enemyCount = 0;
                for (int i = 0; i < _characterList.Count; i++)
                {
                    if (_characterList[i].Faction == BattleCharacterInfo.FactionEnum.Player)
                    {
                        playerCount++;
                    }
                    else
                    {
                        enemyCount++;
                    }
                }

                if (playerCount == 0)
                {
                    _context.SetState<LoseState>();
                }
                else if (enemyCount == 0)
                {
                    _context.SetState<WinState>();
                }
                else
                {
                    if (_character.ActionCount > 0)
                    {
                        _context.SetState<ActionState>();
                    }
                    else
                    {
                        _context.SetState<EndState>();
                    }
                }
            }
        }
    }
}