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
    private string _prePath = Application.streamingAssetsPath;
    private TileScriptableObject[] _tileScriptableObjects;
    private AttachScriptableObject[] _attachScriptableObjects;
    private Dictionary<string, TileScriptableObject> _tileScriptableObjectDic = new Dictionary<string, TileScriptableObject>();
    private Dictionary<string, AttachScriptableObject> _attachScriptableObjectDic = new Dictionary<string, AttachScriptableObject>();

    public void LoadData()
    {
        DirectoryInfo d;
        FileInfo[] Files;
        string str = "";
        AttachScriptableObject attach;
        TileScriptableObject tile;

        d = new DirectoryInfo(_prePath + "./Attach/"); //Assuming Test is your Folder
        Files = d.GetFiles("*.json"); //Getting Text files
        foreach (FileInfo file in Files)
        {
            str = str + ", " + file.Name;
            string jsonString = File.ReadAllText(file.FullName);
            attach = JsonConvert.DeserializeObject<AttachScriptableObject>(jsonString);
            _attachScriptableObjectDic.Add(attach.ID, attach);

        }

        d = new DirectoryInfo(_prePath + "./Tile/"); //Assuming Test is your Folder
        Files = d.GetFiles("*.json"); //Getting Text files
        foreach (FileInfo file in Files)
        {
            str = str + ", " + file.Name;
            string jsonString = File.ReadAllText(file.FullName);
            tile = JsonConvert.DeserializeObject<TileScriptableObject>(jsonString);
            _tileScriptableObjectDic.Add(tile.ID, tile);

        }
    }

    public void Read(string path, out int width, out int height, out Dictionary<Vector2, TileInfo> tileInfoDic, out Dictionary<Vector2, TileComponent> tileComponentDic, out Dictionary<Vector2, GameObject> attachDic) 
    {
        string text = File.ReadAllText(path);
        string[] lines = text.Split('\n', '\r');
        string[] str;
        Vector2 position = new Vector2();
        TileScriptableObject tileScriptableObject;
        AttachScriptableObject attachScriptableObject;
        GameObject tileObj;
        GameObject attachObj; ;
        tileComponentDic = new Dictionary<Vector2, TileComponent>();
        attachDic = new Dictionary<Vector2, GameObject>();

        tileInfoDic = new Dictionary<Vector2, TileInfo>();
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
                        position = new Vector2(i - 1, j);
                        tileScriptableObject = _tileScriptableObjectDic[str[j]];
                        tileInfoDic.Add(position, new TileInfo(tileScriptableObject));
                    }
                }
                else
                {
                    str = lines[i].Split(' ');
                    for (int j = 0; j < str.Length; j++)
                    {
                        position = new Vector2(i - 1 - width, j);
                        if (_attachScriptableObjectDic.ContainsKey(str[j]))
                        {
                            attachScriptableObject = _attachScriptableObjectDic[str[j]];
                            tileInfoDic[position].SetAttach(attachScriptableObject.ID, attachScriptableObject.MoveCost);
                        }
                    }
                }
            }
        }

        foreach (KeyValuePair<Vector2, TileInfo> pair in tileInfoDic)
        {
            tileObj = (GameObject)GameObject.Instantiate(Resources.Load("Tile/" + pair.Value.TileID), Vector3.zero, Quaternion.identity);
            Transform parent = GameObject.Find("Tilemap").transform;
            if (parent != null)
            {
                tileObj.transform.SetParent(parent);
            }
            tileObj.transform.position = new Vector3(pair.Key.x, 0, pair.Key.y);
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
}

