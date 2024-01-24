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
                int itemId;
                EnemyModel enemy;
                List<int> itemList = new List<int>();
                for (int i=0; i<Instance._enemyList.Count; i++) 
                {
                    enemy = DataContext.Instance.EnemyDic[Instance._enemyList[i]];
                    itemId = enemy.DropList[Random.Range(0, enemy.DropList.Count)];
                    itemList.Add(itemId);
                    ItemManager.Instance.AddItem(itemId, 1);
                }
                Instance._battleUI.gameObject.SetActive(false);
                Instance._battleResultUI.gameObject.SetActive(true);
                Instance._battleResultUI.SetData(CharacterManager.Instance.Info.Lv, CharacterManager.Instance.Info.Exp, Instance.Info.Exp, itemList);

                List<BattleCharacterInfo> playerList = new List<BattleCharacterInfo>();
                for (int i=0; i<Instance.CharacterList.Count; i++) 
                {
                    if(Instance.CharacterList[i].Faction == BattleCharacterInfo.FactionEnum.Player) 
                    {
                        playerList.Add(Instance.CharacterList[i]);
                    }
                }
                for (int i=0; i<Instance.DeadCharacterList.Count; i++) 
                {
                    playerList.Add(Instance.DeadCharacterList[i]);
                }
                CharacterManager.Instance.Refresh(playerList);
                CharacterManager.Instance.AddExp(Instance.Info.Exp);
            }
        }
    }
}
