using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class StatusModel
{
    public enum TypeEnum
    {
        None = -1,
        ATK = 1,
        DEF,
    }

    public TypeEnum Type;
    public int ID;
    public string Name;
    public string Comment;
    public int Value;
    public int Time;
}
