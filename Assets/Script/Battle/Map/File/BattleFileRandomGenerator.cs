using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    public class BattleFileRandomGenerator : MonoBehaviour
    {
        public string FileName;
        public Transform Tilemap;

        public class BattleFileFixedGenerator : MonoBehaviour
        {
            public string FileName;
            public int PlayerCount;
            public int Exp;
            public Transform Tilemap;
            public Transform EnemyGroup;

            public void BuildFile()
            {
                int minX = int.MaxValue;
                int maxX = int.MinValue;
                int minY = int.MaxValue;
                int maxY = int.MinValue;
                Vector3 position;
                BattleTileObject obj;
                List<BattleFileTile> tileList = new List<BattleFileTile>();
                BattleFileTile tile;
                List<Vector2Int> playerPositionList = new List<Vector2Int>(); //�T�ذ�,���|�����[����,��m���a���⪺�ϰ�
                foreach (Transform child in Tilemap)
                {
                    obj = child.GetComponent<BattleTileObject>();
                    position = child.position;
                    if (position.x < minX)
                    {
                        minX = Mathf.RoundToInt(position.x);
                    }
                    if (position.x > maxX)
                    {
                        maxX = Mathf.RoundToInt(position.x);
                    }
                    if (position.z < minY)
                    {
                        minY = Mathf.RoundToInt(position.z);
                    }
                    if (position.z > maxY)
                    {
                        maxY = Mathf.RoundToInt(position.z);
                    }
                    tile = new BattleFileTile();
                    tile.ID = obj.ID;
                    tile.Position = new Vector2Int(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.z));
                    tileList.Add(tile);
                    if (obj.tag == "PlayerPosition")
                    {
                        playerPositionList.Add(tile.Position);
                    }
                }

                BattleFileEnemyObject enemyObj;
                BattleFileEnemy enemyFile;
                List<BattleFileEnemy> enemyList = new List<BattleFileEnemy>(); //�ĤH����m�MID
                foreach (Transform child in EnemyGroup)
                {
                    enemyObj = child.GetComponent<BattleFileEnemyObject>();
                    enemyFile = new BattleFileEnemy();
                    enemyFile.ID = enemyObj.ID;
                    enemyFile.Lv = enemyObj.Lv;
                    enemyFile.Position = new Vector3Int(Mathf.RoundToInt(child.position.x), Mathf.RoundToInt(child.position.y), Mathf.RoundToInt(child.position.z));
                    enemyList.Add(enemyFile);
                }

                BattleFileRandom file = new BattleFileRandom();
                file.MinX = minX;
                file.MaxX = maxX;
                file.MinY = minY;
                file.MaxY = maxY;
                file.TileList = tileList;
                file.PlayerPositionList = playerPositionList;
                DataContext.Instance.Save(file, FileName, DataContext.PrePathEnum.MapBattle);
            }
        }
    }
}