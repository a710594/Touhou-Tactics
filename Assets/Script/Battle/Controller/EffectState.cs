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

                _maxFloatingCount = 0;
                if (_character.SelectedObject is Skill)
                {
                    Skill skill = (Skill)_character.SelectedObject;
                    if(skill.Effect.Type == EffectModel.TypeEnum.Summon)
                    {
                        Vector3 v3 = new Vector3(Instance._selectedPosition.x, Instance.Info.TileAttachInfoDic[Instance._selectedPosition].Height, Instance._selectedPosition.y);
                        UseEffect(skill.Effect, v3);
                    }
                    else
                    {
                        for (int i = 0; i < _targetList.Count; i++)
                        {
                            UseEffect_New(skill.Effect, _targetList[i]);
                        }
                    }
                    _character.HasUseSkill = true;
                    _character.ActionCount--;
                    if (skill.Data.CD > 0)
                    {
                        skill.CurrentCD = skill.Data.CD + 1; // n [ @    ] O   F  P   ^ X   CheckCD
                    }
                }
                else if(_character.SelectedObject is Support) 
                {
                    Support support = (Support)_character.SelectedObject;
                    for (int i = 0; i < _targetList.Count; i++)
                    {
                        UseEffect_New(support.Effect, _targetList[i]);
                    }
                    _character.HasUseSupport = true;
                    if (support.Data.CD > 0)
                    {
                        support.CurrentCD = support.Data.CD + 1; // n [ @    ] O   F  P   ^ X   CheckCD
                    }
                }
                else if(_character.SelectedObject is Spell) 
                {
                    Spell card = (Spell)_character.SelectedObject;
                    for (int i = 0; i < _targetList.Count; i++)
                    {
                        UseEffect_New(card.Effect, _targetList[i]);
                    }
                    _character.HasUseItem = true;
                    _character.ActionCount--;
                    if (card.Data.CD > 0)
                    {
                        for(int i=0; i<_characterList.Count; i++)
                        {
                            if(_characterList[i].Faction == BattleCharacterInfo.FactionEnum.Player)
                            {
                                Debug.Log(_characterList[i].Name);
                                for(int j=0; j<_characterList[i].CardList.Count; j++)
                                {
                                    _characterList[i].CardList[j].CurrentCD = card.Data.CD + 1;
                                }
                            }
                        }
                        //card.CurrentCD = card.Data.CD + 1; // n [ @    ] O   F  P   ^ X   CheckCD
                    }
                    ItemManager.Instance.MinusItem(ItemManager.CardID, 1);
                }
                else if(_character.SelectedObject is Consumables) 
                {
                    Consumables consumables = (Consumables)_character.SelectedObject;
                    for (int i = 0; i < _targetList.Count; i++)
                    {
                        UseEffect_New(consumables.Effect, _targetList[i]);
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
                        // List<FloatingNumberData> floatingList = new List<FloatingNumberData>();
                        // food.Effect.Use(_character, _targetList[i], floatingList, _characterList);
                        // SetUI(_targetList[i], floatingList);
                        UseEffect_New(food.Effect, _targetList[i]);
                    }
                    _character.HasUseItem = true;
                    _character.ActionCount--;
                    ItemManager.Instance.MinusItem(food.ID, 1);
                }

                _timer.Start(_maxFloatingCount, CheckResult);
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

            private void UseEffect_New(Effect effect, BattleCharacterInfo target) 
            {
                List<Log> logList = new List<Log>();
                effect.Use(_character, target, logList);
                SetUI(target, logList);

                string targetName;
                if(_character == target)
                {
                    targetName = "自己";
                }
                else
                {
                    targetName = target.Name;
                }
                for(int i=0; i<logList.Count; i++)
                {
                    if (logList[i].Type == EffectModel.TypeEnum.MagicAttack || logList[i].Type == EffectModel.TypeEnum.PhysicalAttack)
                    {
                        if (logList[i].HitType == HitType.Critical)
                        {
                            Instance._battleUI.AddLog(_character.Name + " 對 " + targetName + " 造成了 " + logList[i].Text + " 爆擊傷害");
                        }
                        else if (logList[i].HitType == HitType.Hit)
                        {
                            Instance._battleUI.AddLog(_character.Name + " 對 " + targetName + " 造成了 " + logList[i].Text + " 傷害");
                        }
                        else
                        {
                            Instance._battleUI.AddLog(_character.Name + " 對 " + targetName + " 的攻擊沒有命中");
                        }
                    }
                    else if (logList[i].Type == EffectModel.TypeEnum.Poison)
                    {
                        Instance._battleUI.AddLog(_character.Name + " 使 " + targetName + " 中毒了");
                    }
                    else if (logList[i].Type == EffectModel.TypeEnum.Recover || logList[i].Type == EffectModel.TypeEnum.Medicine || logList[i].Type == EffectModel.TypeEnum.Purify)
                    {
                        Instance._battleUI.AddLog(_character.Name + " 使 " + targetName + " 回復了 " + logList[i].Text + " HP");
                    }
                    else if (logList[i].Type == EffectModel.TypeEnum.Sleep)
                    {
                        Instance._battleUI.AddLog(_character.Name + " 使 " + targetName + " 睡著了");
                    }
                    else
                    {
                        Instance._battleUI.AddLog(_character.Name + " 使 " + targetName + " " + logList[i].Text);
                    }   
                }
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

            private void SetUI(BattleCharacterInfo target, List<Log> logList)
            {
                Instance._battleUI.SetLittleHpBarValue(target.Index, target);
                Instance._battleUI.PlayFloatingNumberPool(target.Index, logList);
                if (logList.Count > _maxFloatingCount)
                {
                    _maxFloatingCount = logList.Count;
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