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
                Instance.BattleUI.gameObject.SetActive(false);
                Instance.BattleResultUI.gameObject.SetActive(true);
                Instance.BattleResultUI.SetLose(() =>
                {
                    SceneController.Instance.ChangeScene("Camp", (sceneName) =>
                    {
                        CharacterManager.Instance.RecoverAllHP();
                        InputMamager.Instance.Unlock();
                    });
                });
            }
        }
    }
}
