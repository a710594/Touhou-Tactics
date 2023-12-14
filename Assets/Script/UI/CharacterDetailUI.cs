using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterDetailUI : MonoBehaviour
{
    public RectTransform RectTransform;
    public Text NameLabel;
    public Text PassiveLabel;
    public Text HPLabel;
    public Text STRLabel;
    public Text CONLabel; 
    public Text INTLabel; 
    public Text MENLabel;
    public Text DEXLabel; 
    public Text AGILabel;
    public Text MOVLabel;
    public Text WTLabel;

    public ButtonPlus WeaponButton;
    public ButtonPlus ArmorButton;
    public ButtonPlus[] AmuletButtons;

    public ScrollView SkillScrollView;
    public ScrollView SupportScrollView;
    public EquipDetail EquipDetail;
    public SkillInfoGroup SkillInfoGroup;

    public Button CloseButton;

    private CharacterInfo _characterInfo;

    public static CharacterDetailUI Open(bool canSelectEquip) 
    {
        GameObject obj = (GameObject)GameObject.Instantiate(Resources.Load("Prefab/UI/CharacterDetailUI"), Vector3.zero, Quaternion.identity);
        GameObject canvas = GameObject.Find("Canvas");
        obj.transform.SetParent(canvas.transform);
        CharacterDetailUI characterDetailUI = obj.GetComponent<CharacterDetailUI>();
        characterDetailUI.RectTransform.offsetMax = Vector3.zero;
        characterDetailUI.RectTransform.offsetMin = Vector3.zero;
        characterDetailUI.WeaponButton.Button.interactable = canSelectEquip;
        characterDetailUI.ArmorButton.Button.interactable = canSelectEquip;
        characterDetailUI.AmuletButtons[0].Button.interactable = canSelectEquip;
        characterDetailUI.AmuletButtons[1].Button.interactable = canSelectEquip;

        return characterDetailUI;
    }

    public void SetData(BattleCharacterInfo character) 
    {
        NameLabel.text = character.Name;
        if(character.PassiveList.Count>0)
        {
            PassiveLabel.text = character.PassiveList[0].Data.Name;
        }
        else
        {
            PassiveLabel.text = string.Empty;
        }
        HPLabel.text = "HP " + character.CurrentHP + "/" + character.MaxHP;
        STRLabel.text = "力量 " + character.STR;
        CONLabel.text = "體質 " + character.CON;
        INTLabel.text = "智力 " + character.INT;
        MENLabel.text = "精神 " + character.MEN;
        DEXLabel.text = "靈巧 " + character.DEX;
        AGILabel.text = "敏捷 " + character.AGI;
        MOVLabel.text = "移動 " + character.MOV;
        WTLabel.text = "WT " + character.WT;

        WeaponButton.Label.text = character.Weapon.Name;
        ArmorButton.Label.text = character.Armor.Name;
        for (int i=0; i<AmuletButtons.Length; i++) 
        {
            AmuletButtons[i].Label.text = character.Amulets[i].Name;
        }

        if (character.SkillList.Count > 0)
        {
            List<object> list = new List<object>(character.SkillList);
            SkillScrollView.SetData(list);
            SkillScrollView.gameObject.SetActive(true);
        }
        else 
        {
            SkillScrollView.gameObject.SetActive(false);
        }

        if (character.SupportList.Count > 0)
        {
            List<object> list = new List<object>(character.SupportList);
            SupportScrollView.SetData(list);
            SupportScrollView.gameObject.SetActive(true);
        }
        else
        {
            SupportScrollView.gameObject.SetActive(false);
        }
    }

    public void SetData(CharacterInfo character)
    {
        _characterInfo = character;
        NameLabel.text = character.Name;
        if (character.PassiveList.Count > 0)
        {
            PassiveLabel.text = character.PassiveList[0].Data.Name;
        }
        else
        {
            PassiveLabel.text = string.Empty;
        }
        HPLabel.text = "HP " + character.CurrentHP + "/" + character.MaxHP;
        STRLabel.text = "力量 " + character.STR;
        CONLabel.text = "體質 " + character.CON;
        INTLabel.text = "智力 " + character.INT;
        MENLabel.text = "精神 " + character.MEN;
        DEXLabel.text = "靈巧 " + character.DEX;
        AGILabel.text = "敏捷 " + character.AGI;
        MOVLabel.text = "移動 " + character.MOV;
        WTLabel.text = "WT " + character.WT;

        WeaponButton.Label.text = character.Weapon.Name;
        WeaponButton.SetData(character.Weapon);
        ArmorButton.Label.text = character.Armor.Name;
        ArmorButton.SetData(character.Armor);
        for (int i = 0; i < AmuletButtons.Length; i++)
        {
            AmuletButtons[i].Label.text = character.Amulets[i].Name;
            AmuletButtons[i].SetData(character.Amulets[i]);
        }

        if (character.SkillList.Count > 0)
        {
            List<object> list = new List<object>(character.SkillList);
            SkillScrollView.SetData(list);
            SkillScrollView.gameObject.SetActive(true);
        }
        else
        {
            SkillScrollView.gameObject.SetActive(false);
        }

        if (character.SupportList.Count > 0)
        {
            List<object> list = new List<object>(character.SupportList);
            SupportScrollView.SetData(list);
            SupportScrollView.gameObject.SetActive(true);
        }
        else
        {
            SupportScrollView.gameObject.SetActive(false);
        }
    }

    private void WeaponOnClick(ButtonPlus button)
    {
        BagUI bagUI =  BagUI.Open();
        bagUI.SetEquipState(EquipModel.CategoryEnum.Weapon, _characterInfo);
        bagUI.UseHandler += SetWeapon;
    }

    private void ArmornOnClick(ButtonPlus button)
    {
        BagUI bagUI = BagUI.Open();
        bagUI.SetEquipState(EquipModel.CategoryEnum.Armor, _characterInfo);
        bagUI.UseHandler += SetArmor;
    }

    private void AmuletOnClick_1(ButtonPlus button)
    {
        BagUI bagUI = BagUI.Open();
        bagUI.SetEquipState(EquipModel.CategoryEnum.Amulet, _characterInfo);
        bagUI.UseHandler += SetAmulet_1;
    }

    private void AmuletOnClick_2(ButtonPlus button)
    {
        BagUI bagUI = BagUI.Open();
        bagUI.SetEquipState(EquipModel.CategoryEnum.Amulet, _characterInfo);
        bagUI.UseHandler += SetAmulet_2;
    }

    private void SetWeapon(object obj) 
    {
        //如果角色身上有裝備,就把裝備放回背包
        if (_characterInfo.Weapon != null && _characterInfo.Weapon.ID != 0) 
        {
            ItemManager.Instance.AddEquip(_characterInfo.Weapon);
        }

        Equip equip = (Equip)obj;
        _characterInfo.Weapon = equip;
        SetData(_characterInfo);
    }

    private void SetArmor(object obj)
    {
        //如果角色身上有裝備,就把裝備放回背包
        if (_characterInfo.Armor != null && _characterInfo.Armor.ID != 0)
        {
            ItemManager.Instance.AddEquip(_characterInfo.Armor);
        }

        Equip equip = (Equip)obj;
        _characterInfo.Armor = equip;
        SetData(_characterInfo);
    }

    private void SetAmulet_1(object obj)
    {
        //如果角色身上有裝備,就把裝備放回背包
        if (_characterInfo.Amulets[0] != null && _characterInfo.Amulets[0].ID != 0)
        {
            ItemManager.Instance.AddEquip(_characterInfo.Amulets[0]);
        }

        Equip equip = (Equip)obj;
        _characterInfo.Amulets[0] = equip;
        SetData(_characterInfo);
    }

    private void SetAmulet_2(object obj)
    {
        //如果角色身上有裝備,就把裝備放回背包
        if (_characterInfo.Amulets[1] != null && _characterInfo.Amulets[1].ID != 0)
        {
            ItemManager.Instance.AddEquip(_characterInfo.Amulets[1]);
        }

        Equip equip = (Equip)obj;
        _characterInfo.Amulets[1] = equip;
        SetData(_characterInfo);
    }

    private void ShowEquipDetail(ButtonPlus button) 
    {
        Equip equip = (Equip)button.Data;
        EquipDetail.SetData(equip);
        EquipDetail.gameObject.SetActive(true);
    }

    private void HideEquipDetail(ButtonPlus button)
    {
        EquipDetail.gameObject.SetActive(false);
    }

    private void ShowSkillInfo(ScrollItem scrollItem) 
    {
        Skill skill = (Skill)scrollItem.Data;
        SkillInfoGroup.SetData(skill);
        SkillInfoGroup.transform.localPosition = Vector3.zero;
        SkillInfoGroup.gameObject.SetActive(true);
    }

    private void HideSkillInfo(ScrollItem scrollItem)
    {
        SkillInfoGroup.gameObject.SetActive(false);
    }

    private void ShowSupportInfo(ScrollItem scrollItem)
    {
        Support support = (Support)scrollItem.Data;
        SkillInfoGroup.SetData(support);
        SkillInfoGroup.transform.localPosition = new Vector3(0, -540, 0);
        SkillInfoGroup.gameObject.SetActive(true);
    }

    private void HideSupportInfo(ScrollItem scrollItem)
    {
        SkillInfoGroup.gameObject.SetActive(false);
    }

    private void Close() 
    {
        Destroy(gameObject);
    }

    private void Awake()
    {
        EquipDetail.gameObject.SetActive(false);
        SkillInfoGroup.gameObject.SetActive(false);
        WeaponButton.ClickHandler += WeaponOnClick;
        WeaponButton.EnterHandler += ShowEquipDetail;
        WeaponButton.ExitHandler += HideEquipDetail;
        ArmorButton.ClickHandler += ArmornOnClick;
        ArmorButton.EnterHandler += ShowEquipDetail;
        ArmorButton.ExitHandler += HideEquipDetail;
        AmuletButtons[0].ClickHandler += AmuletOnClick_1;
        AmuletButtons[0].EnterHandler += ShowEquipDetail;
        AmuletButtons[0].ExitHandler += HideEquipDetail;
        AmuletButtons[1].ClickHandler += AmuletOnClick_2;
        AmuletButtons[1].EnterHandler += ShowEquipDetail;
        AmuletButtons[1].ExitHandler += HideEquipDetail;
        SkillScrollView.EnterHandler += ShowSkillInfo;
        SkillScrollView.ExitHandler += HideSkillInfo;
        SupportScrollView.EnterHandler += ShowSupportInfo;
        SupportScrollView.ExitHandler += HideSupportInfo;
        CloseButton.onClick.AddListener(Close);
    }
}
