using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollGrid : MonoBehaviour
{
    public Image Background;
    public GridLayoutGroup Grid;
    public ScrollItem ScrollItem;

    [NonReorderable]
    public int ItemAmount;

    private List<ScrollItem> _itemList = new List<ScrollItem>();

    public void Init(ScrollView.TypeEnum type, float length, float cellSizeX, float cellSizeY, float spacingX, float spacingY, int itemAmount)
    {
        Grid.cellSize = new Vector2(cellSizeX, cellSizeY);
        Grid.spacing = new Vector2(spacingX, spacingY);

        if (type == ScrollView.TypeEnum.Horizontal) 
        {
            ItemAmount = itemAmount;
            Background.rectTransform.sizeDelta = new Vector2(ScrollItem.Background.rectTransform.sizeDelta.x, length);
        }
        else if(type == ScrollView.TypeEnum.Vertical)
        {
            ItemAmount = itemAmount;
            Background.rectTransform.sizeDelta = new Vector2(length, ScrollItem.Background.rectTransform.sizeDelta.y);
        }

        ScrollItem item;
        for (int i = 0; i < ItemAmount; i++)
        {
            item = Instantiate(ScrollItem);
            item.transform.SetParent(transform);
            _itemList.Add(item);
        }
    }

    public void RefreshData(int index, List<object> list)
    {
        for (int i = 0; i < _itemList.Count; i++)
        {
            if (index * ItemAmount + i < list.Count)
            {
                _itemList[i].gameObject.SetActive(true);
                _itemList[i].SetData(list[index * ItemAmount + i]);
            }
            else
            {
                _itemList[i].gameObject.SetActive(false);
            }
        }
    }
}
