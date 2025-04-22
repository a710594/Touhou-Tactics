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
                _selectedCharacter = Instance.SelectedCharacter;
                Instance.BattleUI.SetCommandVisible(false);
                Instance.CharacterInfoUIGroup.HideCharacterInfoUI_1();
                Instance.BattleUI.SetDirectionGroupVisible(!_selectedCharacter.Info.IsAuto);
                Instance.BattleUI.SetDirectionGroupPosition(_selectedCharacter.transform.position);

                List<Log> logList = _selectedCharacter.Info.CheckStatus();
                for(int i=0; i<logList.Count; i++)
                {
                    Instance.BattleUI.PlayFloatingNumberPool(_selectedCharacter, logList[i]);
                }
                for (int i = 0; i < _selectedCharacter.Info.SkillList.Count; i++)
                {
                    _selectedCharacter.Info.SkillList[i].CheckCD();
                }
                for (int i = 0; i < _selectedCharacter.Info.SubList.Count; i++)
                {
                    _selectedCharacter.Info.SubList[i].CheckCD();
                }

                if (!_selectedCharacter.Info.CanUseSpell && !_selectedCharacter.Info.HasSpell)
                {
                    _selectedCharacter.Info.CanUseSpell = true;
                }

                _selectedCharacter.Info.CurrentWT = _selectedCharacter.Info.WT;
                _selectedCharacter.LastPosition = BattleCharacterInfo.DefaultLastPosition;
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
                    _selectedCharacter.SetDirection(direction);
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

            public override void Update()
            {
                if (Input.GetMouseButtonDown(1))
                {
                    _context.SetState<CommandState>();
                    _selectedCharacter.Info.HasMove = false;
                    return;
                }
            }
        }
    }
}