using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BagEquipGroup : MonoBehaviour
{
    public Action<object> ScrollHandler;

    public ScrollView ScrollView;
    public ButtonPlusGroup ButtonGroup;
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

    public void SetScrollView()
    {
        ScrollView.SetData(new List<object>(ItemManager.Instance.Info.EquipList));

        ButtonGroup.Clear();
        for (int i = 0; i < ScrollView.GridList.Count; i++)
        {
            for (int j = 0; j < ScrollView.GridList[i].ScrollItemList.Count; j++)
            {
                ButtonGroup.Add(ScrollView.GridList[i].ScrollItemList[j].GetComponent<ButtonPlusSingle>());
            }
        }
        ButtonGroup.CancelAllSelect();
    }

    public void SetScrollView(EquipModel.CategoryEnum category, CharacterInfo character) 
    {
        List<Equip> list = new List<Equip>();
        for (int i=0; i<ItemManager.Instance.Info.EquipList.Count; i++) 
        {
            if(ItemManager.Instance.Info.EquipList[i].Category == category) 
            {
                list.Add(ItemManager.Instance.Info.EquipList[i]);
            }
        }
        list.Insert(0, new Equip(category));
        ScrollView.SetData(new List<object>(list));

        ButtonGroup.Clear();
        for (int i = 0; i < ScrollView.GridList.Count; i++)
        {
            for (int j = 0; j < ScrollView.GridList[i].ScrollItemList.Count; j++)
            {
                ButtonGroup.Add(ScrollView.GridList[i].ScrollItemList[j].GetComponent<ButtonPlusSingle>());
            }
        }
        ButtonGroup.CancelAllSelect();
    }

    public void SetDetail(Equip equip) 
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

    private void ScrollItemOnClick(PointerEventData eventData, ButtonPlus button)
    {
        object data = button.Data;
        ScrollHandler(data);
    }

    private void Awake()
    {
        ScrollView.ClickHandler += ScrollItemOnClick;
    }
}
