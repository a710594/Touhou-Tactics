using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    public class BattleFileRandomGenerator : MonoBehaviour
    {
        public string FileName;
        public Transform Tilemap;
        public Transform CameraDefaultPosition;
        public Transform[] PlayerPosition;
        public FileLoader FileLoader;

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
            List<Vector2Int> playerPositionList = new List<Vector2Int>(); //禁建區,不會有附加物件,放置玩家角色的區域
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
                tile.Position = Utility.ConvertToVector2Int(position);
                tileList.Add(tile);
                //if (obj.tag == "PlayerPosition")
                //{
                //    playerPositionList.Add(tile.Position);
                //}
            }

            for (int i=0; i< PlayerPosition.Length; i++) 
            {
                playerPositionList.Add(Utility.ConvertToVector2Int(PlayerPosition[i].position));
            }

            BattleFileRandom file = new BattleFileRandom();
            file.MinX = minX;
            file.MaxX = maxX;
            file.MinY = minY;
            file.MaxY = maxY;
            file.TileList = tileList;
            file.PlayerPositionList = playerPositionList;
            file.CameraDefaultPosition = Utility.ConvertToVector2Int(CameraDefaultPosition.position);
            FileLoader.Save(file, FileName, FileLoader.PathEnum.MapBattleRandom);
        }
    }
}