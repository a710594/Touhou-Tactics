using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipIntroduction : MyEvent
{
    private ItemModel _data;
    private TutorialUI _tutorialUI;
    private CharacterUI _characterUI;
    private CharacterDetailUI _characterDetailUI;
    private BagUI _bagUI;
    private CharacterInfo _characterInfo;
    private EquipModel _equipData;

    public void Start(ItemModel data)
    {
        _data = data;
        _tutorialUI = TutorialUI.Open(15, null);
        _tutorialUI.CloseButton.gameObject.SetActive(false);
        InputMamager.Instance.CHandler += Step_1;
    }

    private void Step_1() 
    {
        InputMamager.Instance.CHandler -= Step_1;
        _tutorialUI.Close();
        _characterUI = GameObject.Find("CharacterUI(Clone)").GetComponent<CharacterUI>();
        Cursor.lockState = CursorLockMode.None;
        CharacterScrollItem scrollItem = (CharacterScrollItem)_characterUI.ScrollView.GridList[0].ScrollItemList[0];
        TutorialArrowUI.Open("選擇角色的詳細資料", scrollItem.DetailButton.transform, new Vector3(0, 100, 0), Vector2Int.down);
        _characterUI.DetailHandler += DetailOnClick;
        _characterUI.UseItemHandler += UseItemOnClick;
        _characterUI.CloseButton.enabled = false;
        InputMamager.Instance.CurrentUI = null;
    }

    private void Step_2() 
    {
        _characterUI.DetailHandler = null;
        _characterUI.UseItemHandler = null;
        _characterUI.CloseButton.enabled = true;

        _characterDetailUI = CharacterDetailUI.Open();
        _characterDetailUI.SetData(_characterInfo);
        _characterDetailUI.CloseButton.enabled = false;
        _characterDetailUI.SkillButton.enabled = false;

        Transform transform = null;
        _equipData = DataTable.Instance.EquipDic[_data.ID];
        if (_equipData.Category == EquipModel.CategoryEnum.Weapon)
        {
            transform = _characterDetailUI.WeaponButton.transform;
            _characterDetailUI.WeaponHandler += EquipOnClick;
            _characterDetailUI.ArmorButtons[0].ClickHandler = null;
            _characterDetailUI.ArmorButtons[1].ClickHandler = null;
            _characterDetailUI.AmuletButtons[0].ClickHandler = null;
            _characterDetailUI.AmuletButtons[1].ClickHandler = null;
        }
        else if (_equipData.Category == EquipModel.CategoryEnum.Armor)
        {
            transform = _characterDetailUI.ArmorButtons[0].transform;
            _characterDetailUI.ArmorHandler_1 += EquipOnClick;
            _characterDetailUI.WeaponButton.ClickHandler = null;
            _characterDetailUI.ArmorButtons[1].ClickHandler = null;
            _characterDetailUI.AmuletButtons[0].ClickHandler = null;
            _characterDetailUI.AmuletButtons[1].ClickHandler = null;
        }
        else if (_equipData.Category == EquipModel.CategoryEnum.Amulet)
        {
            transform = _characterDetailUI.AmuletButtons[0].transform;
            _characterDetailUI.AmuletHandler_1 += EquipOnClick;
            _characterDetailUI.WeaponButton.ClickHandler = null;
            _characterDetailUI.ArmorButtons[0].ClickHandler = null;
            _characterDetailUI.ArmorButtons[1].ClickHandler = null;
            _characterDetailUI.AmuletButtons[1].ClickHandler = null;
        }
        TutorialArrowUI.Close();
        TutorialArrowUI.Open("選擇裝備", transform, new Vector3(150, 0, 0), Vector2Int.left);
    }

    private void Step_3() 
    {
        _bagUI = GameObject.Find("BagUI(Clone)").GetComponent<BagUI>();
        _bagUI.CloseButton.enabled = false;
        _bagUI.ScrollItemHandler += EquipScrollItemOnClick;
        BagScrollItem scrollItem = (BagScrollItem)_bagUI.EquipGroup.ScrollView.GridList[1].ScrollItemList[0];
        TutorialArrowUI.Close();
        TutorialArrowUI.Open("選擇", scrollItem.transform, new Vector3(-650, 0, 0), Vector2Int.right);
    }

    private void Step_4() 
    {
        TutorialArrowUI.Close();
        TutorialArrowUI.Open("", _bagUI.UseButton.transform, new Vector3(-150, 0, 0), Vector2Int.right);
        _bagUI.SetEquipHandler += Step_5;
    }

    private void Step_5(int index, object obj) 
    {
        _characterDetailUI.CloseButton.enabled = true;
        _characterDetailUI.SkillButton.enabled = true;
        _characterDetailUI.ResetHandler();
        _characterDetailUI.CloseHandler = () => 
        {
            InputMamager.Instance.CurrentUI = _characterUI;
        };

        TutorialArrowUI.Close();
        TutorialUI.Open(16, null);
    }

    private void DetailOnClick(CharacterScrollItem scrollItem) 
    {
        _characterInfo = (CharacterInfo)scrollItem.Data;
        if (_characterInfo.JobId == 4)
        {
            Step_2();
        }
    }

    private void UseItemOnClick(CharacterScrollItem scrollItem) { }

    private void EquipOnClick() 
    {
        Step_3();
    }

    private bool _hasSelected = false;
    private void EquipScrollItemOnClick(object obj) 
    {
        if (_hasSelected) 
        {
            return;
        }

        Equip equip = (Equip)obj;
        if (equip.ID != 0)
        {
            _bagUI.SetSelectObj(obj);
            _bagUI.EquipGroup.SetDetail(equip);
            Step_4();
            _hasSelected = true;
        }
    }
}
