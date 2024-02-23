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
                Instance.DirectionGroup.transform.position = _character.Position;
                Instance.DirectionGroup.SetActive(true);
            }

            public override void Click(Vector2Int position)
            {
                if (!_lock)
                {
                    _lock = true;
                    Instance._controllerDic[_character.Index].SetDirection(position);
                    _timer.Start(0.5f, ()=> 
                    {
                        _lock = false;
                        _context.SetState<CharacterState>();
                    });
                }
            }

            public override void End()
            {
                Instance.DirectionGroup.SetActive(false);
            }
        }
    }
}