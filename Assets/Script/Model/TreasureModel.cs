using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureModel
{
    public enum TypeEnum 
    {
        Normal = 1,
        Special,
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

    public List<int> ItemList = new List<int>();

    public void SetItemList()
    {
        if (ID_1 != -1)
            ItemList.Add(ID_1);
        if (ID_2 != -1)
            ItemList.Add(ID_2);
        if (ID_3 != -1)
            ItemList.Add(ID_3);
        if (ID_4 != -1)
            ItemList.Add(ID_4);
    }

    public int GetItemID()
    {
        return ItemList[Random.Range(0, ItemList.Count)];
    }
}
