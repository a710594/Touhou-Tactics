using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookModel
{
    public int ID;
    public int Material_1;
    public int Material_2;
    public int Material_3;
    public int Material_4;
    public int Material_5;
    public int Result;
    public List<int> MaterialList = new List<int>();

    public void GetList()
    {
        if (Material_1 != -1)
        {
            MaterialList.Add(Material_1);
        }
        if (Material_2 != -1)
        {
            MaterialList.Add(Material_2);
        }
        if (Material_3 != -1)
        {
            MaterialList.Add(Material_3);
        }
        if (Material_4 != -1)
        {
            MaterialList.Add(Material_4);
        }
        if (Material_5 != -1)
        {
            MaterialList.Add(Material_5);
        }
    }
}
