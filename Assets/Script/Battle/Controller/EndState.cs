using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    public partial class BattleController
    {
        private class EndState : BattleControllerState
        {
            public EndState(StateContext context) : base(context)
            {
            }

            public override void Begin()
            {
                _characterList = Instance.CharacterList;
                _character = Instance.SelectedCharacter;
                Dictionary<Vector2Int, TileAttachInfo> tileDic = Instance.Info.TileAttachInfoDic;

                if (_characterList.Contains(_character))
                {
                    if (_character.ActionCount == 0)
                    {
                        List<FloatingNumberData> floatingList = new List<FloatingNumberData>();
                        _character.CheckStatus(floatingList);
                        for (int j = 0; j < floatingList.Count; j++)
                        {
                            Instance._battleUI.PlayFloatingNumberPool(_character.Index, floatingList[j].Type, floatingList[j].Text);
                            Instance._battleUI.SetLittleHpBarValue(_character.Index, _character);
                        }
                        for (int i = 0; i < _character.SkillList.Count; i++)
                        {
                            _character.SkillList[i].CheckCD();
                        }
                        for (int i = 0; i < _character.SupportList.Count; i++)
                        {
                            _character.SupportList[i].CheckCD();
                        }
                        _character.CurrentWT = _character.WT;
                        _character.ActionCount = 2;
                        _character.HasUseSkill = false;
                        _character.HasUseSupport = false;
                        _character.HasMove = false;
                        _character.LastPosition = BattleCharacterInfo.DefaultLastPosition;
                        _characterList.RemoveAt(0);
                        _characterList.Add(_character);
                        Instance.SortCharacterList(false);

                        if (_character.Faction == BattleCharacterInfo.FactionEnum.Player)
                        {
                            _context.SetState<DirectionState>();
                        }
                        else
                        {
                            _context.SetState<CharacterState>();
                        }
                    }
                    else
                    {
                        _context.SetState<ActionState>();
                    }
                }
                else
                {
                    _context.SetState<CharacterState>();
                }
            }
        }
    }
}
