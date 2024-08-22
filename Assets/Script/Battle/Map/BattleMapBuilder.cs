using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Battle
{
    public class BattleMapBuilder : MonoBehaviour //建造戰鬥場景的類別
    {
        public Transform Tilemap;
        public string[] SeedFile;

        private string _prePath = Application.streamingAssetsPath;
        private BattleFileReader _reader = new BattleFileReader();

        public BattleInfo Get(string map) //固定的地圖
        { 
            BattleFileFixed file = DataContext.Instance.Load<BattleFileFixed>(map, DataContext.PrePathEnum.MapBattle);
            BattleInfo info = new BattleInfo(file);

            CreateObject(info);

            return info;
        }

        //public BattleInfo Generate()
        //{
        //    return Generate(SeedFile[UnityEngine.Random.Range(0, SeedFile.Length)]);
        //}

        public BattleInfo Generate(string seed, List<int> enemyList, int lv, int exp) //有隨機成分的地圖
        {
            /*BattleFileFixed file = DataContext.Instance.Load<BattleFileFixed>(seed, DataContext.PrePathEnum.MapSeed);
            BattleInfo info = new BattleInfo(file);

            Vector2Int position;
            TileSetting tileSetting;
            TileSetting adjacentTileSetting;
            AttachSetting attachSetting;
            //Dictionary<Vector2Int, TileSetting> visitedDic = new Dictionary<Vector2Int, TileSetting>();
            Queue<Vector2Int> queue = new Queue<Vector2Int>();

            foreach (KeyValuePair<Vector2Int, BattleInfoTile> pair in info.TileDic)
            {
                tileSetting = DataContext.Instance.TileSettingDic[pair.Value.TileID];
                if (tileSetting.Enqueue)
                {
                    queue.Enqueue(pair.Key);
                }
            }

            //BFS
            while (queue.Count != 0)
            {
                position = queue.Dequeue();
                tileSetting = DataContext.Instance.TileSettingDic[info.TileAttachInfoDic[position].TileID];

                if ((position + Vector2Int.up).y <= info.MaxY && !info.TileAttachInfoDic.ContainsKey(position + Vector2Int.up))
                {
                    adjacentTileSetting = GetAdjacentTile(tileSetting, Vector2Int.up);
                    info.TileAttachInfoDic.Add(position + Vector2Int.up, new TileAttachInfo(adjacentTileSetting));
                    if (adjacentTileSetting.Enqueue)
                    {
                        queue.Enqueue(position + Vector2Int.up);
                    }
                }
                if ((position + Vector2Int.down).y >= info.MinY && !info.TileAttachInfoDic.ContainsKey(position + Vector2Int.down))
                {
                    adjacentTileSetting = GetAdjacentTile(tileSetting, Vector2Int.down);
                    info.TileAttachInfoDic.Add(position + Vector2Int.down, new TileAttachInfo(adjacentTileSetting));
                    if (adjacentTileSetting.Enqueue)
                    {
                        queue.Enqueue(position + Vector2Int.down);
                    }
                }
                if ((position + Vector2Int.left).x >= info.MinX && !info.TileAttachInfoDic.ContainsKey(position + Vector2Int.left))
                {
                    adjacentTileSetting = GetAdjacentTile(tileSetting, Vector2Int.left);
                    info.TileAttachInfoDic.Add(position + Vector2Int.left, new TileAttachInfo(adjacentTileSetting));
                    if (adjacentTileSetting.Enqueue)
                    {
                        queue.Enqueue(position + Vector2Int.left);
                    }
                }
                if ((position + Vector2Int.right).x <= info.MaxX && !info.TileAttachInfoDic.ContainsKey(position + Vector2Int.right))
                {
                    adjacentTileSetting = GetAdjacentTile(tileSetting, Vector2Int.right);
                    info.TileAttachInfoDic.Add(position + Vector2Int.right, new TileAttachInfo(adjacentTileSetting));
                    if (adjacentTileSetting.Enqueue)
                    {
                        queue.Enqueue(position + Vector2Int.right);
                    }
                }
            }

            //Attach
            foreach (KeyValuePair<Vector2Int, TileAttachInfo> pair in info.TileAttachInfoDic)
            {
                if (!info.PlayerPositionList.Contains(pair.Key))
                {
                    tileSetting = DataContext.Instance.TileSettingDic[pair.Value.TileID];
                    attachSetting = GetAttachIDRRandomly(tileSetting.Attachs); ;
                    if (attachSetting != null)
                    {
                        info.TileAttachInfoDic[pair.Key].SetAttach(attachSetting.ID, attachSetting.MoveCost);
                    }
                    else
                    {
                        info.TileAttachInfoDic[pair.Key].SetAttach(null, 0);
                    }
                }
            }

            //Life Game
            int count = 5;
            while (count > 0)
            {
                NextGeneration(info.MinX, info.MaxX, info.MinY, info.MaxY, info.TileAttachInfoDic, info.PlayerPositionList);
                count--;
            }

            //Create Enemy
            info.Exp = exp;
            List<Vector2Int> invalidList = new List<Vector2Int>();
            foreach (KeyValuePair<Vector2Int, TileAttachInfo> pair in info.TileAttachInfoDic)
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
            BattleCharacterInfo battleCharacterInfo;
            for (int i = 0; i < enemyList.Count; i++)
            {
                if (Utility.GetRandomPosition(info.MinX, info.MaxX, info.MinY, info.MaxY, invalidList, out Vector2Int result))
                {
                    enemyData = DataContext.Instance.EnemyDic[enemyList[i]];
                    battleCharacterInfo = new BattleCharacterInfo(info.Lv, enemyData);
                    Type t = Type.GetType("Battle." + enemyData.AI);
                    battleCharacterInfo.AI = (BattleAI)Activator.CreateInstance(t);
                    battleCharacterInfo.AI.Init(battleCharacterInfo);
                    battleCharacterInfo.Position = Utility.ConvertToVector3Int(result, info.TileAttachInfoDic);
                    info.EnemyList.Add(battleCharacterInfo);
                }
            }

            CreateObject(info);

            return info;*/

            return null;
        }

        private void CreateObject(BattleInfo info)
        {
            for (int i = Tilemap.childCount; i > 0; --i)
            {
                DestroyImmediate(Tilemap.GetChild(0).gameObject);
            }

            TileObject tileObj;
            GameObject attachObj;
            foreach (KeyValuePair<Vector2Int, BattleInfoTile> pair in info.TileDic)
            {
                tileObj = ((GameObject)GameObject.Instantiate(Resources.Load("Tile/" + pair.Value.TileData.Name), Vector3.zero, Quaternion.identity)).GetComponent<TileObject>();
                tileObj.transform.SetParent(Tilemap);
                tileObj.transform.position = new Vector3(pair.Key.x, 0, pair.Key.y);
                //info.TileComponentDic.Add(pair.Key, tileObj.GetComponent<TileComponent>());
                pair.Value.TileObject = tileObj;

                if (pair.Value.AttachName != null)
                {
                    attachObj = (GameObject)GameObject.Instantiate(Resources.Load("Attach/" + pair.Value.AttachName), Vector3.zero, Quaternion.identity);
                    attachObj.transform.position = tileObj.transform.position + new Vector3(0, pair.Value.TileData.Height - 0.5f, 0);
                    attachObj.transform.parent = tileObj.transform;
                    pair.Value.AttachObject = attachObj;
                }
            }

            GameObject enemyObj;
            for(int i=0; i<info.EnemyList.Count; i++)
            {
                enemyObj = (GameObject)GameObject.Instantiate(Resources.Load("Prefab/Character/" + info.EnemyList[i].Enemy.Controller), Vector3.zero, Quaternion.identity);
                enemyObj.transform.position = info.EnemyList[i].Position;
                enemyObj.transform.SetParent(transform);
                info.EnemyList[i].Controller = enemyObj.GetComponent<BattleCharacterController>();
                info.EnemyList[i].Controller.Init(info.EnemyList[i].Sprite);
            }
        }

        private TileModel GetAdjacentTile(TileModel tile, Vector2Int direction)
        {
            int random;
            TileModel adjacentTile;
            List<int> pool = new List<int>();

            if (direction == Vector2Int.left)
            {
                pool = DataContext.Instance.TileDic[tile.ID].LeftPool;
            }
            else if (direction == Vector2Int.right)
            {
                pool = DataContext.Instance.TileDic[tile.ID].RightPool;
            }
            else if (direction == Vector2Int.up)
            {
                pool = DataContext.Instance.TileDic[tile.ID].UpPool;
            }
            else if (direction == Vector2Int.down)
            {
                pool = DataContext.Instance.TileDic[tile.ID].DownPool;
            }
            random = UnityEngine.Random.Range(0, pool.Count);
            adjacentTile = DataContext.Instance.TileDic[pool[random]];

            return adjacentTile;
        }

        /*private AttachSetting GetAttachIDRRandomly(List<TileSetting.Attach> attachs) //�H�����o����,�]�t null
        {
            int random = UnityEngine.Random.Range(0, attachs.Count + 1);
            string id = null;
            if (random < attachs.Count)
            {
                if (attachs[random].ID != "")
                {
                    id = attachs[random].ID;
                }
            }

            if (id != null)
            {
                return DataContext.Instance.AttachSettingDic[id];
            }
            else
            {
                return null;
            }
        }*/

        /*private AttachSetting GetAttachID(List<TileSetting.Attach> attachs) //�ھھ��v�Ө��o����,���]�t null
        {
            List<string> pool = new List<string>();
            for (int i = 0; i < attachs.Count; i++)
            {
                for (int j = 0; j < attachs[i].Probability; j++)
                {
                    if (attachs[i].ID != "")
                    {
                        pool.Add(attachs[i].ID);
                    }
                    else
                    {
                        pool.Add(null);
                    }
                }
            }

            int random = UnityEngine.Random.Range(0, pool.Count);
            string id = null;
            if (pool.Count > 0)
            {
                id = pool[random];
            }

            if (id != null)
            {
                return DataContext.Instance.AttachSettingDic[id];
            }
            else
            {
                return null;
            }
        }*/

        // �p�� Life Game �U�@�N���ѽL
        /*private Dictionary<Vector2Int, TileAttachInfo> NextGeneration(int minX, int maxX, int minY, int maxY, Dictionary<Vector2Int, TileAttachInfo> tileInfoDic, List<Vector2Int> noAttachList)
        {
            AttachSetting attach;
            foreach (KeyValuePair<Vector2Int, TileAttachInfo> pair in tileInfoDic)
            {
                int neighbors = CountNeighbors(minX, maxX, minY, maxY, tileInfoDic, pair.Key, null);
                int grassNeighbors = CountNeighbors(minX, maxX, minY, maxY, tileInfoDic, pair.Key, "Grass");
                int treeNeighbors = CountNeighbors(minX, maxX, minY, maxY, tileInfoDic, pair.Key, "Tree");

                if (pair.Value.AttachID != null)
                {
                    if (neighbors < 2)
                    {
                        pair.Value.SetAttach(null, 0);
                    }
                    else if (neighbors >= 2 && neighbors <= 3)
                    {
                        continue;
                    }
                    else
                    {
                        pair.Value.SetAttach(null, 0);
                    }
                }
                else
                {
                    if (neighbors == 3 && !noAttachList.Contains(pair.Key))
                    {
                        attach = GetAttachID(DataContext.Instance.TileSettingDic[pair.Value.TileID].Attachs);
                        if (attach != null)
                        {
                            pair.Value.SetAttach(attach.ID, attach.MoveCost);
                        }
                        else
                        {
                            pair.Value.SetAttach(null, 0);
                        }
                    }
                }
            }

            return tileInfoDic;
        }*/

        /*private int CountNeighbors(int minX, int maxX, int minY, int maxY, Dictionary<Vector2Int, TileAttachInfo> tileInfoDic, Vector2Int position, string attach)
        {
            int count = 0;

            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    int x = position.x + i;
                    int y = position.y + j;

                    if (x <= minX || x >= maxX || y <= minY || y >= maxY || (i == 0 && j == 0))
                    {
                        continue;
                    }

                    if ((attach == null && tileInfoDic[new Vector2Int(x, y)].AttachID != null) || (attach != null && tileInfoDic[new Vector2Int(x, y)].AttachID == attach))
                    {
                        count++;
                    }
                }
            }

            return count;
        }*/
    }
}