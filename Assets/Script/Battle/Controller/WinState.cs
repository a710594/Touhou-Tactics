using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    public partial class BattleController
    {
        private class WinState : BattleControllerState 
        {
            public WinState(StateContext context) : base(context)
            {
            }

            public override void Begin(object obj) 
            {
                SceneController.Instance.ChangeScene("Explore", () =>
                {
                    Explore.ExploreManager.Instance.Reload();
                });
            }
        }
    }
}
