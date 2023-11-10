using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class ShopUI : MonoBehaviour
{
    private class BuyState: ShopState 
    {
        public BuyState(StateContext context) : base(context)
        {

        }

        public override void ConsumablesOnClick() 
        {
            _shopContext.ShopUI.ShopItemGroup.gameObject.SetActive(true);
            _shopContext.ShopUI.ShopEquipGroup.gameObject.SetActive(false);
            _shopContext.ShopUI.ShopItemGroup.SetScrollViewBuy(ItemModel.CategoryEnum.Consumables);
            _shopContext.ShopUI.ShopItemGroup.CancelScrollViewSelect();
            _shopContext.ShopUI.ShopItemGroup.SetComment("");
            _shopContext.ShopUI._selectedShopData = null;
        }

        public override void CardOnClick() 
        {
            _shopContext.ShopUI.ShopItemGroup.gameObject.SetActive(true);
            _shopContext.ShopUI.ShopEquipGroup.gameObject.SetActive(false);
            _shopContext.ShopUI.ShopItemGroup.SetScrollViewBuy(ItemModel.CategoryEnum.Card);
            _shopContext.ShopUI.ShopItemGroup.CancelScrollViewSelect();
            _shopContext.ShopUI.ShopItemGroup.SetComment("");
            _shopContext.ShopUI._selectedShopData = null;
        }

        public override void EquipOnClick() 
        {
            _shopContext.ShopUI.ShopItemGroup.gameObject.SetActive(false);
            _shopContext.ShopUI.ShopEquipGroup.gameObject.SetActive(true);
            _shopContext.ShopUI.ShopEquipGroup.SetScrollViewBuy();
            _shopContext.ShopUI.ShopEquipGroup.CancelScrollViewSelect();
            _shopContext.ShopUI.ShopEquipGroup.SetDetailByData(null, null);
            _shopContext.ShopUI._selectedShopData = null;
        }
    }
}
