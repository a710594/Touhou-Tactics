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
                if(Instance.SelectedCharacter != null) 
                {
                    Instance.SelectedCharacter.Outline.OutlineWidth = 1;
                    if (Instance.SelectedCharacter.Info.Faction == BattleCharacterInfo.FactionEnum.Player) 
                    {
                        Instance.SelectedCharacter.Outline.OutlineColor = Color.blue;
                    }
                    else
                    {
                        Instance.SelectedCharacter.Outline.OutlineColor = Color.red;
                    }
                }

                _character = Instance.CharacterList[0];
                Instance.SelectedCharacter = _character;
                
                if(Instance.CharacterStateBeginHandler != null)
                {
                    Instance.CharacterStateBeginHandler();
                }

                int wt = _character.Info.CurrentWT;
                List<BattleCharacterController> characterList = Instance.CharacterList;
                for (int i = 0; i < characterList.Count; i++)
                {
                    characterList[i].Info.CurrentWT -= wt;
                }

                Instance.BattleUI.SetCommandVisible(false);
                _character.Outline.OutlineWidth = 2;
                _character.Outline.OutlineColor = Color.yellow;
                _character.Info.HasMove = false;
                _character.Info.MoveAgain = false;
                _character.Info.HasSub = false;
                _character.Info.HasMain = false;
                _character.Info.HasSpell = false;
                Instance.BattleUI.CharacterListGroupRefresh();
                Instance._cameraController.SetMyGameObj(_character.gameObject, ()=> 
                {
                    _context.SetState<CommandState>();
                });
            }
        }
    }
}