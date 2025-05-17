using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    public partial class BattleController
    {
        public class CharacterState : BattleControllerState
        {
            public CharacterState(StateContext context) : base(context)
            {
            }

            public override void Begin()
            {
                _selectedCharacter = Instance.CharacterAliveList[0];
                Instance.SelectedCharacter = _selectedCharacter;
                
                if(Instance.CharacterStateBeginHandler != null)
                {
                    Instance.CharacterStateBeginHandler();
                }

                int wt = _selectedCharacter.Info.CurrentWT;
                List<BattleCharacterController> characterList = Instance.CharacterAliveList;
                for (int i = 0; i < characterList.Count; i++)
                {
                    characterList[i].Info.CurrentWT -= wt;
                }

                Instance.BattleUI.SetCommandVisible(false);
                _selectedCharacter.Info.HasMove = false;
                _selectedCharacter.Info.MoveAgain = false;
                _selectedCharacter.Info.HasSub = false;
                _selectedCharacter.Info.HasMain = false;
                _selectedCharacter.Info.HasSpell = false;
                _selectedCharacter.Info.HasItem = false;
                Instance.BattleUI.CharacterListGroupRefresh();
                Instance.BattleUI.ShowArrow(_selectedCharacter.transform);
                Instance.CharacterInfoUIGroup.ShowCharacterInfoUI_1(_selectedCharacter.Info, Utility.ConvertToVector2Int(_selectedCharacter.transform.position));
                Instance.CharacterInfoUIGroup.MoveCharacterInfoUI_1();
                Instance.CharacterInfoUIGroup.HideCharacterInfoUI_2();
                Instance._cameraController.SetMyGameObj(_selectedCharacter.gameObject, ()=> 
                {
                    if (_selectedCharacter.Info is BattlePlayerInfo)
                    {
                        EventManager.Instance.CheckCharacterStateEvent(_selectedCharacter);
                    }

                    if (_selectedCharacter.Info.IsAuto)
                    {
                        _selectedCharacter.AI.Begin();
                    }
                    else
                    {
                        _context.SetState<CommandState>();
                    }
                });
            }
        }
    }
}