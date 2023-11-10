using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopModel
{
    public int ID;
    public int Price;
    public int UnlockFloor;
    public int MaterialID_1;
    public int MaterialID_2;
    public int MaterialID_3;
    public int MaterialAmount_1;
    public int MaterialAmount_2;
    public int MaterialAmount_3;
    public List<int> MaterialIDList = new List<int>();
    public List<int> MaterialAmountList = new List<int>();

    public void GetList()
    {
        if (MaterialID_1 != -1)
        {
            MaterialIDList.Add(MaterialID_1);
            MaterialAmountList.Add(MaterialAmount_1);
        }
        if (MaterialID_2 != -1)
        {
            MaterialIDList.Add(MaterialID_2);
            MaterialAmountList.Add(MaterialAmount_2);
        }
        if (MaterialID_3 != -1)
        {
            MaterialIDList.Add(MaterialID_3);
            MaterialAmountList.Add(MaterialAmount_3);
        }
    }
}
