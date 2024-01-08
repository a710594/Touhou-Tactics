using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class BattleFileGenerator : MonoBehaviour
{
    public bool IsTutorial;
    public string FileName;
    public int Width;
    public int Height;
    public Transform Tilemap;
    public Transform EnemyGroup;

    private string _prePath;

    public void BuildFile() 
    {
        Dictionary<Vector2Int, string> tileDic = new Dictionary<Vector2Int, string>();
        TileComponent component;
        //List<Vector2Int> noAttachList = new List<Vector2Int>(); //禁建區,不會有附加物件的區域
        List<int[]> noAttachList = new List<int[]>(); //禁建區,不會有附加物件的區域
        foreach (Transform child in Tilemap)
        {
            component = child.GetComponent<TileComponent>();
            tileDic.Add(Utility.ConvertToVector2Int(child.position), component.ID);
            if(component.tag == "NoAttach") 
            {
                //noAttachList.Add(new Vector2Int((int)child.position.x, (int)child.position.z));
                noAttachList.Add(new int[2] { (int)child.position.x, (int)child.position.z });
            }
        }

        BattleMapEnemy battleMapEnemy;
        List<int[]> enemyList = new List<int[]>(); //敵人的位置和ID
        foreach (Transform child in EnemyGroup) 
        {
            battleMapEnemy = child.GetComponent<BattleMapEnemy>();
            enemyList.Add(new int[4] { (int)child.position.x, (int)child.position.y, (int)child.position.z , battleMapEnemy.ID});
        }

        _prePath = Application.streamingAssetsPath + "/Map/Battle/";
        string path = Path.Combine(_prePath, FileName + ".txt");
        Vector2Int position;
        using (StreamWriter writer = new StreamWriter(path))
        {
            writer.Write(IsTutorial + "\n");
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
            writer.WriteLine(JsonConvert.SerializeObject(noAttachList));
            writer.Write(JsonConvert.SerializeObject(enemyList));
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

        DataContext.Instance.Init();

        for (int i = Tilemap.childCount; i > 0; --i)
        {
            DestroyImmediate(Tilemap.GetChild(0).gameObject);
        }

        for (int i = EnemyGroup.childCount; i > 0; --i)
        {
            DestroyImmediate(EnemyGroup.GetChild(0).gameObject);
        }

        str = lines[1].Split(' ');
        width = int.Parse(str[0]);
        height = int.Parse(str[1]);

        for (int i=2; i<lines.Length; i++)
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
                            tile = (GameObject)GameObject.Instantiate(Resources.Load("Tile/" + str[j]), Vector3.zero, Quaternion.identity);
                            tile.transform.SetParent(Tilemap);
                            tile.transform.position = new Vector3(i - 1, 0, j);
                            tileList.Add(Utility.ConvertToVector2Int(tile.transform.position), tile);
                        }
                    }
                }
                else if(i == width + 1)
                {
                    //List<Vector2Int> noAttachList = JsonConvert.DeserializeObject<List<Vector2Int>>(lines[i]);
                    List<int[]> noAttachList = JsonConvert.DeserializeObject<List<int[]>>(lines[i]);
                    for (int j = 0; j < noAttachList.Count; j++)
                    {
                        tileList[new Vector2Int(noAttachList[j][0], noAttachList[j][1])].tag = "NoAttach";
                    }
                }
                else 
                {
                    Vector3 position;
                    GameObject obj;
                    BattleMapEnemy battleMapEnemy;
                    List<int[]> enemyList = JsonConvert.DeserializeObject<List<int[]>>(lines[i]);
                    for (int j = 0; j < enemyList.Count; j++)
                    {
                        position = new Vector3(enemyList[j][0], enemyList[j][1], enemyList[j][2]);
                        obj = (GameObject)GameObject.Instantiate(Resources.Load("Prefab/Other/BattleMapEnemy"), Vector3.zero, Quaternion.identity);
                        battleMapEnemy = obj.GetComponent<BattleMapEnemy>();
                        battleMapEnemy.Init(enemyList[j][3]);
                        battleMapEnemy.transform.SetParent(EnemyGroup);
                        battleMapEnemy.transform.position = position;
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
