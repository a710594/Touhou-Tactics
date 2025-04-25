using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    public class BattleFileFixedGenerator : MonoBehaviour
    {
        public string FileName;
        public int PlayerCount;
        public int Exp;
        public Transform Tilemap;
        public Transform EnemyGroup;
        public FileManager FileManager;

        public void BuildFile()
        {
            Vector3 position;
            BattleTileObject obj;
            List<BattleFileTile> tileList = new List<BattleFileTile>();
            BattleFileTile tile;
            List<Vector2Int> playerPositionList = new List<Vector2Int>(); //禁建區,不會有附加物件,放置玩家角色的區域
            foreach (Transform child in Tilemap)
            {
                obj = child.GetComponent<BattleTileObject>();
                position = child.position;
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
            List<BattleFileEnemy> enemyList = new List<BattleFileEnemy>(); //敵人的位置和ID
            foreach (Transform child in EnemyGroup)
            {
                enemyObj = child.GetComponent<BattleFileEnemyObject>();
                enemyFile = new BattleFileEnemy();
                enemyFile.ID = enemyObj.ID;
                enemyFile.Lv = enemyObj.Lv;
                enemyFile.Position = new Vector3Int(Mathf.RoundToInt(child.position.x), Mathf.RoundToInt(child.position.y), Mathf.RoundToInt(child.position.z));
                enemyList.Add(enemyFile);
            }

            BattleFileFixed file = new BattleFileFixed();
            file.PlayerCount = PlayerCount;
            file.Exp = Exp;
            file.TileList = tileList;
            file.PlayerPositionList = playerPositionList;
            file.EnemyList = enemyList;
            FileManager.Save(file, FileName, FileManager.PathEnum.MapBattleFixed);
        }
    }
}