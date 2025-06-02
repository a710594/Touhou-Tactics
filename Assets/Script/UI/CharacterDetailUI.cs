using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Battle;
using UnityEngine.EventSystems;

public class CharacterDetailUI : MonoBehaviour
{
    public Action WeaponHandler;
    public Action ArmorHandler_1;
    public Action ArmorHandler_2;
    public Action AmuletHandler_1;
    public Action AmuletHandler_2;

    public RectTransform RectTransform;
    public Text NameLabel;
    public Text LvLabel;
    public Text ExpLabel;
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
    public Text ATKLabel;
    public Text DEFLabel;
    public Text MTKLabel;
    public Text MEFLabel;
    public Text PassiveCommentLabel;

    public Image Image;

    public ButtonPlus WeaponButton;
    public ButtonPlus[] ArmorButtons;
    public ButtonPlus[] AmuletButtons;

    public ScrollView SkillScrollView;
    public ScrollView SupportScrollView;
    public EquipDetail EquipDetail;
    public SkillInfoGroup SkillInfoGroup;
    public StatusIconGroup StatusIconGroup;

    public Button CloseButton;
    public Button EquipButton;
    public Button SkillButton;
    public ButtonGroup ButtonGroup;
    public GameObject EquipGroup;
    public GameObject SkillGroup;

    private CharacterInfo _characterInfo = null;
    private BattleCharacterInfo _battleCharacterInfo = null;

    public static CharacterDetailUI Open() 
    {
        GameObject obj = (GameObject)GameObject.Instantiate(Resources.Load("Prefab/UI/CharacterDetailUI"), Vector3.zero, Quaternion.identity);
        GameObject canvas = GameObject.Find("Canvas");
        obj.transform.SetParent(canvas.transform);
        CharacterDetailUI characterDetailUI = obj.GetComponent<CharacterDetailUI>();
        characterDetailUI.RectTransform.offsetMax = Vector3.zero;
        characterDetailUI.RectTransform.offsetMin = Vector3.zero;

        return characterDetailUI;
    }

