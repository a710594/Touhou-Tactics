using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    public partial class BattleController
    {
        public class SpellState : BattleControllerState
        {
                        private Timer _timer = new Timer();

            public SpellState(StateContext context) : base(context)
            {
            }
            public override void Begin()
            {
                _character = Instance.SelectedCharacter;
                _characterList = Instance.CharacterList;

                _character.Info.HasMain = true;
                _character.Info.HasSpell = true;
                foreach (KeyValuePair<Command, List<BattleCharacterController>> pair in Instance._commandTargetDic)
                {
                    for (int i = 0; i < pair.Value.Count; i++)
                    {
                        Instance.UseEffect(pair.Key, _character, pair.Value[i]);
                    }

                    for (int i = 0; i < _characterList.Count; i++)
                    {
                        if (_characterList[i].Info.Faction == BattleCharacterInfo.FactionEnum.Player)
                        {
                            _characterList[i].Info.CanUseSpell = false;
                        }
                    }
                }

                _timer.Start(1, Instance.CheckResult);
            }
        }
    }
}