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
            private CameraMove _cameraMove;

            public CharacterState(StateContext context) : base(context)
            {
                _cameraMove = Camera.main.transform.parent.gameObject.GetComponent<CameraMove>();
            }

            public override void Begin()
            {
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
                _cameraMove.Move(_controller.transform.position, ()=> 
                {
                    _context.SetState<CommandState>();
                });
                Instance.BattleUI.SetArrowTransform(_controller.transform);
                Instance.BattleUI.CharacterListGroupRefresh();
            }
        }
    }
}