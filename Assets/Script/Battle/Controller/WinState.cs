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
                EnemyGroupModel enemyGroup = Instance._enemyGroup;
                EnemyModel enemy;
                List<int> itemList = new List<int>();
                for (int i=0; i<enemyGroup.EnemyList.Count; i++) 
                {
                    enemy = DataContext.Instance.EnemyDic[enemyGroup.EnemyList[i]];
                    itemId = enemy.DropList[Random.Range(0, enemy.DropList.Count)];
                    itemList.Add(itemId);
                    ItemManager.Instance.AddItem(itemId, 1);
                }
                Instance._battleUI.gameObject.SetActive(false);
                Instance._battleResultUI.gameObject.SetActive(true);
                Instance._battleResultUI.SetData(CharacterManager.Instance.CharacterInfoGroup.Lv, CharacterManager.Instance.CharacterInfoGroup.Exp, Instance._enemyGroup.Exp, itemList);
                CharacterManager.Instance.AddExp(Instance._enemyGroup.Exp);
                //SceneController.Instance.ChangeScene("Explore", () =>
                //{
                //    Explore.ExploreManager.Instance.Reload();
                //});
            }
        }
    }
}
