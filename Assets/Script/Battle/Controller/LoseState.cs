using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    public partial class BattleController
    {
        private class LoseState : BattleControllerState
        {
            public LoseState(StateContext context) : base(context)
            {
            }

            public override void Begin()
            {
                Instance._battleUI.gameObject.SetActive(false);
                Instance._battleResultUI.gameObject.SetActive(true);
                Instance._battleResultUI.SetLose(() =>
                {
                    SceneController.Instance.ChangeScene("Camp", () =>
                    {
                        CharacterManager.Instance.RecoverAllHP();
                        InputMamager.Instance.Unlock();
                    });
                });
            }
        }
    }
}
