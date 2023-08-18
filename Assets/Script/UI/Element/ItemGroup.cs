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
        List<object> list = new List<object>(ItemManager.Instance.GetItemList(character));
        ScrollView.SetData(list);
        PPGroup.SetData(character.CurrentPP);
    }

    private void ItemScrollItemOnClick(object obj, ScrollItem scrollItem)
    {
        BattleController.Instance.SelectItem((Item)obj);
    }

    private void Awake()
    {
        ScrollView.Init();
        ScrollView.ItemOnClickHandler += ItemScrollItemOnClick;
    }
}
