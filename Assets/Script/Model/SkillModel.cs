using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class SkillModel
{
    public enum TypeEnum 
    {
        Normal = 1,
        Support,
        Card,
    }

    public TypeEnum Type;
    public int ID;  
    public string Name;
    public string Comment;
    public int CD;
    public int SP;
    public EffectModel.TypeEnum EffectType;
    public int EffectID;
}