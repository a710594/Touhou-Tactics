using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BagEquipGroup : MonoBehaviour
{
    public ScrollView ScrollView;
    public Text CommentLabel;
    public Text ATKLabel;
    public Text DEFLabel;
    public Text MTKLabel;
    public Text MEFLabel;
    public Text HPLabel;
    public Text STRLabel;
    public Text CONLabel;
    public Text INTLabel;
    public Text MENLabel;
    public Text DEXLabel;
    public Text AGILabel;
    public Text MOVLabel;

    public void SetScrollView() 
    {
        ScrollView.SetData(new List<object>(ItemManager.Instance.BagInfo.EquipList));
    }

    public void SetDetail(Equip equip) 
    {
        if (equip != null)
        {
            CommentLabel.text = equip.Comment;
            ATKLabel.text = "攻擊：" + equip.ATK;
            DEFLabel.text = "防禦：" + equip.DEF;
            MTKLabel.text = "魔攻：" + equip.MTK;
            MEFLabel.text = "魔防：" + equip.MEF;
            HPLabel.text = "HP：" + equip.HP;
            STRLabel.text = "力量：" + equip.STR;
            CONLabel.text = "體質：" + equip.CON;
            INTLabel.text = "智力：" + equip.INT;
            MENLabel.text = "精神：" + equip.MEN;
            DEXLabel.text = "靈巧：" + equip.DEX;
            AGILabel.text = "敏捷：" + equip.AGI;
            MOVLabel.text = "移動：" + equip.MOV;
        }
        else
        {
            CommentLabel.text = "";
            ATKLabel.text = "";
            DEFLabel.text = "";
            MTKLabel.text = "";
            MEFLabel.text = "";
            HPLabel.text = "";
            STRLabel.text = "";
            CONLabel.text = "";
            INTLabel.text = "";
            MENLabel.text = "";
            DEXLabel.text = "";
            AGILabel.text = "";
            MOVLabel.text = "";
        }
    }

    private void ScrollItemOnClick(object obj, ScrollItem scrollItem)
    {
        Equip equip = (Equip)obj;
        SetDetail(equip);
    }

    private void Awake()
    {
        ScrollView.ItemOnClickHandler += ScrollItemOnClick;
    }
}
