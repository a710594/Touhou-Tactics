using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�b�ө��ʶR���˳Ƽƭȷ|�O�зǭ�
//���O�q�_�c�}�X�Ӫ��˳Ƽƭȷ|�O�зǭȪ�50%~200%
public class Equip
{
    public EquipModel.CategoryEnum Category;
    public int ID;
    public string Name;
    public string Comment;
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

    public Equip(EquipModel.CategoryEnum category) 
    {
        Category = category;
        ID = 0;
        Name = "�L";
        Comment = "";
        HP = 0;
        ATK = 0;
        DEF = 0;
        MTK = 0;
        MEF = 0;
        DEX = 0;
        AGI = 0;
        MOV = 0;
        Weight = 1;
    }

    public Equip(EquipModel data) 
    {
        Category = data.Category;
        ID = data.ID;
        Name = data.Name;
        Comment = data.Comment;
        HP = data.HP;
        ATK = data.ATK;
        DEF = data.DEF;
        MTK = data.MTK;
        MEF = data.MEF;
        DEX = data.DEX;
        AGI = data.AGI;
        MOV = data.MOV;
        Weight = data.Weight;
    }
}