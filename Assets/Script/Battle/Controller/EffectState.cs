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
            private Timer _timer = new Timer();
            private List<Log> _currentLogList = new List<Log>(); //當前回合的 log 總和

            public EffectState(StateContext context) : base(context)
            {
            }

            public override void Begin()
            {
                Instance.BattleUI.SetCharacterInfoUI_2(null);
                Instance.ClearQuad();
                _characterList = Instance.CharacterList;
                _character = Instance.SelectedCharacter;
                _currentLogList.Clear();

                int index = 0;
                foreach(KeyValuePair<Command, List<BattleCharacterInfo>> pair in Instance._commandTargetDic) 
                {
                    if (pair.Key.Effect.Type == EffectModel.TypeEnum.Summon)
                    {
                        Vector3 v3 = new Vector3(Instance._selectedPosition.x, Instance.Info.TileAttachInfoDic[Instance._selectedPosition].Height, Instance._selectedPosition.y);
                        UseEffect(pair.Key.Effect, v3);
                    }
                    else
                    {
                        for (int i = 0; i < pair.Value.Count; i++)
                        {
                            UseEffect(pair.Key, pair.Value[i]);
                        }
                    }

                    if (pair.Key is Skill)
                    {
                        Skill skill = (Skill)pair.Key;
                        _character.HasUseSkill = true;
                        if (skill.CD > 0)
                        {
                            skill.CurrentCD = skill.CD + 1;
                        }
                    }
                    else if(pair.Key is Support) 
                    {
                        Support support = (Support)pair.Key;
                        _character.HasUseSupport = true;
                        if (support.CD > 0)
                        {
                            support.CurrentCD = support.CD + 1;
                        }
                    }
                    else if(pair.Key is Spell) 
                    {
                        Spell spell = (Spell)pair.Key;
                        _character.HasUseSpell = true;
                        if (spell.CD > 0)
                        {
                            for (int i = 0; i < _characterList.Count; i++)
                            {
                                if (_characterList[i].Faction == BattleCharacterInfo.FactionEnum.Player)
                                {
                                    for (int j = 0; j < _characterList[i].SpellList.Count; j++)
                                    {
                                        _characterList[i].SpellList[j].CurrentCD = spell.CD + 1;
                                    }
                                }
                            }
                        }
                        ItemManager.Instance.MinusItem(ItemManager.CardID, 1);
                    }
                    else if(pair.Key is Consumables) 
                    {
                        Consumables consumables = (Consumables)pair.Key;
                        _character.HasUseItem = true;
                        ItemManager.Instance.MinusItem(consumables.ID, 1);
                    }
                    else if(pair.Key is Food) 
                    {
                        Food food = (Food)pair.Key;
                        _character.HasUseItem = true;
                        ItemManager.Instance.MinusItem(food.ID, 1);
                    }

                    if(!(pair.Key is Support) && index == 0) 
                    {
                        _character.ActionCount--;
                    }
                    index++;
                }

                RerangeLog();
                _timer.Start(_maxFloatingCount, CheckResult); 
            }

            public void UseEffect(Effect effect, Vector3 position) 
            {
                effect.Use(_character, position);
            }

            private void UseEffect(Command command, BattleCharacterInfo target) 
            {
                HitType hitType;

                if (command.AreaTarget == TargetEnum.Us || command.AreaTarget == TargetEnum.Self || command.AreaTarget == TargetEnum.UsMinHP || Instance.Tutorial != null)
                {
                    hitType = HitType.Hit;
                }
                else
                {
                    hitType = Instance.CheckHit(command.Hit, _character, target);
                }

                List<Log> logList = new List<Log>();
                command.Effect.Use(hitType, _character, target, logList);
                _currentLogList.AddRange(logList);
                Instance.LogList.AddRange(logList);
                //SetUI(target, logList);

                //for(int i=0; i<logList.Count; i++)
                //{
                //    Instance._battleUI.AddLog(logList[i].FullText);  
                //}
            }

            private void RerangeLog()
            {
                Dictionary<BattleCharacterInfo, List<Log>> rerangeDic = new Dictionary<BattleCharacterInfo, List<Log>>();
                for (int i=0; i< _currentLogList.Count; i++) 
                {
                    if (!rerangeDic.ContainsKey(_currentLogList[i].Target)) 
                    {
                        rerangeDic.Add(_currentLogList[i].Target, new List<Log>());
                    }
                    rerangeDic[_currentLogList[i].Target].Add(_currentLogList[i]);
                }

                _maxFloatingCount = 0;
                foreach(KeyValuePair<BattleCharacterInfo, List<Log>> pair in rerangeDic) 
                {
                    SetUI(pair.Key, pair.Value);
                }
            }

            private void SetUI(BattleCharacterInfo target, List<Log> logList)
            {
                Instance.BattleUI.SetLittleHpBarValue(target.Index, target);
                Instance.BattleUI.PlayFloatingNumberPool(target.Index, logList);
                if (logList.Count > _maxFloatingCount)
                {
                    _maxFloatingCount = logList.Count;
                }
                for(int i=0; i<logList.Count; i++)
                {
                    Instance.BattleUI.AddLog(logList[i].FullText);  
                }
            }

            private void CheckResult()
            {
                BattleCharacterInfo target;
                foreach(KeyValuePair<Command, List<BattleCharacterInfo>> pair in Instance._commandTargetDic)
                 {
                    for(int i=0; i<pair.Value.Count; i++)
                    {
                        target = pair.Value[i];
                        if (target.CurrentHP <= 0)
                        {
                            if (target.Faction == BattleCharacterInfo.FactionEnum.Player)
                            {
                                if (Instance.DyingList.Contains(target))
                                {
                                    Instance._controllerDic[target.Index].gameObject.SetActive(false);
                                    Instance.DyingList.Remove(target);
                                    Instance.DeadList.Add(target);
                                }
                                else
                                {
                                    _characterList.Remove(target);
                                    Instance.DyingList.Add(target);
                                    Instance._controllerDic[target.Index].SetGray(true);
                                }
                            }
                            else
                            {
                                _characterList.Remove(target);
                                Instance._controllerDic[target.Index].gameObject.SetActive(false);
                            }
                        }
                        else if (Instance.DyingList.Contains(target)) 
                        {
                            _characterList.Add(target);
                            Instance.DyingList.Remove(target);
                            Instance._controllerDic[target.Index].SetGray(false);
                        }
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
                        _context.SetState<CommandState>();
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