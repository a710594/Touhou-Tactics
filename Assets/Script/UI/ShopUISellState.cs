using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class ShopUI : MonoBehaviour
{
    private class SellState : ShopState 
    {
        public SellState(StateContext context) : base(context)
        {

        }

        public override void ConsumablesOnClick()
        {
            _shopContext.ShopUI.ShopItemGroup.gameObject.SetActive(true);
            _shopContext.ShopUI.ShopEquipGroup.gameObject.SetActive(false);
            _shopContext.ShopUI.ShopItemGroup.SetScrollViewSell(ItemModel.CategoryEnum.Consumables);
            _shopContext.ShopUI.ShopItemGroup.CancelScrollViewSelect();
            _shopContext.ShopUI.ShopItemGroup.SetComment("");
            _shopContext.ShopUI._selectedSell = null;
        }

        public override void FoodOnClick()
        {
            _shopContext.ShopUI.ShopItemGroup.gameObject.SetActive(true);
            _shopContext.ShopUI.ShopEquipGroup.gameObject.SetActive(false);
            _shopContext.ShopUI.ShopItemGroup.SetScrollViewSell(ItemModel.CategoryEnum.Food);
            _shopContext.ShopUI.ShopItemGroup.CancelScrollViewSelect();
            _shopContext.ShopUI.ShopItemGroup.SetComment("");
            _shopContext.ShopUI._selectedSell = null;
        }

        public override void ItemOnClick() 
        {
            _shopContext.ShopUI.ShopItemGroup.gameObject.SetActive(true);
            _shopContext.ShopUI.ShopEquipGroup.gameObject.SetActive(false);
            _shopContext.ShopUI.ShopItemGroup.SetScrollViewSell(ItemModel.CategoryEnum.Item);
            _shopContext.ShopUI.ShopItemGroup.CancelScrollViewSelect();
            _shopContext.ShopUI.ShopItemGroup.SetComment("");
            _shopContext.ShopUI._selectedSell = null;
        }

        public override void EquipOnClick()
        {
            _shopContext.ShopUI.ShopItemGroup.gameObject.SetActive(false);
            _shopContext.ShopUI.ShopEquipGroup.gameObject.SetActive(true);
            _shopContext.ShopUI.ShopEquipGroup.SetScrollViewSell();
            _shopContext.ShopUI.ShopEquipGroup.CancelScrollViewSelect();
            _shopContext.ShopUI.ShopEquipGroup.SetDetailByData(null, null);
            _shopContext.ShopUI._selectedSell = null;
        }
    }
}
