using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BagEquipGroup : MonoBehaviour
{
    public Action<object> ScrollHandler;

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

    public void SetScrollView(EquipModel.CategoryEnum category, CharacterInfo character) 
    {
        BagScrollItem.Data data;
        List<BagScrollItem.Data> list = new List<BagScrollItem.Data>();
        for (int i=0; i<ItemManager.Instance.BagInfo.EquipList.Count; i++) 
        {
            if(ItemManager.Instance.BagInfo.EquipList[i].Category == category) 
            {
                data = new BagScrollItem.Data(ItemManager.Instance.BagInfo.EquipList[i], character.Weight);
                list.Add(data);
            }
        }

        if (character.Weight == 1)
        {
            list.Sort((x, y) =>
            {
                return x.Equip.Weight.CompareTo(y.Equip.Weight);
            });
        }
        else
        {
            list.Sort((x, y) =>
            {
                return y.Equip.Weight.CompareTo(x.Equip.Weight);
            });
        }
        list.Insert(0, new BagScrollItem.Data(new Equip(category), character.Weight));

        ScrollView.SetData(new List<object>(list));
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

    private void ScrollItemOnClick(ScrollItem scrollItem)
    {
        object obj = scrollItem.Data;
        Equip equip = null;
        if (obj is Equip)
        {
            equip = (Equip)obj;
        }
        else if(obj is BagScrollItem.Data) 
        {
            BagScrollItem.Data data = (BagScrollItem.Data)obj;
            equip = data.Equip;
        }
        SetDetail(equip);
        ScrollHandler(obj);
    }

    private void Awake()
    {
        ScrollView.ClickHandler += ScrollItemOnClick;
    }
}
