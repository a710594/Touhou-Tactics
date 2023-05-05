using Newtonsoft.Json;
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

    public void Generate(out Dictionary<Vector3, TileComponent> tileComponentDic, out Dictionary<Vector3, TileInfo> tileInfoDic, out Dictionary<Vector3, GameObject> attachDic, out List<Vector3> noAttachList) 
    {
        int width;
        int height;
        string path = Path.Combine(_prePath, SeedFile[Random.Range(0, SeedFile.Length)] + ".txt");
        string text = File.ReadAllText(path);
        string[] lines = text.Split('\n');
        string[] str;
        Vector3 position = new Vector3();
        TileScriptableObject tileScriptableObject;
        AttachScriptableObject attachScriptableObject;
        Dictionary<Vector3, TileScriptableObject> visitedDic = new Dictionary<Vector3, TileScriptableObject>();
        Queue<Vector3> queue = new Queue<Vector3>();
        GameObject tileObj;
        GameObject attachObj;

        tileInfoDic = new Dictionary<Vector3, TileInfo>();
        tileComponentDic = new Dictionary<Vector3, TileComponent>();
        attachDic = new Dictionary<Vector3, GameObject>();
        noAttachList = new List<Vector3>(); //禁建區,不會有附加物件的區域

        for (int i = this.transform.childCount; i > 0; --i)
        {
            DestroyImmediate(this.transform.GetChild(0).gameObject);
        }

        str = lines[0].Split(' ');
        width = int.Parse(str[0]);
        height = int.Parse(str[1]);

        for (int i = 1; i < lines.Length; i++) //第一行是長寬,忽視之
        {
            if (lines[i] != "")
            {
                if (i <= width)
                {
                    str = lines[i].Split(' ');
                    for (int j = 0; j < str.Length; j++)
                    {
                        position = new Vector3(i - 1, 0, j);
                        if (str[j] == "X")
                        {
                            visitedDic.Add(position, null);
                            tileInfoDic.Add(position, null);
                            continue;
                        }
                        else
                        {
                            tileScriptableObject = _tileScriptableObjectDic[str[j]];
                            visitedDic.Add(position, tileScriptableObject);
                            tileInfoDic.Add(position, new TileInfo(tileScriptableObject));
                            if (tileScriptableObject.Enqueue)
                            {
                                queue.Enqueue(position);
                            }
                        }
                    }
                }
                else
                {
                    noAttachList = JsonConvert.DeserializeObject<List<Vector3>>(lines[i]);
                }

            }
        }

        //BFS
        while (queue.Count != 0)
        {
            position = queue.Dequeue();

            tileScriptableObject = visitedDic[position];
            if ((position + Vector3.forward).z < height && visitedDic[position + Vector3.forward] == null)
            {
                visitedDic[position + Vector3.forward] = GetAdjacentTile(tileScriptableObject, Vector3.forward);
                tileInfoDic[position + Vector3.forward] = new TileInfo(visitedDic[position + Vector3.forward]);
                if (visitedDic[position + Vector3.forward].Enqueue)
                {
                    queue.Enqueue(position + Vector3.forward);
                }
            }
            if ((position + Vector3.back).z >= 0 && visitedDic[position + Vector3.back] == null)
            {
                visitedDic[position + Vector3.back] = GetAdjacentTile(tileScriptableObject, Vector3.back);
                tileInfoDic[position + Vector3.back] = new TileInfo(visitedDic[position + Vector3.back]);
                if (visitedDic[position + Vector3.back].Enqueue)
                {
                    queue.Enqueue(position + Vector3.back);
                }
            }
            if ((position + Vector3.left).x >= 0 && visitedDic[position + Vector3.left] == null)
            {
                visitedDic[position + Vector3.left] = GetAdjacentTile(tileScriptableObject, Vector3.left);
                tileInfoDic[position + Vector3.left] = new TileInfo(visitedDic[position + Vector3.left]);
                if (visitedDic[position + Vector3.left].Enqueue)
                {
                    queue.Enqueue(position + Vector3.left);
                }
            }
            if ((position + Vector3.right).x < width && visitedDic[position + Vector3.right] == null)
            {
                visitedDic[position + Vector3.right] = GetAdjacentTile(tileScriptableObject, Vector3.right);
                tileInfoDic[position + Vector3.right] = new TileInfo(visitedDic[position + Vector3.right]);
                if (visitedDic[position + Vector3.right].Enqueue)
                {
                    queue.Enqueue(position + Vector3.right);
                }
            }
        }

        //Attach
        foreach (KeyValuePair<Vector3, TileScriptableObject> pair in visitedDic)
        {
            if (!noAttachList.Contains(pair.Key))
            {
                attachScriptableObject = GetAttachIDRRandomly(pair.Value.Attachs); ;
                if (attachScriptableObject != null)
                {
                    tileInfoDic[pair.Key].SetAttach(attachScriptableObject.ID, attachScriptableObject.MoveCost);
                }
                else
                {
                    tileInfoDic[pair.Key].SetAttach(null, 0);
                }
            }
        }

        //使用 Life Game 去調整 Attach 的分布
        int count = 5;
        while (count > 0)
        {
            NextGeneration(width, height, tileInfoDic, noAttachList);
            count--;
        }

        for (int i = this.transform.childCount; i > 0; --i)
        {
            DestroyImmediate(this.transform.GetChild(0).gameObject);
        }

        foreach(KeyValuePair<Vector3, TileInfo> pair in tileInfoDic) 
        {
            tileObj = (GameObject)GameObject.Instantiate(Resources.Load("Tile/" + pair.Value.TileID), Vector3.zero, Quaternion.identity);
            Transform parent = GameObject.Find("Tilemap").transform;
            if (parent != null)
            {
                tileObj.transform.SetParent(parent);
            }
            tileObj.transform.position = pair.Key;
            tileComponentDic.Add(pair.Key, tileObj.GetComponent<TileComponent>());

            if (pair.Value.AttachID != null)
            {
                attachObj = (GameObject)GameObject.Instantiate(Resources.Load("Attach/" + pair.Value.AttachID), Vector3.zero, Quaternion.identity);
                attachObj.transform.position = tileObj.transform.position + new Vector3(0, pair.Value.Height - 0.5f, 0);
                attachObj.transform.parent = tileObj.transform;
                attachDic.Add(pair.Key, attachObj);
            }
        } 
    }

    private TileScriptableObject GetAdjacentTile(TileScriptableObject tile, Vector3 direction) 
    {
        int random;
        TileScriptableObject adjacentTile = null;
        List<string> pool = new List<string>();

        if(direction == Vector3.forward) 
        {
            pool = GetTilePool(tile.Up);
            random = Random.Range(0, pool.Count);
            adjacentTile = _tileScriptableObjectDic[pool[random]];
        }
        else if (direction == Vector3.back)
        {
            pool = GetTilePool(tile.Down);
            random = Random.Range(0, pool.Count);
            adjacentTile = _tileScriptableObjectDic[pool[random]];
        }
        else if (direction == Vector3.left)
        {
            pool = GetTilePool(tile.Left);
            random = Random.Range(0, pool.Count);
            adjacentTile = _tileScriptableObjectDic[pool[random]];
        }
        else if (direction == Vector3.right)
        {
            pool = GetTilePool(tile.Right);
            random = Random.Range(0, pool.Count);
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
        int random = Random.Range(0, attachs.Length + 1);
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

        int random = Random.Range(0, pool.Count);
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
    private Dictionary<Vector3, TileInfo> NextGeneration(int width, int height, Dictionary<Vector3, TileInfo> tileInfoDic, List<Vector3> noAttachList)
    {
        AttachScriptableObject attach;
        foreach(KeyValuePair<Vector3, TileInfo> pair in tileInfoDic) 
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

    private int CountNeighbors(int width, int height, Dictionary<Vector3, TileInfo> tileInfoDic, Vector3 position, string attach)
    {
        int count = 0;

        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                int x = (int)position.x + i;
                int z = (int)position.z + j;

                if (x < 0 || x >= width || z < 0 || z >= height || (i == 0 && j == 0))
                {
                    continue;
                }

                if ((attach == null && tileInfoDic[new Vector3(x, 0, z)].AttachID != null)  || (attach != null && tileInfoDic[new Vector3(x, 0, z)].AttachID == attach))
                {
                    count++;
                }
            }
        }

        return count;
    }
}
