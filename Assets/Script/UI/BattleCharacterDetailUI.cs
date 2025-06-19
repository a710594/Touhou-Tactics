using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Battle;
using UnityEngine.EventSystems;

public class BattleCharacterDetailUI : MonoBehaviour
{
    public Action CloseHandler;

    public RectTransform RectTransform;
    public Text NameLabel;
    public Text LvLabel;
    public Text PassiveLabel;
    public Text PassiveCommentLabel;
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
    public Image Image;
    public ScrollView StatusScrollView;
    public Button CloseButton;

    private CharacterInfo _characterInfo = null;
    private BattleCharacterInfo _battleCharacterInfo = null;

    public static BattleCharacterDetailUI Open(BattleCharacterInfo character, Vector2Int position) 
    {
        GameObject obj = (GameObject)GameObject.Instantiate(Resources.Load("Prefab/UI/BattleCharacterDetailUI"), Vector3.zero, Quaternion.identity);
        GameObject canvas = GameObject.Find("Canvas");
        obj.transform.SetParent(canvas.transform);
        BattleCharacterDetailUI battleCharacterDetailUI = obj.GetComponent<BattleCharacterDetailUI>();
        battleCharacterDetailUI.RectTransform.offsetMax = Vector2.zero;
        battleCharacterDetailUI.RectTransform.offsetMin = Vector3.zero;
        battleCharacterDetailUI.SetData(character, position);

        return battleCharacterDetailUI;
    }

    public void SetData(BattleCharacterInfo character, Vector2Int position) 
    {
        _battleCharacterInfo = character;
        NameLabel.text = character.Name;
        LvLabel.text = "Lv. " + character.Lv;
        if (character is BattlePlayerInfo)
        {
            Sprite sprite = Resources.Load<Sprite>("Image/Character/" + ((BattlePlayerInfo)character).Job.Controller + "_Head");
            Image.sprite = sprite;
            Image.transform.GetChild(0).gameObject.SetActive(sprite == null);
        }
        else if(character is BattleEnemyInfo) 
        {
            Sprite sprite = Resources.Load<Sprite>("Image/Character/" + ((BattleEnemyInfo)character).Enemy.Controller + "_Head");
            Image.sprite = sprite;
            Image.transform.GetChild(0).gameObject.SetActive(sprite == null);
        }

        if (character.PassiveList.Count>0)
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
        for (int i = 0; i < character.Armor.Count; i++)
        {
            def += character.Armor[i].DEF;
            mef += character.Armor[i].MEF;
        }
        DEFLabel.text = "物理防禦" + def;
        MEFLabel.text = "魔法防禦" + mef;

        List<Status> statusList = BattleController.Instance.GetStatueList(character, StatusModel.TypeEnum.None, position);
        if (statusList.Count > 0)
        {
            StatusScrollView.SetData(new List<object>(statusList));
            StatusScrollView.gameObject.SetActive(true);
        }
        else 
        {
            StatusScrollView.gameObject.SetActive(false);
        }
    }

    private void Close() 
    {
        if (CloseHandler != null) 
        {
            CloseHandler();
        }

        Destroy(gameObject);
    }

    private void Awake()
    {
        CloseButton.onClick.AddListener(Close);
    }
}
