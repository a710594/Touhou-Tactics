using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    public partial class BattleController
    {
        public class DirectionState : BattleControllerState
        {
            private bool _lock = false;
            private Timer _timer = new Timer();

            public DirectionState(StateContext context) : base(context)
            {
            }

            public override void Begin()
            {
                _character = Instance.SelectedCharacter;
                Instance.ClearQuad();
                Instance.BattleUI.SetCommandVisible(false);
                Instance.CharacterInfoUIGroup.SetCharacterInfoUI_1(null);
                Instance.BattleUI.SetDirectionGroupVisible(!_character.Info.IsAuto);
                Instance.BattleUI.SetDirectionGroupPosition(_character.transform.position);

                List<Log> logList = _character.Info.CheckStatus();
                Instance.BattleUI.SetLittleHpBarValue(_character);
                if (logList.Count > 0)
                {
                    Instance.BattleUI.PlayFloatingNumberPool(_character, logList);
                }
                for (int i = 0; i < _character.Info.SkillList.Count; i++)
                {
                    _character.Info.SkillList[i].CheckCD();
                }
                for (int i = 0; i < _character.Info.SubList.Count; i++)
                {
                    _character.Info.SubList[i].CheckCD();
                }

                if (!_character.Info.CanUseSpell && !_character.Info.HasSpell)
                {
                    _character.Info.CanUseSpell = true;
                }

                _character.Info.CurrentWT = _character.Info.WT;
                _character.LastPosition = BattleCharacterInfo.DefaultLastPosition;
                Instance.SortCharacterList(false);

                if (Instance.DirectionStateBeginHandler!=null)
                {
                    Instance.DirectionStateBeginHandler();
                }
            }

            public void SetDirection(Vector2Int direction)
            {
                if (!_lock)
                {
                    _lock = true;
                    _character.SetDirection(direction);
                    _timer.Start(0.5f, () =>
                    {
                        _lock = false;
                        _context.SetState<CharacterState>();
                    });
                }
            }

            public override void End()
            {
                //Instance.DirectionGroup.SetActive(false);
                Instance.BattleUI.SetDirectionGroupVisible(false);
            }
        }
    }
}