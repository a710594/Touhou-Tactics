using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class RandomMapGenerator : MonoBehaviour
{
    public string[] SeedFile;

    private TileScriptableObject[] _tileScriptableObjects;
    private AttachScriptableObject[] _attachScriptableObjects;
    private string _prePath = Application.streamingAssetsPath;
    private Dictionary<string, TileScriptableObject> _tileScriptableObjectDic = new Dictionary<string, TileScriptableObject>();
    private Dictionary<string, AttachScriptableObject> _attachScriptableObjectDic = new Dictionary<string, AttachScriptableObject>();

    private void Awake()
    {
        _tileScriptableObjects = Resources.LoadAll("ScriptableObject", typeof(TileScriptableObject)).Cast<TileScriptableObject>().ToArray();

        for (int i=0; i< _tileScriptableObjects.Length; i++) 
        {
            _tileScriptableObjectDic.Add(_tileScriptableObjects[i].ID, _tileScriptableObjects[i]);
        }

        _attachScriptableObjects = Resources.LoadAll("ScriptableObject", typeof(AttachScriptableObject)).Cast<AttachScriptableObject>().ToArray();

        for (int i = 0; i < _attachScriptableObjects.Length; i++)
        {
            _attachScriptableObjectDic.Add(_attachScriptableObjects[i].ID, _attachScriptableObjects[i]);
        }
    }

    public void Generate(out BattleInfo battleInfo) 
    {
        string path = Path.Combine(_prePath, SeedFile[UnityEngine.Random.Range(0, SeedFile.Length)] + ".txt");
        string text = File.ReadAllText(path);
        string[] stringSeparators = new string[] { "\n", "\r\n" };
        string[] lines = text.Split(stringSeparators, StringSplitOptions.None);
        string[] str;
        Vector2Int position = new Vector2Int();
        TileScriptableObject tileScriptableObject;
        AttachScriptableObject attachScriptableObject;
        Dictionary<Vector2Int, TileScriptableObject> visitedDic = new Dictionary<Vector2Int, TileScriptableObject>();
        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        GameObject tileObj;
        GameObject attachObj;
        battleInfo = new BattleInfo();

        foreach(KeyValuePair<string, TileScriptableObject> pair1 in _tileScriptableObjectDic) 
        {
            Debug.Log(pair1.Key);
        }

        for (int i = this.transform.childCount; i > 0; --i)
        {
            DestroyImmediate(this.transform.GetChild(0).gameObject);
        }

        str = lines[0].Split(' ');
        battleInfo.Width = int.Parse(str[0]);
        battleInfo.Height = int.Parse(str[1]);

        for (int i = 1; i < lines.Length; i++) //第一行是長寬,忽視之
        {
            if (lines[i] != "")
            {
                if (i <= battleInfo.Width)
                {
                    str = lines[i].Split(' ');
                    for (int j = 0; j < str.Length; j++)
                    {
                        position = new Vector2Int(i - 1, j);
                        if (str[j] == "X")
                        {
                            visitedDic.Add(position, null);
                            battleInfo.TileInfoDic.Add(position, null);
                            continue;
                        }
                        else
                        {
                            tileScriptableObject = _tileScriptableObjectDic[str[j]];
                            visitedDic.Add(position, tileScriptableObject);
                            battleInfo.TileInfoDic.Add(position, new TileInfo(tileScriptableObject));
                            if (tileScriptableObject.Enqueue)
                            {
                                queue.Enqueue(position);
                            }
                        }
                    }
                }
                else
                {
                    battleInfo.NoAttachList = JsonConvert.DeserializeObject<List<Vector2Int>>(lines[i]);
                }

            }
        }

        //BFS
        while (queue.Count != 0)
        {
            position = queue.Dequeue();

            tileScriptableObject = visitedDic[position];
            if ((position + Vector2Int.up).y < battleInfo.Height && visitedDic[position + Vector2Int.up] == null)
            {
                visitedDic[position + Vector2Int.up] = GetAdjacentTile(tileScriptableObject, Vector2Int.up);
                battleInfo.TileInfoDic[position + Vector2Int.up] = new TileInfo(visitedDic[position + Vector2Int.up]);
                if (visitedDic[position + Vector2Int.up].Enqueue)
                {
                    queue.Enqueue(position + Vector2Int.up);
                }
            }
            if ((position + Vector2Int.down).y >= 0 && visitedDic[position + Vector2Int.down] == null)
            {
                visitedDic[position + Vector2Int.down] = GetAdjacentTile(tileScriptableObject, Vector2Int.down);
                battleInfo.TileInfoDic[position + Vector2Int.down] = new TileInfo(visitedDic[position + Vector2Int.down]);
                if (visitedDic[position + Vector2Int.down].Enqueue)
                {
                    queue.Enqueue(position + Vector2Int.down);
                }
            }
            if ((position + Vector2Int.left).x >= 0 && visitedDic[position + Vector2Int.left] == null)
            {
                visitedDic[position + Vector2Int.left] = GetAdjacentTile(tileScriptableObject, Vector2Int.left);
                battleInfo.TileInfoDic[position + Vector2Int.left] = new TileInfo(visitedDic[position + Vector2Int.left]);
                if (visitedDic[position + Vector2Int.left].Enqueue)
                {
                    queue.Enqueue(position + Vector2Int.left);
                }
            }
            if ((position + Vector2Int.right).x < battleInfo.Width && visitedDic[position + Vector2Int.right] == null)
            {
                visitedDic[position + Vector2Int.right] = GetAdjacentTile(tileScriptableObject, Vector2Int.right);
                battleInfo.TileInfoDic[position + Vector2Int.right] = new TileInfo(visitedDic[position + Vector2Int.right]);
                if (visitedDic[position + Vector2Int.right].Enqueue)
                {
                    queue.Enqueue(position + Vector2Int.right);
                }
            }
        }

        //Attach
        foreach (KeyValuePair<Vector2Int, TileScriptableObject> pair in visitedDic)
        {
            if (!battleInfo.NoAttachList.Contains(pair.Key))
            {
                attachScriptableObject = GetAttachIDRRandomly(pair.Value.Attachs); ;
                if (attachScriptableObject != null)
                {
                    battleInfo.TileInfoDic[pair.Key].SetAttach(attachScriptableObject.ID, attachScriptableObject.MoveCost);
                }
                else
                {
                    battleInfo.TileInfoDic[pair.Key].SetAttach(null, 0);
                }
            }
        }

        //使用 Life Game 去調整 Attach 的分布
        int count = 5;
        while (count > 0)
        {
            NextGeneration(battleInfo.Width, battleInfo.Height, battleInfo.TileInfoDic, battleInfo.NoAttachList);
            count--;
        }

        for (int i = this.transform.childCount; i > 0; --i)
        {
            DestroyImmediate(this.transform.GetChild(0).gameObject);
        }

        foreach(KeyValuePair<Vector2Int, TileInfo> pair in battleInfo.TileInfoDic) 
        {
            tileObj = (GameObject)GameObject.Instantiate(Resources.Load("Tile/" + pair.Value.TileID), Vector3.zero, Quaternion.identity);
            Transform parent = GameObject.Find("Tilemap").transform;
            if (parent != null)
            {
                tileObj.transform.SetParent(parent);
            }
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

    private TileScriptableObject GetAdjacentTile(TileScriptableObject tile, Vector2Int direction) 
    {
        int random;
        TileScriptableObject adjacentTile = null;
        List<string> pool = new List<string>();

        if(direction == Vector2Int.up) 
        {
            pool = GetTilePool(tile.Up);
            random = UnityEngine.Random.Range(0, pool.Count);
            adjacentTile = _tileScriptableObjectDic[pool[random]];
        }
        else if (direction == Vector2Int.down)
        {
            pool = GetTilePool(tile.Down);
            random = UnityEngine.Random.Range(0, pool.Count);
            adjacentTile = _tileScriptableObjectDic[pool[random]];
        }
        else if (direction == Vector2Int.left)
        {
            pool = GetTilePool(tile.Left);
            random = UnityEngine.Random.Range(0, pool.Count);
            adjacentTile = _tileScriptableObjectDic[pool[random]];
        }
        else if (direction == Vector2Int.right)
        {
            pool = GetTilePool(tile.Right);
            random = UnityEngine.Random.Range(0, pool.Count);
            adjacentTile = _tileScriptableObjectDic[pool[random]];
        }

        return adjacentTile;
    }

    private List<string> GetTilePool(TileScriptableObject.Contact[] contact) 
    {
        List<string> pool = new List<string>();
        for (int i=0; i<contact.Length; i++) 
        {
            for (int j=0; j<contact[i].Probability; j++) 
            {
                pool.Add(contact[i].ID);
            }
        }

        return pool;
    }

    private AttachScriptableObject GetAttachIDRRandomly(TileScriptableObject.Attach[] attachs) //隨機取得物件,包含 null
    {
        int random = UnityEngine.Random.Range(0, attachs.Length + 1);
        string id = null;
        if (random < attachs.Length)
        {
            if (attachs[random].ID != "")
            {
                id = attachs[random].ID;
            }
        }

        if (id != null)
        {
            return _attachScriptableObjectDic[id];
        }
        else
        {
            return null;
        }
    }

    private AttachScriptableObject GetAttachID(TileScriptableObject.Attach[] attachs) //根據機率來取得物件,不包含 null
    {
        List<string> pool = new List<string>();
        for (int i = 0; i < attachs.Length; i++)
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
            return _attachScriptableObjectDic[id];
        }
        else
        {
            return null;
        }
    }

    // 計算 Life Game 下一代的棋盤
    private Dictionary<Vector2Int, TileInfo> NextGeneration(int width, int height, Dictionary<Vector2Int, TileInfo> tileInfoDic, List<Vector2Int> noAttachList)
    {
        AttachScriptableObject attach;
        foreach(KeyValuePair<Vector2Int, TileInfo> pair in tileInfoDic) 
        {
            int neighbors = CountNeighbors(width, height, tileInfoDic, pair.Key, null);
            int grassNeighbors = CountNeighbors(width, height, tileInfoDic, pair.Key, "Grass");
            int treeNeighbors = CountNeighbors(width, height, tileInfoDic, pair.Key, "Tree");

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
                    attach = GetAttachID(_tileScriptableObjectDic[pair.Value.TileID].Attachs);
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

    private int CountNeighbors(int width, int height, Dictionary<Vector2Int, TileInfo> tileInfoDic, Vector2Int position, string attach)
    {
        int count = 0;

        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                int x = position.x + i;
                int y = position.y + j;

                if (x < 0 || x >= width || y < 0 || y >= height || (i == 0 && j == 0))
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
