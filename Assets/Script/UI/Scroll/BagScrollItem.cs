using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BagScrollItem : ScrollItem
{
    public Text NameLabel;
    public Text AmountLabel;
    public ButtonColorSetting ColorSetting;

    public class Data
    {
        public Equip Equip;
        public int Weight;

        public Data(Equip equip, int weight) 
        {
            Equip = equip;
            Weight = weight;
        }
    }

    public override void SetData(object obj)
    {
        base.SetData(obj);

        if (obj is Item)
        {
            Item item = (Item)obj;
            NameLabel.text = item.Data.Name;
            AmountLabel.text = item.Amount.ToString();
        }
        else if (obj is Consumables)
        {
            Consumables consumables = (Consumables)obj;
            NameLabel.text = consumables.ItemData.Name;
            AmountLabel.text = consumables.Amount.ToString();
        }
        else if (obj is Card)
        {
            Card card = (Card)obj;
            NameLabel.text = card.ItemData.Name;
            AmountLabel.text = card.Amount.ToString();
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
        else if(obj is Data) 
        {
            Data data = (Data)obj;
            Equip equip = data.Equip;
            int weight = data.Weight;
            NameLabel.text = equip.Name;
            AmountLabel.text = "";
            ColorSetting.SetColor(weight >= equip.Weight);
        }
    }
}
