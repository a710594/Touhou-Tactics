using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    public partial class BattleController
    {
        private class CharacterState : BattleControllerState
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

                if (Instance.Info.IsTutorial && info.Faction == BattleCharacterInfo.FactionEnum.Player)
                {
                    BattleTutorialController.Instance.ToState_9(); //to State_9
                }

                int wt = info.CurrentWT;
                List<BattleCharacterInfo> characterList = Instance.CharacterList;
                for (int i = 0; i < characterList.Count; i++)
                {
                    characterList[i].CurrentWT -= wt;
                }

                Vector3 position = new Vector3();
                BattleCharacterController controller = Instance._controllerDic[info.Index];
                position = controller.transform.position;
                Instance._battleUI.ActionButtonGroup.gameObject.SetActive(false);
                _cameraMove.Move(position, ()=> 
                {
                    _context.SetState<ActionState>();
                });
                Instance.Arrow.transform.SetParent(controller.transform);
                Instance.Arrow.transform.localPosition = new Vector3(0, 1.3f, 0);
                Instance.Arrow.transform.localEulerAngles = Vector3.zero;
                Instance._battleUI.CharacterListGroupRefresh();
            }
        }
    }
}