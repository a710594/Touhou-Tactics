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
            ATKLabel.text = "攻擊：" + equipData.ATK;
            DEFLabel.text = "防禦：" + equipData.DEF;
            MTKLabel.text = "魔攻：" + equipData.MTK;
            MEFLabel.text = "魔防：" + equipData.MEF;
            HPLabel.text = "HP：" + equipData.HP;
            STRLabel.text = "力量：" + equipData.STR;
            CONLabel.text = "體質：" + equipData.CON;
            INTLabel.text = "智力：" + equipData.INT;
            MENLabel.text = "精神：" + equipData.MEN;
            DEXLabel.text = "靈巧：" + equipData.DEX;
            AGILabel.text = "敏捷：" + equipData.AGI;
            MOVLabel.text = "移動：" + equipData.MOV;
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
