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

    public Text WeaponLabel;
    public Text ArmorLabel;
    public Text[] AmuletLabels;

    public ScrollView SkillScrollView;
    public ScrollView SupportScrollView;

    public Button CloseButton;

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

        WeaponLabel.text = character.Weapon.Name;
        ArmorLabel.text = character.Armor.Name;
        for (int i=0; i<AmuletLabels.Length; i++) 
        {
            AmuletLabels[i].text = character.Amulets[i].Name;
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

    private void Close() 
    {
        Destroy(gameObject);
    }

    private void Awake()
    {
        SkillScrollView.Init();
        SupportScrollView.Init();
        CloseButton.onClick.AddListener(Close);
    }
}
