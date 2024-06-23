using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class BattleMapBuilder : MonoBehaviour //�سy�԰����������O
{
    public Transform Tilemap;
    public string[] SeedFile;

    private string _prePath = Application.streamingAssetsPath;
    private BattleFileReader _reader = new BattleFileReader();
    private BattleInfo _info;

    public BattleInfo Generate() //���H���������a��
    {
        string path = Path.Combine(_prePath, "MapSeed/" + SeedFile[UnityEngine.Random.Range(0, SeedFile.Length)] + ".txt");
        _info = _reader.Read(path);

        for (int i = Tilemap.childCount; i > 0; --i)
        {
            DestroyImmediate(Tilemap.GetChild(0).gameObject);
        }

        //str = lines[0].Split(' ');
        //battleInfo.Width = int.Parse(str[0]);
        //battleInfo.Height = int.Parse(str[1]);


        Vector2Int position = new Vector2Int();
        TileSetting tileSetting;
        TileSetting adjacentTileSetting;
        AttachSetting attachSetting;
        //Dictionary<Vector2Int, TileSetting> visitedDic = new Dictionary<Vector2Int, TileSetting>();
        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        GameObject tileObj;
        GameObject attachObj;
        //battleInfo = new BattleInfo();

        foreach (KeyValuePair<Vector2Int, TileAttachInfo> pair in _info.TileAttachInfoDic)
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
            tileSetting = DataContext.Instance.TileSettingDic[_info.TileAttachInfoDic[position].TileID];

            if ((position + Vector2Int.up).y <= _info.MaxY && !_info.TileAttachInfoDic.ContainsKey(position + Vector2Int.up))
            {
                adjacentTileSetting = GetAdjacentTile(tileSetting, Vector2Int.up);
                _info.TileAttachInfoDic.Add(position + Vector2Int.up, new TileAttachInfo(adjacentTileSetting));
                if (adjacentTileSetting.Enqueue)
                {
                    queue.Enqueue(position + Vector2Int.up);
                }
            }
            if ((position + Vector2Int.down).y >= _info.MinY && !_info.TileAttachInfoDic.ContainsKey(position + Vector2Int.down))
            {
                adjacentTileSetting = GetAdjacentTile(tileSetting, Vector2Int.down);
                _info.TileAttachInfoDic.Add(position + Vector2Int.down, new TileAttachInfo(adjacentTileSetting));
                if (adjacentTileSetting.Enqueue)
                {
                    queue.Enqueue(position + Vector2Int.down);
                }
            }
            if ((position + Vector2Int.left).x >= _info.MinX && !_info.TileAttachInfoDic.ContainsKey(position + Vector2Int.left))
            {
                adjacentTileSetting = GetAdjacentTile(tileSetting, Vector2Int.left);
                _info.TileAttachInfoDic.Add(position + Vector2Int.left, new TileAttachInfo(adjacentTileSetting));
                if (adjacentTileSetting.Enqueue)
                {
                    queue.Enqueue(position + Vector2Int.left);
                }
            }
            if ((position + Vector2Int.right).x <= _info.MaxX && !_info.TileAttachInfoDic.ContainsKey(position + Vector2Int.right))
            {
                adjacentTileSetting = GetAdjacentTile(tileSetting, Vector2Int.right);
                _info.TileAttachInfoDic.Add(position + Vector2Int.right, new TileAttachInfo(adjacentTileSetting));
                if (adjacentTileSetting.Enqueue)
                {
                    queue.Enqueue(position + Vector2Int.right);
                }
            }
        }

        //Attach
        foreach (KeyValuePair<Vector2Int, TileAttachInfo> pair in _info.TileAttachInfoDic)
        {
            if (!_info.NoAttachList.Contains(pair.Key))
            {
                tileSetting = DataContext.Instance.TileSettingDic[pair.Value.TileID];
                attachSetting = GetAttachIDRRandomly(tileSetting.Attachs); ;
                if (attachSetting != null)
                {
                    _info.TileAttachInfoDic[pair.Key].SetAttach(attachSetting.ID, attachSetting.MoveCost);
                }
                else
                {
                    _info.TileAttachInfoDic[pair.Key].SetAttach(null, 0);
                }
            }
        }

        //�ϥ� Life Game �h�վ� Attach ������
        int count = 5;
        while (count > 0)
        {
            NextGeneration(_info.MinX, _info.MaxX, _info.MinY, _info.MaxY, _info.TileAttachInfoDic, _info.NoAttachList);
            count--;
        }

        //for (int i = this.transform.childCount; i > 0; --i)
        //{
        //    DestroyImmediate(this.transform.GetChild(0).gameObject);
        //}

        foreach (KeyValuePair<Vector2Int, TileAttachInfo> pair in _info.TileAttachInfoDic) 
        {
            tileObj = (GameObject)GameObject.Instantiate(Resources.Load("Tile/" + pair.Value.TileID), Vector3.zero, Quaternion.identity);
            tileObj.transform.SetParent(Tilemap);
            tileObj.transform.position = new Vector3(pair.Key.x, 0, pair.Key.y);
            _info.TileComponentDic.Add(pair.Key, tileObj.GetComponent<TileComponent>());

            if (pair.Value.AttachID != null)
            {
                attachObj = (GameObject)GameObject.Instantiate(Resources.Load("Attach/" + pair.Value.AttachID), Vector3.zero, Quaternion.identity);
                attachObj.transform.position = tileObj.transform.position + new Vector3(0, pair.Value.Height - 0.5f, 0);
                attachObj.transform.parent = tileObj.transform;
                _info.AttachDic.Add(pair.Key, attachObj);
            }
        }

        return _info;
    }

    public BattleInfo Get(string map) //�T�w���a��
    {
        string path = Path.Combine(_prePath, "Map/Battle/" + map + ".txt");
        _info = _reader.Read(path);

        for (int i = Tilemap.childCount; i > 0; --i)
        {
            DestroyImmediate(Tilemap.GetChild(0).gameObject);
        }

        CreateObject(_info);

        return _info;
    }

    public void CreateObject(BattleInfo battleInfo)
    {
        GameObject tileObj;
        GameObject attachObj;
        foreach (KeyValuePair<Vector2Int, TileAttachInfo> pair in battleInfo.TileAttachInfoDic)
        {
            tileObj = (GameObject)GameObject.Instantiate(Resources.Load("Tile/" + pair.Value.TileID), Vector3.zero, Quaternion.identity);
            tileObj.transform.SetParent(Tilemap);
            tileObj.transform.position = new Vector3(pair.Key.x, 0, pair.Key.y);
            battleInfo.TileComponentDic.Add(pair.Key, tileObj.GetComponent<TileComponent>());

            if (pair.Value.AttachID != null)
            {
                attachObj = (GameObject)GameObject.Instantiate(Resources.Load("Attach/" + pair.Value.AttachID), Vector3.zero, Quaternion.identity);
                attachObj.transform.position = tileObj.transform.position + new Vector3(0, pair.Value.Height - 0.5f, 0);
                attachObj.transform.parent = tileObj.transform;
                battleInfo.AttachDic.Add(pair.Key, attachObj);
            }
        }
    }

    private TileSetting GetAdjacentTile(TileSetting tile, Vector2Int direction) 
    {
        int random;
        TileSetting adjacentTile = null;
        List<string> pool = new List<string>();

        if(direction == Vector2Int.up) 
        {
            pool = GetTilePool(tile.Up);
            random = UnityEngine.Random.Range(0, pool.Count);
            adjacentTile = DataContext.Instance.TileSettingDic[pool[random]];
        }
        else if (direction == Vector2Int.down)
        {
            pool = GetTilePool(tile.Down);
            random = UnityEngine.Random.Range(0, pool.Count);
            adjacentTile = DataContext.Instance.TileSettingDic[pool[random]];
        }
        else if (direction == Vector2Int.left)
        {
            pool = GetTilePool(tile.Left);
            random = UnityEngine.Random.Range(0, pool.Count);
            adjacentTile = DataContext.Instance.TileSettingDic[pool[random]];
        }
        else if (direction == Vector2Int.right)
        {
            pool = GetTilePool(tile.Right);
            random = UnityEngine.Random.Range(0, pool.Count);
            adjacentTile = DataContext.Instance.TileSettingDic[pool[random]];
        }

        return adjacentTile;
    }

    private List<string> GetTilePool(List<TileSetting.Contact> contact) 
    {
        List<string> pool = new List<string>();
        for (int i=0; i<contact.Count; i++) 
        {
            for (int j=0; j<contact[i].Probability; j++) 
            {
                pool.Add(contact[i].ID);
            }
        }

        return pool;
    }

    private AttachSetting GetAttachIDRRandomly(List<TileSetting.Attach> attachs) //�H�����o����,�]�t null
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
    }

    private AttachSetting GetAttachID(List<TileSetting.Attach> attachs) //�ھھ��v�Ө��o����,���]�t null
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
    }

    // �p�� Life Game �U�@�N���ѽL
    private Dictionary<Vector2Int, TileAttachInfo> NextGeneration(int minX, int maxX, int minY, int maxY, Dictionary<Vector2Int, TileAttachInfo> tileInfoDic, List<Vector2Int> noAttachList)
    {
        AttachSetting attach;
        foreach(KeyValuePair<Vector2Int, TileAttachInfo> pair in tileInfoDic) 
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
    }

    private int CountNeighbors(int minX, int maxX, int minY, int maxY, Dictionary<Vector2Int, TileAttachInfo> tileInfoDic, Vector2Int position, string attach)
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

                if ((attach == null && tileInfoDic[new Vector2Int(x, y)].AttachID != null)  || (attach != null && tileInfoDic[new Vector2Int(x, y)].AttachID == attach))
                {
                    count++;
                }
            }
        }

        return count;
    }
}
