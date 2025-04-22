using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class EffectModel
{
    public enum TypeEnum
    {
        None = -1,
        Idle = 0,
        PhysicalAttack,
        MagicAttack,
        Recover,
        Provocative,
        Buff,
        Poison,
        Purify,
        Medicine,
        Sleep,
        Summon,
        RatioDamage,
        RecoverAll
    }

    public enum TargetEnum 
    {
        None=-1,
        Self=1,
        MinHP=2,
    }

    public TypeEnum Type;
    public int ID;
    public string Name;
    public int Value;
    public int StatusID;
    public TargetEnum Target;
    public int SubID;
}
