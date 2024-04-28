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
            private CameraRotate _cameraRotate;

            public DirectionState(StateContext context) : base(context)
            {
                _cameraRotate = Camera.main.GetComponent<CameraRotate>();
            }

            public override void Begin()
            {
                _character = Instance.SelectedCharacter;
                Instance._battleUI.ActionButtonGroup.gameObject.SetActive(false);
                Instance._battleUI.SetCharacterInfoUI_1(null);
                Instance._battleUI.SetDirectionGroupVisible(true);
                Instance._battleUI.SetDirectionGroupPosition(_character.Position);

                if (Instance.Info.IsTutorial)
                {
                    BattleTutorialController.Instance.ToState_8();
                }
            }

            public override void Click(Vector2Int position)
            {
                BattleCharacterController _controller = Instance._controllerDic[_character.Index];
                if (!_lock)
                {
                    if (Vector2Int.Distance(position, Utility.ConvertToVector2Int(_character.Position)) == 1)
                    {
                        _lock = true;
                        _controller.SetDirection(_cameraRotate.CurrentState, position - Utility.ConvertToVector2Int(_controller.transform.position), Camera.main.transform.eulerAngles.y);
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