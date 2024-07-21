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

    private void ItemScrollItemOnClick(ScrollItem scrollItem)
    {
        BattleController.Instance.SetSelectedCommand((Command)scrollItem.Data);
        BattleController.Instance.SetState<BattleController.TargetState>();
    }

    private void Awake()
    {
        ScrollView.ClickHandler += ItemScrollItemOnClick;
    }
}
