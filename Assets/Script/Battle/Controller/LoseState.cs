using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    public partial class BattleController
    {
        public class LoseState : BattleControllerState
        {
            public LoseState(StateContext context) : base(context)
            {
            }

            public override void Begin()
            {
                Instance.DeInit();

                Instance.BattleResultUI.SetLose(() =>
                {
                    SceneController.Instance.ChangeScene("Camp", ChangeSceneUI.TypeEnum.Fade, (sceneName) =>
                    {
                        CharacterManager.Instance.RecoverAllHP();
                        ItemManager.Instance.Info.Key = 0;
                        InputMamager.Instance.Unlock();
                    });
                });
            }
        }
    }
}
