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
            private BattleCharacterController _controller;

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

                _controller = Instance.CharacterList[0];
                Instance.SelectedCharacter = _controller;
                
                if(Instance.CharacterStateBeginHandler != null)
                {
                    Instance.CharacterStateBeginHandler();
                }

                int wt = _controller.Info.CurrentWT;
                List<BattleCharacterController> characterList = Instance.CharacterList;
                for (int i = 0; i < characterList.Count; i++)
                {
                    characterList[i].Info.CurrentWT -= wt;
                }

                Instance.BattleUI.SetActionVisible(false);
                _controller.Outline.OutlineWidth = 2;
                _controller.Outline.OutlineColor = Color.yellow;
                Instance.BattleUI.CharacterListGroupRefresh();
                Instance._cameraController.SetMyGameObj(_controller.gameObject, ()=> 
                {
                    _context.SetState<CommandState>();
                });
            }
        }
    }
}