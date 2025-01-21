using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Rendering;

namespace Battle
{
    /*public class BattleFileLoader : MonoBehaviour
    {
        public string FileName;
        public BattleFileGenerator Generator;
        public Transform Tilemap;
        public Transform EnemyGroup;

        public void Load()
        {
            string path = Application.streamingAssetsPath + "/Map/Battle/" + FileName + ".txt";

            DataContext.Instance.Init();
            BattleFileReader reader = new BattleFileReader();
            BattleInfo info = reader.Read(path);

            for (int i = Tilemap.childCount; i > 0; --i)
            {
                DestroyImmediate(Tilemap.GetChild(0).gameObject);
            }

            for (int i = EnemyGroup.childCount; i > 0; --i)
            {
                DestroyImmediate(EnemyGroup.GetChild(0).gameObject);
            }

            GameObject obj;
            int count = 0;
            Transform[] noAttach = new Transform[info.PlayerPositionList.Count];
            foreach (KeyValuePair<Vector2Int, TileAttachInfo> pair in info.TileAttachInfoDic)
            {
                obj = (GameObject)GameObject.Instantiate(Resources.Load("Tile/" + pair.Value.TileID), Vector3.zero, Quaternion.identity);
                obj.transform.SetParent(Tilemap);
                obj.transform.position = new Vector3(pair.Key.x, 0, pair.Key.y);
                if (info.PlayerPositionList.Contains(pair.Key))
                {
                    noAttach[count] = obj.transform;
                    count++;
                }

                Generator.FileName = FileName;
                Generator.NeedCount = info.NeedCount;
                Generator.MustBeEqualToNeedCount = info.MustBeEqualToNeedCount;
                Generator.Exp = info.Exp;
                Generator.NoAttach = noAttach;
            }

            BattleFileEnemyObject battleMapEnemy;
            for(int i=0; i<info.EnemyList.Count; i++)
            {
                obj = (GameObject)GameObject.Instantiate(Resources.Load("Prefab/Other/BattleMapEnemy"), Vector3.zero, Quaternion.identity);
                battleMapEnemy = obj.GetComponent<BattleFileEnemyObject>();
                battleMapEnemy.Init(info.EnemyList[i].Enemy.ID);
                battleMapEnemy.transform.SetParent(EnemyGroup);
                battleMapEnemy.transform.position = info.EnemyList[i].Position;
            }
        }
    }*/
}