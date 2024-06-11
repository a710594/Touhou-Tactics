using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public partial class ShopUI : MonoBehaviour
{
    public Button ConsumablesButton;
    public Button FoodButton;
    public Button EquipButton;
    public Button ItemButton;
    public Button BuyButton;
    public Button SellButton;
    public Button BuyModeButton;
    public Button SellModeButton;
    public Button CloseButton;
    public Text MoneyLabel;
    public ShopItemGroup ShopItemGroup;
    public ShopEquipGroup ShopEquipGroup;
    public TipLabel TipLabel;
    public ButtonGroup ModeGroup;
    public ButtonGroup CategoryGroup;

    private ShopModel _selectedShopData = null;
    private object _selectedSell = null;
    private ShopContext _context = new ShopContext();

    public void Open()
    {
        gameObject.SetActive(true);
        _selectedShopData = null;
        _selectedSell = null;
        ConsumablesButton.gameObject.SetActive(true);
        FoodButton.gameObject.SetActive(false);
        EquipButton.gameObject.SetActive(true);
        ItemButton.gameObject.SetActive(false);
        ShopItemGroup.gameObject.SetActive(true);
        ShopEquipGroup.gameObject.SetActive(false);
        ShopItemGroup.SetScrollViewBuy(ItemModel.CategoryEnum.Consumables);
        ShopItemGroup.CancelScrollViewSelect();
        MoneyLabel.text = ItemManager.Instance.BagInfo.Money + "$";
        ModeGroup.SetSelect(BuyModeButton.gameObject);
        CategoryGroup.SetSelect(ConsumablesButton.gameObject);
    }

    private void ConsumablesOnClick()
    {
        ((ShopState)_context.CurrentState).ConsumablesOnClick();
    }

    private void FoodOnClick()
    {
        ((ShopState)_context.CurrentState).FoodOnClick();
    }

    private void EquipOnClick()
    {
        ((ShopState)_context.CurrentState).EquipOnClick();
    }

    private void ItemOnClick()
    {
        ((ShopState)_context.CurrentState).ItemOnClick();
    }

    private void BuyOnClick() 
    {
        if(_selectedShopData==null)
        {
            TipLabel.SetLabel("�|����ܪ��~");
        }
        else if(_selectedShopData.Price > ItemManager.Instance.BagInfo.Money)
        {
            TipLabel.SetLabel("�l�B����");
        }
        else
        {
            if (_selectedShopData.MaterialAmountList.Count == 0)
            {
                ItemManager.Instance.AddItem(_selectedShopData.ID, 1);
                ItemModel item = DataContext.Instance.ItemDic[_selectedShopData.ID];
                ShopItemGroup.SetScrollViewBuy(item.Category);
                ItemManager.Instance.BagInfo.Money -= _selectedShopData.Price;
                MoneyLabel.text = ItemManager.Instance.BagInfo.Money + "$";
            }
            else 
            {
                bool canBuy = true;
                for (int i=0; i<_selectedShopData.MaterialIDList.Count; i++) 
                {
                    if (ItemManager.Instance.GetAmount(_selectedShopData.MaterialIDList[i]) <  _selectedShopData.MaterialAmountList[i]) 
                    {
                        canBuy = false;
                        break;
                    }
                }
                if (canBuy)
                {
                    for (int i = 0; i < _selectedShopData.MaterialIDList.Count; i++)
                    {
                        ItemManager.Instance.MinusItem(_selectedShopData.MaterialIDList[i], _selectedShopData.MaterialAmountList[i]);
                    }
                    ItemManager.Instance.BagInfo.Money -= _selectedShopData.Price;
                    MoneyLabel.text = ItemManager.Instance.BagInfo.Money + "$";

                    ItemModel itemData = DataContext.Instance.ItemDic[_selectedShopData.ID];
                    if (itemData.Category == ItemModel.CategoryEnum.Equip)
                    {
                        ItemManager.Instance.AddEquip(_selectedShopData.ID);
                        ShopEquipGroup.SetScrollViewBuy();
                        ShopEquipGroup.SetMaterial(_selectedShopData);
                    }
                    else
                    {
                        ItemManager.Instance.AddItem(itemData.ID, 1);
                        ShopItemGroup.SetScrollViewBuy(itemData.Category);
                        ShopItemGroup.SetMaterial(_selectedShopData);
                    }
                }
                else
                {
                    TipLabel.SetLabel("���Ƥ���");
                }
            }
        }
    }

    private void SellOnClick() 
    {
        if (_selectedSell != null) 
        {
            if(_selectedSell is Consumables) 
            {
                Consumables consumables = (Consumables)_selectedSell;
                ItemManager.Instance.BagInfo.Money += consumables.Price;
                MoneyLabel.text = ItemManager.Instance.BagInfo.Money + "$";
                ItemManager.Instance.MinusItem(consumables.ID, 1);
                ShopItemGroup.SetScrollViewSell(consumables.Category);
                if (consumables.Amount == 0)
                {
                    _selectedSell = null;
                    ShopItemGroup.CancelScrollViewSelect();
                }
            }
            else if (_selectedSell is Item)
            {
                Item item = (Item)_selectedSell;
                ItemManager.Instance.BagInfo.Money += item.Data.Price;
                MoneyLabel.text = ItemManager.Instance.BagInfo.Money + "$";
                ItemManager.Instance.MinusItem(item.ID, 1);
                ShopItemGroup.SetScrollViewSell(item.Data.Category);
                if (item.Amount == 0)
                {
                    _selectedSell = null;
                    ShopItemGroup.CancelScrollViewSelect();
                }
            }
            else if(_selectedSell is Food) 
            {
                Food food = (Food)_selectedSell;
                ItemManager.Instance.BagInfo.Money += food.Price;
                MoneyLabel.text = ItemManager.Instance.BagInfo.Money + "$";
                ItemManager.Instance.MinusFood(food);
                ShopItemGroup.SetScrollViewSell(ItemModel.CategoryEnum.Food);
                ShopItemGroup.CancelScrollViewSelect();
                _selectedSell = null;
            }
            else if(_selectedSell is Equip) 
            {
                Equip equip = (Equip)_selectedSell;
                ItemManager.Instance.BagInfo.Money += equip.Price;
                MoneyLabel.text = ItemManager.Instance.BagInfo.Money + "$";
                ItemManager.Instance.MinusEquip(equip);
                ShopEquipGroup.SetScrollViewSell();
                ShopEquipGroup.CancelScrollViewSelect();
                _selectedSell = null;
            }
        }
        else
        {
            TipLabel.SetLabel("�|����ܪ��~");
        }
    }

    private void BuyModeOnClick() 
    {
        _context.SetState<BuyState>();
        ((ShopState)_context.CurrentState).ConsumablesOnClick();
        CategoryGroup.SetSelect(ConsumablesButton.gameObject);
        FoodButton.gameObject.SetActive(false);
        ItemButton.gameObject.SetActive(false);
        BuyButton.gameObject.SetActive(true);
        SellButton.gameObject.SetActive(false);
    }

    private void SellModeOnClick()
    {
        _context.SetState<SellState>();
        ((ShopState)_context.CurrentState).ConsumablesOnClick();
        CategoryGroup.SetSelect(ConsumablesButton.gameObject);
        FoodButton.gameObject.SetActive(true);
        ItemButton.gameObject.SetActive(true);
        BuyButton.gameObject.SetActive(false);
        SellButton.gameObject.SetActive(true);
    }

    private void CloseOnClick()
    {
        gameObject.SetActive(false);
    }

    private void SetSelectedShopData(ShopModel data) 
    {
        _selectedShopData = data;
    }

    private void SetSelectedSell(object obj)
    {
        _selectedSell = obj;
    }

    public class ShopContext : StateContext
    {
        public ShopUI ShopUI;

        public void Init(ShopUI shopUI)
        {
            ShopUI = shopUI;
        }
    }

    private class ShopState : State
    {
        protected ShopContext _shopContext;

        public ShopState(StateContext context) : base(context)
        {
            _shopContext = (ShopContext)context;
        }

        public virtual void ConsumablesOnClick() { }

        public virtual void FoodOnClick() { }

        public virtual void ItemOnClick() { }

        public virtual void EquipOnClick() { }
    }

    private void Update()
    {
    }

    private void Awake()
    {
        _context.Init(this);
        _context.AddState(new BuyState(_context));
        _context.AddState(new SellState(_context));
        _context.SetState<BuyState>();

        ConsumablesButton.onClick.AddListener(ConsumablesOnClick);
        FoodButton.onClick.AddListener(FoodOnClick);
        ItemButton.onClick.AddListener(ItemOnClick);
        EquipButton.onClick.AddListener(EquipOnClick);
        BuyButton.onClick.AddListener(BuyOnClick);
        SellButton.onClick.AddListener(SellOnClick);
        BuyModeButton.onClick.AddListener(BuyModeOnClick);
        SellModeButton.onClick.AddListener(SellModeOnClick);
        CloseButton.onClick.AddListener(CloseOnClick);

        ShopItemGroup.ShopDataHandler += SetSelectedShopData;
        ShopItemGroup.SellHandler += SetSelectedSell;
        ShopEquipGroup.ShopDataHandler += SetSelectedShopData;
        ShopEquipGroup.EquipHandler += SetSelectedSell;
    }
}
