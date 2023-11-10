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

            public CharacterState(StateContext context) : base(context)
            {
            }

            public override void Begin(object obj)
            {
                info = Instance.CharacterList[0];
                Instance.SelectedCharacter = info;

                int wt = info.CurrentWT;
                List<BattleCharacterInfo> characterList = Instance.CharacterList;
                for (int i = 0; i < characterList.Count; i++)
                {
                    characterList[i].CurrentWT -= wt;
                }

                Vector3 position = new Vector3();
                BattleCharacterController controller = Instance._controllerDic[info.Index];
                if (Instance._cameraRotate.CurrentState == CameraRotate.StateEnum.Slope)
                {
                    position = controller.transform.position + new Vector3(-10, 10, -10);
                }
                else
                {
                    position = controller.transform.position + new Vector3(0, 10, 0);
                }
                Instance._battleUI.ActionButtonGroup.gameObject.SetActive(false);
                float distance = Vector3.Distance(Camera.main.transform.position, position);
                Camera.main.transform.DOMove(position, 0.1f * distance).OnComplete(() =>
                {
                    _context.SetState<ActionState>();
                });
                Instance._arrow.transform.SetParent(controller.transform);
                Instance._arrow.transform.localPosition = new Vector3(0, 1.3f, 0);
                Instance._arrow.transform.localEulerAngles = Vector3.zero;
                Instance._battleUI.CharacterListGroupRefresh();
            }
        }
    }
}