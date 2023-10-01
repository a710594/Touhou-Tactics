using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSetting
{
    public string ID;
    public string Comment;
    public int Height;
    public int MoveCost;
    public bool Enqueue; //是否會進入 wave function collapse 的 BFS queue 中
    public List<Attach> Attachs = new List<Attach>(); //Tile 上面的東西,例如草,樹,石頭...
    public List<Contact> Up = new List<Contact>();
    public List<Contact> Down = new List<Contact>();
    public List<Contact> Left = new List<Contact>();
    public List<Contact> Right = new List<Contact>();

    public class Contact
    {
        public string ID;
        public int Probability;
    }

    public class Attach
    {
        public string ID;
        public int Probability;
    }
}
