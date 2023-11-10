using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipModel
{
    public enum CategoryEnum 
    {
        Weapon = 1,
        Armor,
        Amulet
    }

    public CategoryEnum Category;
    public int ID;
    public int Weight;
    public int ATK;
    public int DEF;
    public int MTK;
    public int MEF;
    public int HP;
    public int STR; //Strength �O�q �v�T���z�ˮ`
    public int CON; //Constitution ��� ��ܪ��z�ˮ`
    public int INT; //Intelligence ���O �v�T�k�N�ˮ`
    public int MEN; //mentality �믫 ��ܪk�N�ˮ`
    public int DEX; //Dexterity �F�� �v�T�R���v
    public int AGI; //Agility �ӱ� ��ܩR���v
    public int MOV; //move ����
}
