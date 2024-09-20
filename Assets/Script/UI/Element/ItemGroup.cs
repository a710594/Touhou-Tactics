using Battle;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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

    private void ItemScrollItemOnClick(PointerEventData eventData, object data)
    {
        BattleController.Instance.SetSelectedCommand((Command)data);
        BattleController.Instance.SetState<BattleController.TargetState>();
    }

    private void Awake()
    {
        ScrollView.ClickHandler += ItemScrollItemOnClick;
    }
}
