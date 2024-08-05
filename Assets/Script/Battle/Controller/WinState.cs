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

            public override void Begin() 
            {
                int itemId;
                EnemyModel enemy;
                List<int> itemList = new List<int>();
                for (int i=0; i<Instance._enemyList.Count; i++) 
                {
                    enemy = DataContext.Instance.EnemyDic[Instance._enemyList[i]];
                    if (enemy.DropList.Count > 0)
                    {
                        itemId = enemy.DropList[Random.Range(0, enemy.DropList.Count)];
                        itemList.Add(itemId);
                        ItemManager.Instance.AddItem(itemId, 1);
                    }
                }
                Instance.BattleUI.gameObject.SetActive(false);
                Instance.BattleResultUI.gameObject.SetActive(true);
                Instance.BattleResultUI.SetWin(CharacterManager.Instance.Info.Lv, CharacterManager.Instance.Info.Exp, Instance.Info.Exp, itemList, ()=> 
                {
                    SceneController.Instance.ChangeScene("Explore", (sceneName) =>
                    {
                        Explore.ExploreManager.Instance.Reload();
                    });
                });

                List<BattleCharacterInfo> playerList = new List<BattleCharacterInfo>();
                for (int i=0; i<Instance.CharacterList.Count; i++) 
                {
                    if(Instance.CharacterList[i].Faction == BattleCharacterInfo.FactionEnum.Player) 
                    {
                        playerList.Add(Instance.CharacterList[i]);
                    }
                }
                for (int i = 0; i < Instance.DyingList.Count; i++)
                {
                    playerList.Add(Instance.DyingList[i]);
                }
                for (int i=0; i<Instance.DeadList.Count; i++) 
                {
                    playerList.Add(Instance.DeadList[i]);
                }
                CharacterManager.Instance.Refresh(playerList);
                CharacterManager.Instance.AddExp(Instance.Info.Exp);
            }
        }
    }
}
