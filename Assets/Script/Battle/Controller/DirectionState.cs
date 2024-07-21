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
                Instance.ClearQuad();
                Instance.BattleUI.SetActionVisible(false);
                Instance.BattleUI.SetCharacterInfoUI_1(null);
                Instance.BattleUI.SetDirectionGroupVisible(true);
                Instance.BattleUI.SetDirectionGroupPosition(_character.Position);

                if(Instance.DirectionStateBeginHandler!=null)
                {
                    Instance.DirectionStateBeginHandler();
                }
            }

            /*public override void Click(Vector2Int position)
            {
                BattleCharacterController _controller = Instance._controllerDic[_character.Index];
                if (!_lock)
                {
                    if (Vector2Int.Distance(position, Utility.ConvertToVector2Int(_character.Position)) == 1)
                    {
                        _lock = true;
                        _controller.SetDirection(position - Utility.ConvertToVector2Int(_controller.transform.position));
                        _controller.SetSprite();
                        _timer.Start(0.5f, () =>
                        {
                            _lock = false;
                            _context.SetState<CharacterState>();
                        });
                    }
                }
            }*/

            public void SetDirection(Vector2Int direction)
            {
                if (!_lock)
                {
                    BattleCharacterController _controller = Instance._controllerDic[_character.Index];
                    _lock = true;
                    _controller.SetDirection(direction);
                    _controller.SetSprite();
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