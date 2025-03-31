using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BagScrollItem : ScrollItem
{
    public Text NameLabel;
    public Text AmountLabel;
    public ButtonColorSetting ColorSetting;

    //public new class Data
    //{
    //    public Equip Equip;
    //    public int Weight;

    //    public Data(Equip equip, int weight) 
    //    {
    //        Equip = equip;
    //        Weight = weight;
    //    }
    //}

    public override void SetData(object obj)
    {
        base.SetData(obj);

        if (obj is Item)
        {
            Item item = (Item)obj;
            NameLabel.text = item.Data.Name;
            AmountLabel.text = item.Amount.ToString();
        }
        else if(obj is Food)
        {
            Food food = (Food)obj;
            NameLabel.text = food.Name;
            AmountLabel.text = "";
        }
        else if(obj is Equip)
        {
            Equip equip = (Equip)obj;
            NameLabel.text = equip.Name;
            AmountLabel.text = "";
        }
    }
}
