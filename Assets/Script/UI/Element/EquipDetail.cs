using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipDetail : MonoBehaviour
{
    public Text NameLabel;
    public Text CommentLabel;
    public Text WeightLabel;
    public Text ATKLabel;
    public Text DEFLabel;
    public Text MTKLabel;
    public Text MEFLabel;
    public Text OtherLabel;

    public void SetData(Equip equip) 
    {
        if (equip != null)
        {
            NameLabel.text = equip.Name;
            CommentLabel.text = equip.Comment;
            if (equip.Weight == 1)
            {
                WeightLabel.text = "��";
            }
            else if (equip.Weight == 2)
            {
                WeightLabel.text = "��";
            }
            else
            {
                WeightLabel.text = "";
            }
            ATKLabel.text = "���z�����G" + equip.ATK;
            DEFLabel.text = "���z���m�G" + equip.DEF;
            MTKLabel.text = "�]�k�����G" + equip.MTK;
            MEFLabel.text = "�]�k���m�G" + equip.MEF;
            OtherLabel.text = "";
            if (equip.STR != 0)
            {
                OtherLabel.text += "�O�q+" + equip.STR + " ";
            }
            if (equip.CON != 0)
            {
                OtherLabel.text += "���+" + equip.CON + " ";
            }
            if (equip.INT != 0)
            {
                OtherLabel.text += "���O+" + equip.INT + " ";
            }
            if (equip.MEN != 0)
            {
                OtherLabel.text += "�믫+" + equip.MEN + " ";
            }
            if (equip.DEX != 0)
            {
                OtherLabel.text += "�F��+" + equip.DEX + " ";
            }
            if (equip.AGI != 0)
            {
                OtherLabel.text += "�ӱ�+" + equip.AGI + " ";
            }
            if (equip.MOV != 0)
            {
                OtherLabel.text += "����+" + equip.MOV + " ";
            }
        }
    }
}
