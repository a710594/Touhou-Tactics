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
                    if (_character.Info.ActionCount == 0)
                    {
                        List<Log> logList = _character.Info.CheckStatus();
                        Instance.BattleUI.SetLittleHpBarValue(_character);
                        if(logList.Count > 0)
                        {
                            Instance.BattleUI.PlayFloatingNumberPool(_character, logList);
                        }
                        for (int i = 0; i < _character.Info.SkillList.Count; i++)
                        {
                            _character.Info.SkillList[i].CheckCD();
                        }
                        for (int i = 0; i < _character.Info.SupportList.Count; i++)
                        {
                            _character.Info.SupportList[i].CheckCD();
                        }

                        _character.Info.CurrentWT = _character.Info.WT;
                        _character.Info.ActionCount = 2;
                        _character.Info.HasUseSkill = false;
                        _character.Info.HasUseSupport = false;
                        _character.Info.HasMove = false;
                        _character.LastPosition = BattleCharacterControllerData.DefaultLastPosition;
                        _characterList.RemoveAt(0);
                        _characterList.Add(_character);
                        Instance.SortCharacterList(false);

                        if (_character.Info.Faction == BattleCharacterControllerData.FactionEnum.Player)
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
