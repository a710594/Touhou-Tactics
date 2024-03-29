using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopEquipGroup : MonoBehaviour
{
    public Action<ShopModel> ShopDataHandler;
    public Action<Equip> EquipHandler;

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
    public Text MaterialLabel;

    public void SetScrollViewBuy()
    {
        ScrollView.SetData(new List<object>(DataContext.Instance.ShopItemDic[ItemModel.CategoryEnum.Equip]));
    }

    public void SetScrollViewSell()
    {
        ScrollView.SetData(new List<object>(ItemManager.Instance.BagInfo.EquipList));
    }

    public void CancelScrollViewSelect()
    {
        ScrollView.CancelSelect();
        MaterialLabel.text = "";
    }

    public void SetDetailByData(ItemModel itemData, EquipModel equipData)
    {
        if (equipData != null)
        {
            CommentLabel.text = itemData.Comment;
            ATKLabel.text = "�����G" + equipData.ATK;
            DEFLabel.text = "���m�G" + equipData.DEF;
            MTKLabel.text = "�]��G" + equipData.MTK;
            MEFLabel.text = "�]���G" + equipData.MEF;
            HPLabel.text = "HP�G" + equipData.HP;
            STRLabel.text = "�O�q�G" + equipData.STR;
            CONLabel.text = "���G" + equipData.CON;
            INTLabel.text = "���O�G" + equipData.INT;
            MENLabel.text = "�믫�G" + equipData.MEN;
            DEXLabel.text = "�F���G" + equipData.DEX;
            AGILabel.text = "�ӱ��G" + equipData.AGI;
            MOVLabel.text = "���ʡG" + equipData.MOV;
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

    public void SetDetailByEquip(Equip equip)
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

    public void SetMaterial(ShopModel shopData) 
    {
        ItemModel itemData;
        MaterialLabel.text = "";
        for (int i = 0; i < shopData.MaterialIDList.Count; i++)
        {
            itemData = DataContext.Instance.ItemDic[shopData.MaterialIDList[i]];
            MaterialLabel.text += itemData.Name + " " + ItemManager.Instance.GetAmount(itemData.ID) + "/" + shopData.MaterialAmountList[i] + " ";
        }
    }

    private void ScrollItemOnClick(ScrollItem scrollItem)
    {
        if (scrollItem.Data is ShopModel)
        {
            ShopModel shopData = (ShopModel)scrollItem.Data;
            SetDetailByData(DataContext.Instance.ItemDic[shopData.ID], DataContext.Instance.EquipDic[shopData.ID]);
            SetMaterial(shopData);
            ShopDataHandler(shopData);
        }
        else
        {
            Equip equip = (Equip)scrollItem.Data;
            SetDetailByEquip(equip);
            MaterialLabel.text = "";
            EquipHandler(equip);
;        }
    }

    private void Awake()
    {
        ScrollView.ClickHandler += ScrollItemOnClick;
    }
}
