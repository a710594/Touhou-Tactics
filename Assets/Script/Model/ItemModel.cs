using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemModel
{
    public enum CategoryEnum
    {
        Consumables = 1,
        Food,
        Card,
        Equip,
        Item,
    }

    public CategoryEnum Category;
    public int ID;
    public string Name;
    public string Comment;
    public int Price;
}
