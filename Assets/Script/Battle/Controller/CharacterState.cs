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
            private BattleCharacterInfo info;
            private CameraMove _cameraMove;

            public CharacterState(StateContext context) : base(context)
            {
                _cameraMove = Camera.main.transform.parent.gameObject.GetComponent<CameraMove>();
            }

            public override void Begin()
            {
                info = Instance.CharacterList[0];
                Instance.SelectedCharacter = info;
                
                if(Instance.CharacterStateBeginHandler != null)
                {
                    Instance.CharacterStateBeginHandler();
                }

                int wt = info.CurrentWT;
                List<BattleCharacterInfo> characterList = Instance.CharacterList;
                for (int i = 0; i < characterList.Count; i++)
                {
                    characterList[i].CurrentWT -= wt;
                }

                Instance.BattleUI.SetActionVisible(false);
                _cameraMove.Move(info.Position, ()=> 
                {
                    _context.SetState<CommandState>();
                });
                Instance.BattleUI.SetArrowTransform(info.Controller.transform);
                Instance.BattleUI.CharacterListGroupRefresh();
            }
        }
    }
}