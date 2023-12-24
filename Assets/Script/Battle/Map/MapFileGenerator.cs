using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MapFileGenerator : MonoBehaviour
{
    public string FileName;
    public int Width;
    public int Height;

    private string _prePath = Application.streamingAssetsPath;

    public void BuildFile() 
    {
        Dictionary<Vector2Int, string> tileDic = new Dictionary<Vector2Int, string>();
        TileComponent component;
        List<Vector2Int> noAttachList = new List<Vector2Int>(); //禁建區,不會有附加物件的區域
        foreach (Transform child in transform)
        {
            component = child.GetComponent<TileComponent>();
            tileDic.Add(Utility.ConvertToVector2Int(child.position), component.ID);
            if(component.tag == "NoAttach") 
            {
                noAttachList.Add(new Vector2Int((int)child.position.x, (int)child.position.z));
            }
        }

        string path = Path.Combine(_prePath, FileName + ".txt");
        Vector2Int position;
        using (StreamWriter writer = new StreamWriter(path))
        {
            writer.Write(Width + " " + Height + "\n");

            for (int i=0; i<Width; i++)
            {
                for (int j=0; j<Height; j++) 
                {
                    if (j > 0) 
                    {
                        writer.Write(" ");
                    }

                    position = new Vector2Int(i, j);
                    if (tileDic.ContainsKey(position))
                    {
                        writer.Write(tileDic[position]);
                    }
                    else 
                    {
                        writer.Write("X");
                    }
                }
                writer.Write("\n");
            }
            writer.Write(JsonConvert.SerializeObject(noAttachList));
        }
    }

    public void LoadFile() 
    {
        int width;
        int height;
        string path = Path.Combine(_prePath, FileName + ".txt");
        string text = File.ReadAllText(path);
        string[] stringSeparators = new string[] { "\n", "\r\n" };
        string[] lines = text.Split(stringSeparators, StringSplitOptions.None);
        string[] str;
        GameObject tile;
        Dictionary<Vector2Int, GameObject> tileList = new Dictionary<Vector2Int, GameObject>();

        for (int i = this.transform.childCount; i > 0; --i)
        {
            DestroyImmediate(this.transform.GetChild(0).gameObject);
        }

        str = lines[0].Split(' ');
        width = int.Parse(str[0]);
        height = int.Parse(str[1]);

        for (int i=1; i<lines.Length; i++) //第一行是長寬,忽視之
        {
            if (lines[i] != "")
            {
                if (i <= width)
                {
                    str = lines[i].Split(' ');
                    for (int j = 0; j < str.Length; j++)
                    {
                        if (str[j] == "X")
                        {
                            continue;
                        }
                        else
                        {
                            Debug.Log(str[j]);
                            tile = (GameObject)GameObject.Instantiate(Resources.Load("Tile/" + str[j]), Vector3.zero, Quaternion.identity);
                            Transform parent = GameObject.Find("Tilemap").transform;
                            if (parent != null)
                            {
                                tile.transform.SetParent(parent);
                            }
                            tile.transform.position = new Vector3(i - 1, 0, j);
                            tileList.Add(Utility.ConvertToVector2Int(tile.transform.position), tile);
                        }
                    }
                }
                else
                {
                    List<Vector2Int> noAttachList = JsonConvert.DeserializeObject<List<Vector2Int>>(lines[i]);
                    for (int j = 0; j < noAttachList.Count; j++)
                    {
                        tileList[noAttachList[j]].tag = "NoAttach";
                    }
                }
            }
        }
    }

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
