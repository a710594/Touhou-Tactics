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
            ATKLabel.text = "�����G" + equip.ATK;
            DEFLabel.text = "���m�G" + equip.DEF;
            MTKLabel.text = "�]��G" + equip.MTK;
            MEFLabel.text = "�]���G" + equip.MEF;
            HPLabel.text = "HP�G" + equip.HP;
            STRLabel.text = "�O�q�G" + equip.STR;
            CONLabel.text = "���G" + equip.CON;
            INTLabel.text = "���O�G" + equip.INT;
            MENLabel.text = "�믫�G" + equip.MEN;
            DEXLabel.text = "�F���G" + equip.DEX;
            AGILabel.text = "�ӱ��G" + equip.AGI;
            MOVLabel.text = "���ʡG" + equip.MOV;
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
