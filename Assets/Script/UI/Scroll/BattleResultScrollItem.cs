using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleResultScrollItem : ScrollItem
{
    public Text Label;

    public override void SetData(object obj)
    {
        base.SetData(obj);
        int id = (int)obj;

        Label.text = DataContext.Instance.ItemDic[ItemModel.CategoryEnum.Material][id].Name;
    }
}
