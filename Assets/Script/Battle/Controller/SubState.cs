using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    public partial class BattleController
    {
        public class SubState : BattleControllerState
        {
            private Timer _timer = new Timer();

            public SubState(StateContext context) : base(context)
            {
            }

            public override void Begin()
            {
                _character = Instance.SelectedCharacter;
                _characterList = Instance.CharacterList;

                _character.Info.HasSub = true;
                foreach (KeyValuePair<Command, List<BattleCharacterController>> pair in Instance._commandTargetDic)
                {
                    for (int i = 0; i < pair.Value.Count; i++)
                    {
                        Instance.UseEffect(pair.Key, _character, pair.Value[i]);
                    }

                    Sub sub = (Sub)pair.Key;
                    if (sub.CD > 0)
                    {
                        sub.CurrentCD = sub.CD + 1;
                    }
                }

                _timer.Start(1, Instance.CheckResult);
            }
        }
    }
}
