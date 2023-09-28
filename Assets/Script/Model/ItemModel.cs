using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemModel
{
    public enum CategoryEnum
    {
        Medicine = 1,
        Card,
        Material,
    }

    public CategoryEnum Category;
    public int ID;
    public string Name;
    public string Description;
    public EffectModel.TypeEnum EffectType;
    public int EffectID;
    public int Job;
    public int PP;
}