    public void SetData(CharacterInfo character)
    {
        _characterInfo = character;

        ButtonGroup.SetSelect(EquipButton.gameObject);

        NameLabel.text = character.Name;
        LvLabel.text = "Lv." + CharacterManager.Instance.Info.Lv;
        ExpLabel.text = "經驗值：" + CharacterManager.Instance.Info.Exp + "/" + CharacterManager.Instance.NeedExp(CharacterManager.Instance.Info.Lv);
        Sprite sprite = Resources.Load<Sprite>("Image/Character/" + character.Name + "(裁切去背");
        Image.sprite = sprite;
        Image.transform.GetChild(0).gameObject.SetActive(sprite == null);
        if (character.PassiveList[0] != null)
        {
            PassiveLabel.text = "被動技能：" + character.PassiveList[0].Data.Name;
            PassiveCommentLabel.text = character.PassiveList[0].Data.Comment;
        }
        else
        {
            PassiveLabel.text = string.Empty;
            PassiveCommentLabel.text = string.Empty;
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
        ATKLabel.text = "物理攻擊" + character.Weapon.ATK;
        MTKLabel.text = "魔法攻擊" + character.Weapon.MTK;
        int def = 0;
        int mef = 0;
        for (int i=0; i<character.Armor.Count; i++) 
        {
            def += character.Armor[i].DEF;
            mef += character.Armor[i].MEF;
        }
        DEFLabel.text = "物理防禦" + def;
        MEFLabel.text = "魔法防禦" + mef;

        WeaponButton.Label.text = character.Weapon.Name;
        WeaponButton.SetData(character.Weapon);
        for (int i = 0; i < AmuletButtons.Length; i++)
        {
            if (i < character.Armor.Count)
            {
                ArmorButtons[i].Label.text = character.Armor[i].Name;
                ArmorButtons[i].SetData(character.Armor[i]);
                ArmorButtons[i].gameObject.SetActive(true);
            }
            else
            {
                ArmorButtons[i].gameObject.SetActive(false);
            }
        }

        for (int i = 0; i < AmuletButtons.Length; i++)
        {
            if (i < character.Decoration.Count)
            {
                AmuletButtons[i].Label.text = character.Decoration[i].Name;
                AmuletButtons[i].SetData(character.Decoration[i]);
                AmuletButtons[i].gameObject.SetActive(true);
            }
            else
            {
                AmuletButtons[i].gameObject.SetActive(false);
            }
        }
    }

    private void WeaponOnClick(PointerEventData eventData, ButtonPlus buttonPlus)
    {
        BagUI bagUI = BagUI.Open(null);
        bagUI.SetEquipState(EquipModel.CategoryEnum.Weapon, _characterInfo, 0);
        bagUI.SetEquipHandler += SetWeapon;

        if (WeaponHandler != null)
        {
            WeaponHandler();
        }
    }

    private void ArmornOnClick_1(PointerEventData eventData, ButtonPlus buttonPlus)
    {
        BagUI bagUI = BagUI.Open(null);
        bagUI.SetEquipState(EquipModel.CategoryEnum.Armor, _characterInfo, 0);
        bagUI.SetEquipHandler += SetArmor;

        if (ArmorHandler_1 != null)
        {
            ArmorHandler_1();
        }
    }

    private void ArmornOnClick_2(PointerEventData eventData, ButtonPlus buttonPlus)
    {
        BagUI bagUI = BagUI.Open(null);
        bagUI.SetEquipState(EquipModel.CategoryEnum.Armor, _characterInfo, 1);
        bagUI.SetEquipHandler += SetArmor;

        if (ArmorHandler_2 != null)
        {
            ArmorHandler_2();
        }
    }

    private void AmuletOnClick_1(PointerEventData eventData, ButtonPlus buttonPlus)
    {
        BagUI bagUI = BagUI.Open(null);
        bagUI.SetEquipState(EquipModel.CategoryEnum.Amulet, _characterInfo, 0);
        bagUI.SetEquipHandler += SetAmulet;

        if (AmuletHandler_1 != null)
        {
            AmuletHandler_1();
        }
    }

    private void AmuletOnClick_2(PointerEventData eventData, ButtonPlus buttonPlus)
    {
        BagUI bagUI = BagUI.Open(null);
        bagUI.SetEquipState(EquipModel.CategoryEnum.Amulet, _characterInfo, 1);
        bagUI.SetEquipHandler += SetAmulet;

        if (AmuletHandler_2 != null)
        {
            AmuletHandler_2();
        }
    }

    private void SetWeapon(int index, object obj) 
    {
        if (_characterInfo != null)
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
    }

    private void SetArmor(int index, object obj)
    {
        if (_characterInfo != null)
        {
            //如果角色身上有裝備,就把裝備放回背包
            if (_characterInfo.Armor != null && _characterInfo.Armor[index].ID != 0)
            {
                ItemManager.Instance.AddEquip(_characterInfo.Armor[index]);
            }

            Equip equip = (Equip)obj;
            _characterInfo.Armor[index] = equip;
            SetData(_characterInfo);
        }
    }

    private void SetAmulet(int index, object obj)
    {
        if (_characterInfo != null)
        {
            //如果角色身上有裝備,就把裝備放回背包
            if (_characterInfo.Decoration[index] != null && _characterInfo.Decoration[index].ID != 0)
            {
                ItemManager.Instance.AddEquip(_characterInfo.Decoration[index]);
            }

            Equip equip = (Equip)obj;
            _characterInfo.Decoration[index] = equip;
            SetData(_characterInfo);
        }
    }

    private void ShowEquipDetail(ButtonPlus button) 
    {
        Equip equip = (Equip)button.Data;
        if (equip.ID != 0)
        {
            EquipDetail.SetData(equip);
            EquipDetail.gameObject.SetActive(true);
        }
    }

    private void HideEquipDetail(ButtonPlus button)
    {
        EquipDetail.gameObject.SetActive(false);
    }

    private void ShowSkillInfo(ButtonPlus buttonPlus) 
    {
        Skill skill = (Skill)buttonPlus.Data;
        SkillInfoGroup.SetData(skill);
        SkillInfoGroup.transform.localPosition = new Vector3(0, -540, 0);
        SkillInfoGroup.gameObject.SetActive(true);
    }

    private void HideSkillInfo(ButtonPlus buttonPlus)
    {
        SkillInfoGroup.gameObject.SetActive(false);
    }

    private void ShowSupportInfo(ButtonPlus buttonPlus)
    {
        Sub sub = (Sub)buttonPlus.Data;
        SkillInfoGroup.SetData(sub);
        SkillInfoGroup.transform.localPosition = new Vector3(0, -70, 0);
        SkillInfoGroup.gameObject.SetActive(true);
    }

    private void HideSupportInfo(ButtonPlus buttonPlus)
    {
        SkillInfoGroup.gameObject.SetActive(false);
    }

    private void EquipOnClick() 
    {
        EquipGroup.SetActive(true);
        SkillGroup.SetActive(false);
    }

    private void SkillOnClick()
    {
        EquipGroup.SetActive(false);
        SkillGroup.SetActive(true);

        if (_characterInfo.SkillList.Count > 0)
        {
            List<object> list = new List<object>(_characterInfo.SkillList);
            SkillScrollView.SetData(list);
            SkillScrollView.gameObject.SetActive(true);
        }
        else
        {
            SkillScrollView.gameObject.SetActive(false);
        }

        if (_characterInfo.SupportList.Count > 0)
        {
            List<object> list = new List<object>(_characterInfo.SupportList);
            SupportScrollView.SetData(list);
            SupportScrollView.gameObject.SetActive(true);
        }
        else
        {
            SupportScrollView.gameObject.SetActive(false);
        }
    }

    public void Close() 
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
        ArmorButtons[0].ClickHandler += ArmornOnClick_1;
        ArmorButtons[0].EnterHandler += ShowEquipDetail;
        ArmorButtons[0].ExitHandler += HideEquipDetail;
        ArmorButtons[1].ClickHandler += ArmornOnClick_2;
        ArmorButtons[1].EnterHandler += ShowEquipDetail;
        ArmorButtons[1].ExitHandler += HideEquipDetail;
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
        EquipButton.onClick.AddListener(EquipOnClick);
        SkillButton.onClick.AddListener(SkillOnClick);

        EquipGroup.SetActive(true);
        SkillGroup.SetActive(false);
    }
}
