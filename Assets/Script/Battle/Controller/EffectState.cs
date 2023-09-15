using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    public partial class BattleController
    {
        private class EffectState : BattleControllerState
        {
            private List<BattleCharacterInfo> _targetList;
            private Timer _timer = new Timer();
            private Effect effect;

            public EffectState(StateContext context) : base(context)
            {
            }

            public override void Begin(object obj)
            {
                Instance._battleUI.SetCharacterInfoUI_2(null);
                Instance.ClearQuad();
                _characterList = Instance.CharacterList;
                _character = Instance._selectedCharacter;
                _targetList = new List<BattleCharacterInfo>();
                for (int i = 0; i < _characterList.Count; i++)
                {
                    if (CheckEffectArea(Instance._areaList, Utility.ConvertToVector2Int(_characterList[i].Position)))
                    {
                        _targetList.Add(_characterList[i]);
                    }

                }
                if (_targetList.Count > 0)
                {
                    int max = 0;
                    for (int i = 0; i < _targetList.Count; i++)
                    {
                        List<FloatingNumberData> floatingList = new List<FloatingNumberData>();
                        if (_character.SelectedSkill != null)
                        {
                            _character.SelectedSkill.SetEffect(_character, _targetList[i], floatingList, _characterList);
                        }
                        else if (_character.SelectedSupport != null) 
                        {
                            _character.SelectedSupport.SetEffect(_character, _targetList[i], floatingList, _characterList);
                        }
                        else if (_character.SelectedItem != null) 
                        {
                            _character.SelectedItem.SetEffect(_character, _targetList[i], floatingList, _characterList);
                        }
                        Instance._battleUI.SetLittleHpBarValue(_targetList[i].ID, _targetList[i]);
                        for (int j = 0; j < floatingList.Count; j++)
                        {
                            Instance._battleUI.PlayFloatingNumberPool(_targetList[i].ID, floatingList[j].Type, floatingList[j].Text);
                        }
                        if (floatingList.Count > max)
                        {
                            max = floatingList.Count;
                        }
                    }
                    _timer.Start(max, CheckResult);
                }
                else
                {
                    Instance._battleUI.TipLabel.SetLabel("≤@µL§œ¿≥...");
                    if (_character.SelectedSkill != null)
                    {
                        _character.HasUseSkill = true;
                        _character.ActionCount--;
                    }
                    else if (_character.SelectedSupport != null)
                    {
                        _character.HasUseSupport = true;
                    }
                    else if (_character.SelectedItem != null)
                    {
                        _character.HasUseItem = true;
                        _character.ActionCount--;
                    }
                    _context.SetState<EndState>();
                }
            }

            private void CheckResult()
            {
                for (int i = 0; i < _targetList.Count; i++)
                {
                    if (_targetList[i].CurrentHP <= 0)
                    {
                        _characterList.Remove(_targetList[i]);
                        Instance._controllerDic[_targetList[i].ID].gameObject.SetActive(false);
                        Instance.BattleInfo.TileInfoDic[Utility.ConvertToVector2Int(_targetList[i].Position)].HasCharacter = false;
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
                    Instance._battleUI.TipLabel.SetLabel("You Lose");
                    _context.SetState<WinState>();
                }
                else if (enemyCount == 0)
                {
                    Instance._battleUI.TipLabel.SetLabel("You Win");
                }
                else
                {
                    //if (_character.SelectedSkill != null)
                    //{
                    //    _character.HasUseSkill = true;
                    //    _character.ActionCount--;
                    //}
                    //else if (_character.SelectedSupport != null)
                    //{
                    //    _character.HasUseSupport = true;
                    //}
                    //else if (_character.SelectedItem != null)
                    //{
                    //    _character.HasUseItem = true;
                    //    _character.ActionCount--;
                    //    ItemManager.Instance.MinusItem(_character.SelectedItem, 1);
                    //}

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