using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    public class BattleFileRandomGenerator : MonoBehaviour
    {
        public string FileName;
        public int StdDev; //標準差
        public Vector2Int MinPosition;
        public Vector2Int MaxPosition;
        public Vector2Int CameraDefaultPosition;
        public Vector2Int EnemyCenterPosition;
        public Vector2Int[] PlayerPosition;
        public Transform Root;
        public FileManager FileManager;

        public void BuildFile()
        {
            Vector3 position;
            BattleTileObject obj;
            List<BattleFileTile> tileList = new List<BattleFileTile>();
            BattleFileTile tile;
            List<Vector2Int> playerPositionList = new List<Vector2Int>(); //禁建區,不會有附加物件,放置玩家角色的區域
            foreach (Transform child in Root)
            {
                obj = child.GetComponent<BattleTileObject>();
                position = child.position;
                tile = new BattleFileTile();
                tile.ID = obj.ID;
                tile.Position = Utility.ConvertToVector2Int(position);
                tileList.Add(tile);
            }

            for (int i=0; i< PlayerPosition.Length; i++) 
            {
                playerPositionList.Add(PlayerPosition[i]);
            }

            BattleFileRandom file = new BattleFileRandom();
            file.StdDev = StdDev;
            file.MinX = MinPosition.x;
            file.MaxX = MaxPosition.x;
            file.MinY = MinPosition.y;
            file.MaxY = MaxPosition.y;
            file.TileList = tileList;
            file.PlayerPositionList = playerPositionList;
            file.CameraDefaultPosition = CameraDefaultPosition;
            file.EnemyCenterPosition = EnemyCenterPosition;
            FileManager.Save(file, FileName, FileManager.PathEnum.MapBattleRandom);
        }
    }
}