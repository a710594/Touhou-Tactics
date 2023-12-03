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
                WeightLabel.text = "輕";
            }
            else if (equip.Weight == 2)
            {
                WeightLabel.text = "重";
            }
            else
            {
                WeightLabel.text = "";
            }
            ATKLabel.text = "物理攻擊：" + equip.ATK;
            DEFLabel.text = "物理防禦：" + equip.DEF;
            MTKLabel.text = "魔法攻擊：" + equip.MTK;
            MEFLabel.text = "魔法防禦：" + equip.MEF;
            OtherLabel.text = "";
            if (equip.STR != 0)
            {
                OtherLabel.text += "力量+" + equip.STR + " ";
            }
            if (equip.CON != 0)
            {
                OtherLabel.text += "體質+" + equip.CON + " ";
            }
            if (equip.INT != 0)
            {
                OtherLabel.text += "智力+" + equip.INT + " ";
            }
            if (equip.MEN != 0)
            {
                OtherLabel.text += "精神+" + equip.MEN + " ";
            }
            if (equip.DEX != 0)
            {
                OtherLabel.text += "靈巧+" + equip.DEX + " ";
            }
            if (equip.AGI != 0)
            {
                OtherLabel.text += "敏捷+" + equip.AGI + " ";
            }
            if (equip.MOV != 0)
            {
                OtherLabel.text += "移動+" + equip.MOV + " ";
            }
        }
    }
}
