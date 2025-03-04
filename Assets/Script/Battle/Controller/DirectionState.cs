using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    public partial class BattleController
    {
        private class DirectionState : BattleControllerState
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
                Instance.BattleUI.SetActionVisible(false);
                Instance.CharacterInfoUIGroup.SetCharacterInfoUI_1(null);
                Instance.BattleUI.SetDirectionGroupVisible(true);
                Instance.BattleUI.SetDirectionGroupPosition(_character.transform.position);

                if(Instance.DirectionStateBeginHandler!=null)
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