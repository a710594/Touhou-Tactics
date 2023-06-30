using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingNumberData
{
    public enum TypeEnum
    {
        Damage,
        Recover,
        Poison,
        Miss,
        Critical,
        Paralysis,
        Sleeping,
        Confusion,
        Other,
    }

    public TypeEnum Type;
    public string Text;

    public FloatingNumberData(TypeEnum type, string text)
    {
        Type = type;
        Text = text;
    }
}
