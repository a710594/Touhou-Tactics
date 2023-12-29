using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class MapReader
{
    //private string _prePath = Application.streamingAssetsPath;
    //private TileScriptableObject[] _tileScriptableObjects;
    //private AttachScriptableObject[] _attachScriptableObjects;
    //private Dictionary<string, TileScriptableObject> _tileScriptableObjectDic = new Dictionary<string, TileScriptableObject>();
    //private Dictionary<string, AttachScriptableObject> _attachScriptableObjectDic = new Dictionary<string, AttachScriptableObject>();

    //public void LoadData()
    //{
    //    DirectoryInfo d;
    //    FileInfo[] Files;
    //    string str = "";
    //    AttachScriptableObject attach;
    //    TileScriptableObject tile;

    //    d = new DirectoryInfo(_prePath + "./Attach/"); //Assuming Test is your Folder
    //    Files = d.GetFiles("*.json"); //Getting Text files
    //    foreach (FileInfo file in Files)
    //    {
    //        str = str + ", " + file.Name;
    //        string jsonString = File.ReadAllText(file.FullName);
    //        attach = JsonConvert.DeserializeObject<AttachScriptableObject>(jsonString);
    //        _attachScriptableObjectDic.Add(attach.ID, attach);

    //    }

    //    d = new DirectoryInfo(_prePath + "./Tile/"); //Assuming Test is your Folder
    //    Files = d.GetFiles("*.json"); //Getting Text files
    //    foreach (FileInfo file in Files)
    //    {
    //        str = str + ", " + file.Name;
    //        string jsonString = File.ReadAllText(file.FullName);
    //        tile = JsonConvert.DeserializeObject<TileScriptableObject>(jsonString);
    //        _tileScriptableObjectDic.Add(tile.ID, tile);

    //    }
    //}

    public void Read(string path, out BattleMapInfo battleInfo) 
    {
        string text = File.ReadAllText(path);
        string[] stringSeparators = new string[] { "\r\n" };
        string[] lines = text.Split(stringSeparators, StringSplitOptions.None);
        string[] str;
        Vector2Int position = new Vector2Int();
        TileScriptableObject tileScriptableObject;
        AttachScriptableObject attachScriptableObject;
        GameObject tileObj;
        GameObject attachObj;
        battleInfo = new BattleMapInfo();
        battleInfo.TileComponentDic = new Dictionary<Vector2Int, TileComponent>();
        battleInfo.AttachDic = new Dictionary<Vector2Int, GameObject>();
        battleInfo.TileInfoDic = new Dictionary<Vector2Int, TileInfo>();
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
                        tileScriptableObject = DataContext.Instance.TileScriptableObjectDic[str[j]];
                        battleInfo.TileInfoDic.Add(position, new TileInfo(tileScriptableObject));
                    }
                }
                else
                {
                    str = lines[i].Split(' ');
                    for (int j = 0; j < str.Length; j++)
                    {
                        position = new Vector2Int(i - 1 - battleInfo.Width, j);
                        if (DataContext.Instance.AttachScriptableObjectDic.ContainsKey(str[j]))
                        {
                            attachScriptableObject = DataContext.Instance.AttachScriptableObjectDic[str[j]];
                            battleInfo.TileInfoDic[position].SetAttach(attachScriptableObject.ID, attachScriptableObject.MoveCost);
                        }
                    }
                }
            }
        }

        foreach (KeyValuePair<Vector2Int, TileInfo> pair in battleInfo.TileInfoDic)
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
}

