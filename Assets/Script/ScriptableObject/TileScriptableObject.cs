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
    public bool Enqueue; //�O�_�|�i�J wave function collapse �� BFS queue ��
    public Attach[] Attachs; //Tile �W�����F��,�Ҧp��,��,���Y...
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
