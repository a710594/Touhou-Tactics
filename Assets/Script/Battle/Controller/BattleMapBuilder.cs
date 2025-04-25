using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Battle
{
    public partial class BattleController //建造戰鬥場景的類別
    {
        //public Transform Tilemap;
        //public string[] SeedFile;

        //private string _prePath = Application.streamingAssetsPath;
        //private BattleFileReader _reader = new BattleFileReader();

        /*public BattleInfo Get(string map) //固定的地圖
        { 
            BattleFile file = DataContext.Instance.Load<BattleFile>(map, DataContext.PrePathEnum.MapBattle);
            BattleInfo info = new BattleInfo(file);

            CreateObject(info);

            return info;
        }

        public BattleInfo Generate(EnemyGroupModel data) //有隨機成分的地圖
        {
            BattleFile file = DataContext.Instance.Load<BattleFile>(data.GetMap(), DataContext.PrePathEnum.MapSeed);
            BattleInfo info = new BattleInfo(file);
            info.Lv = data.Lv;
            info.Exp = data.Exp;
            info.NeedCount = data.NeedCount;

            Vector2Int position;
            TileModel tileData;
            BattleInfoTile tileInfo;
            AttachModel attachData;
            Queue<Vector2Int> queue = new Queue<Vector2Int>();

            foreach (KeyValuePair<Vector2Int, BattleInfoTile> pair in info.TileDic)
            {
                if (pair.Value.TileData.Enqueue)
                {
                    queue.Enqueue(pair.Key);
                }
            }

            //BFS
            while (queue.Count != 0)
            {
                position = queue.Dequeue();
                if ((position + Vector2Int.up).y <= info.MaxY && !info.TileDic.ContainsKey(position + Vector2Int.up))
                {
                    tileData = GetAdjacentTile(info.TileDic[position].TileData, Vector2Int.up);
                    tileInfo = new BattleInfoTile();
                    tileInfo.TileData = tileData;
                    info.TileDic.Add(position + Vector2Int.up, tileInfo);
                    if (tileData.Enqueue)
                    {
                        queue.Enqueue(position + Vector2Int.up);
                    }
                }
                if ((position + Vector2Int.down).y >= info.MinY && !info.TileDic.ContainsKey(position + Vector2Int.down))
                {
                    tileData = GetAdjacentTile(info.TileDic[position].TileData, Vector2Int.down);
                    tileInfo = new BattleInfoTile();
                    tileInfo.TileData = tileData;
                    info.TileDic.Add(position + Vector2Int.down, tileInfo);
                    if (tileData.Enqueue)
                    {
                        queue.Enqueue(position + Vector2Int.down);
                    }
                }
                if ((position + Vector2Int.left).x >= info.MinX && !info.TileDic.ContainsKey(position + Vector2Int.left))
                {
                    tileData = GetAdjacentTile(info.TileDic[position].TileData, Vector2Int.left);
                    tileInfo = new BattleInfoTile();
                    tileInfo.TileData = tileData;
                    info.TileDic.Add(position + Vector2Int.left, tileInfo);
                    if (tileData.Enqueue)
                    {
                        queue.Enqueue(position + Vector2Int.left);
                    }
                }
                if ((position + Vector2Int.right).x <= info.MaxX && !info.TileDic.ContainsKey(position + Vector2Int.right))
                {
                    tileData = GetAdjacentTile(info.TileDic[position].TileData, Vector2Int.right);
                    tileInfo = new BattleInfoTile();
                    tileInfo.TileData = tileData;
                    info.TileDic.Add(position + Vector2Int.right, tileInfo);
                    if (tileData.Enqueue)
                    {
                        queue.Enqueue(position + Vector2Int.right);
                    }
                }
            }

            //Attach
            foreach (KeyValuePair<Vector2Int, BattleInfoTile> pair in info.TileDic)
            {
                if (!info.PlayerPositionList.Contains(pair.Key))
                {
                    attachData = GetAttachRandomly(pair.Value.TileData);
                    pair.Value.AttachData = attachData;
                }
            }

            //Life Game
            int count = 5;
            while (count > 0)
            {
                NextGeneration(info);
                count--;
            }

            //Create Enemy
            List<Vector2Int> invalidList = new List<Vector2Int>();
            foreach (KeyValuePair<Vector2Int, BattleInfoTile> pair in info.TileDic)
            {
                if (pair.Value.MoveCost == 0)
                {
                    invalidList.Add(pair.Key);
                }
            }

            for (int i = 0; i < info.PlayerPositionList.Count; i++)
            {
                invalidList.Add(info.PlayerPositionList[i]);
            }

            EnemyModel enemyData;
            for (int i = 0; i < data.EnemyList.Count; i++)
            {
                if (Utility.GetRandomPosition(info.MinX, info.MaxX, info.MinY, info.MaxY, invalidList, out Vector2Int result))
                {
                    enemyData = DataContext.Instance.EnemyDic[data.EnemyList[i]];
                    battleCharacterInfo = new BattleCharacterInfo(info.Lv, enemyData);
                    Type t = Type.GetType("Battle." + enemyData.AI);
                    battleCharacterInfo.AI = (BattleAI)Activator.CreateInstance(t);
                    battleCharacterInfo.AI.Init(battleCharacterInfo);
                    battleCharacterInfo.Position = Utility.ConvertToVector3Int(result, info.TileDic);
                    info.EnemyList.Add(battleCharacterInfo);
                    invalidList.Add(result);
                }
            }

            CreateObject(info);

            return info;
        }*/

        private void CreateObject()
        {
            for (int i = _root.childCount; i > 0; --i)
            {
                GameObject.DestroyImmediate(_root.GetChild(0).gameObject);
            }

            GameObject obj;
            BattleTileObject tileObj;
            GameObject attachObj;
            foreach (KeyValuePair<Vector2Int, BattleInfoTile> pair in TileDic)
            {
                obj = (GameObject)GameObject.Instantiate(Resources.Load("Tile/" + pair.Value.TileData.Name), Vector3.zero, Quaternion.identity);
                tileObj = obj.GetComponent<BattleTileObject>();
                tileObj.transform.SetParent(_root);
                tileObj.transform.position = new Vector3(pair.Key.x, 0, pair.Key.y);
                pair.Value.TileObject = tileObj;
                if(pair.Key == _cameraDefaultPosition) 
                {
                    _cameraController.SetMyGameObj(obj, null);
                }

                if (pair.Value.AttachData != null)
                {
                    attachObj = (GameObject)GameObject.Instantiate(Resources.Load("Attach/" + pair.Value.AttachData.Name), Vector3.zero, Quaternion.identity);
                    attachObj.transform.position = tileObj.transform.position + new Vector3(0, pair.Value.TileData.Height * 0.5f, 0);
                    attachObj.transform.parent = tileObj.transform;
                    pair.Value.AttachObject = attachObj;
                }
            }
        }

        private TileModel GetAdjacentTile(TileModel tile, Vector2Int direction)
        {
            int random;
            TileModel adjacentTile;
            List<int> pool = new List<int>();

            if (direction == Vector2Int.left)
            {
                pool = DataTable.Instance.TileDic[tile.ID].LeftPool;
            }
            else if (direction == Vector2Int.right)
            {
                pool = DataTable.Instance.TileDic[tile.ID].RightPool;
            }
            else if (direction == Vector2Int.up)
            {
                pool = DataTable.Instance.TileDic[tile.ID].UpPool;
            }
            else if (direction == Vector2Int.down)
            {
                pool = DataTable.Instance.TileDic[tile.ID].DownPool;
            }
            random = UnityEngine.Random.Range(0, pool.Count);
            adjacentTile = DataTable.Instance.TileDic[pool[random]];

            return adjacentTile;
        }

        private AttachModel GetAttachRandomly(TileModel tile)
        {
            int random = UnityEngine.Random.Range(0, tile.AttachPool.Count);
            int id = tile.AttachPool[random];
            AttachModel attach = null;
            if (id != 0)
            {
                attach = DataTable.Instance.AttachDic[id];
            }

            return attach;
        }

        // �p�� Life Game �U�@�N���ѽL
        private void NextGeneration(BattleFileRandom file)
        {
            AttachModel attach;
            foreach (KeyValuePair<Vector2Int, BattleInfoTile> pair in TileDic)
            {
                int neighbors = CountNeighbors(file, pair.Key);

                if (pair.Value.AttachData != null)
                {
                    if (neighbors < 2)
                    {
                        pair.Value.AttachData = null;
                    }
                    else if (neighbors >= 2 && neighbors <= 3)
                    {
                        continue;
                    }
                    else
                    {
                        pair.Value.AttachData = null;
                    }
                }
                else
                {
                    if (neighbors == 3 && !file.PlayerPositionList.Contains(pair.Key))
                    {
                        attach = GetAttachRandomly(pair.Value.TileData);
                        pair.Value.AttachData = attach;
                    }
                }
            }
        }

        private int CountNeighbors(BattleFileRandom file, Vector2Int v1)
        {
            int count = 0;
            Vector2Int v2;
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    int x = v1.x + i;
                    int y = v1.y + j;

                    if (x <= file.MinX || x >= file.MaxX || y <= file.MinY || y >= file.MaxY || (i == 0 && j == 0))
                    {
                        continue;
                    }

                    v2 = new Vector2Int(x, y);
                    if (TileDic[v2].AttachData != null)
                    {
                        count++;
                    }
                }
            }

            return count;
        }
    }
}