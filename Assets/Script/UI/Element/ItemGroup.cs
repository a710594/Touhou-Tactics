using Battle;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGroup : MonoBehaviour
{
    public PointGroup PPGroup;
    public ScrollView ScrollView;

    public void SetScrollView(BattleCharacterInfo character)
    {
        List<object> list = new List<object>(ItemManager.Instance.GetBattleItemList(character));
        ScrollView.SetData(list);
        PPGroup.SetData(character.CurrentPP);
    }

    private void ItemScrollItemOnClick(object obj, ScrollItem scrollItem)
    {
        BattleController.Instance.SelectObject(obj);
    }

    private void Awake()
    {
        ScrollView.ItemOnClickHandler += ItemScrollItemOnClick;
    }
}
