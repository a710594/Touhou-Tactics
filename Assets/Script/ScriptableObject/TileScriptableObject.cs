using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class TileScriptableObject : ScriptableObject
{
    public string ID;
    public string Comment;
    public int Height;
    public int MoveCost;
    public bool Enqueue; //是否會進入 wave function collapse 的 BFS queue 中
    public Attach[] Attachs; //Tile 上面的東西,例如草,樹,石頭...
    public Contact[] Up;
    public Contact[] Down;
    public Contact[] Left;
    public Contact[] Right;

    [Serializable]
    public class Contact 
    {
        public string ID;
        public int Probability;
    }

    [Serializable]
    public class Attach
    {
        public string ID;
        public int Probability;
    }
}
