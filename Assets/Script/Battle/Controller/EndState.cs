using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    public partial class BattleController
    {
        public class EndState : BattleControllerState
        {
            public EndState(StateContext context) : base(context)
            {
            }

            public override void Begin()
            {
                _characterList = Instance.CharacterList;
                _character = Instance.SelectedCharacter;

                if (_characterList.Contains(_character))
                {
                    if (_character.ActionCount == 0)
                    {
                        List<Log> logList = _character.CheckStatus();
                        Instance.BattleUI.SetLittleHpBarValue(_character);
                        if(logList.Count > 0)
                        {
                            Instance.BattleUI.PlayFloatingNumberPool(_character, logList);
                        }
                        for (int i = 0; i < _character.SkillList.Count; i++)
                        {
                            _character.SkillList[i].CheckCD();
                        }
                        for (int i = 0; i < _character.SupportList.Count; i++)
                        {
                            _character.SupportList[i].CheckCD();
                        }
                        if (_character.CurrentPP < BattleCharacterInfo.MaxPP)
                        {
                            _character.CurrentPP++;
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
                        _context.SetState<CommandState>();
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
