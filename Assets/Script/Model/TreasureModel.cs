using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureModel
{
    public enum TypeEnum 
    {
        Item = 1,
        Equip,
    }

    public int ID;
    public TypeEnum Type;
    public string Prefab;
    public float Height;
    public string Rotation;
    public int ID_1;
    public int ID_2;
    public int ID_3;
    public int ID_4;

    public List<int> IDList = new List<int>();

    public void GetList()
    {
        if (ID_1 != -1)
            IDList.Add(ID_1);
        if (ID_2 != -1)
            IDList.Add(ID_2);
        if (ID_3 != -1)
            IDList.Add(ID_3);
        if (ID_4 != -1)
            IDList.Add(ID_4);
    }

    public int GetItem()
    {

        return IDList[Random.Range(0, IDList.Count)];
    }
}
