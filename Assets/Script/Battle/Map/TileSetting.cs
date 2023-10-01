using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSetting
{
    public string ID;
    public string Comment;
    public int Height;
    public int MoveCost;
    public bool Enqueue; //�O�_�|�i�J wave function collapse �� BFS queue ��
    public List<Attach> Attachs = new List<Attach>(); //Tile �W�����F��,�Ҧp��,��,���Y...
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
