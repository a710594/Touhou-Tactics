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
                _characterList = Instance.CharacterAliveList;
                _selectedCharacter = Instance.SelectedCharacter;

                if (_characterList.Contains(_selectedCharacter))
                {
                    _context.SetState<MoveState>();
                }
                else
                {
                    _context.SetState<CharacterState>();
                }
            }
        }
    }
}
