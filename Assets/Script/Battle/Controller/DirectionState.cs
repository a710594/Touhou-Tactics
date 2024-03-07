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
                Instance._battleUI.ActionButtonGroup.gameObject.SetActive(false);
                //Instance.DirectionGroup.transform.position = _character.Position;
                //Instance.DirectionGroup.SetActive(true);
                Instance._battleUI.SetDirectionGroupVisible(true);
                Instance._battleUI.SetDirectionGroupPosition(_character.Position);

                if (Instance.Info.IsTutorial)
                {
                    BattleTutorialController.Instance.ToState_8();
                }
            }

            public override void Click(Vector2Int position)
            {
                if (!_lock)
                {
                    if (Vector2Int.Distance(position, Utility.ConvertToVector2Int(_character.Position)) == 1)
                    {
                        _lock = true;
                        Instance._controllerDic[_character.Index].SetDirection(position);
                        _timer.Start(0.5f, () =>
                        {
                            _lock = false;
                            _context.SetState<CharacterState>();
                        });
                    }
                }
            }

            public override void End()
            {
                //Instance.DirectionGroup.SetActive(false);
                Instance._battleUI.SetDirectionGroupVisible(false);
            }
        }
    }
}