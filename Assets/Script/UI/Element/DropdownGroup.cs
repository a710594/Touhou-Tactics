using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropdownGroup : MonoBehaviour
{
    public GridLayoutGroup GridLayout;
    public RectTransform RectTransform;
    public Text EmptyLabel;

    [NonSerialized]
    public List<DropdownButton> ButtonList = new List<DropdownButton>();

    public void SetData(DropdownRoot root, DropdownButton parent, bool hasUse, List<KeyValuePair<string, object>> list, Type type)
    {
        Clear();

        if (list.Count > 0)
        {
            DropdownButton dropdownButton;
            for (int i = 0; i < list.Count; i++)
            {
                dropdownButton = Instantiate(Resources.Load<DropdownButton>("Prefab/UI/Element/DropdownButton"));
                dropdownButton.transform.SetParent(transform);
                dropdownButton.SetData(list[i].Key, list[i].Value, root, type);
                dropdownButton.SetHasUse(hasUse);
                parent.ChildList.Add(dropdownButton);
                dropdownButton.Parent = parent;
                ButtonList.Add(dropdownButton);
            }
            EmptyLabel.gameObject.SetActive(false);
            RectTransform.sizeDelta = new Vector2(GridLayout.cellSize.x + GridLayout.spacing.x * 2, GridLayout.cellSize.y * list.Count + GridLayout.spacing.y * (list.Count + 1));
        }
        else
        {
            EmptyLabel.gameObject.SetActive(true);
            RectTransform.sizeDelta = new Vector2(GridLayout.cellSize.x + GridLayout.spacing.x * 2, GridLayout.cellSize.y + GridLayout.spacing.y * 2);
        }
    }

    public void Clear() 
    {
        for (int i = 0; i < ButtonList.Count; ++i)
        {
            Destroy(ButtonList[i].gameObject);
        }
        ButtonList.Clear();
    }

    public void SetPosition(Transform parent, GridLayoutGroup gridLayout) 
    {
        transform.position = new Vector3(parent.position.x - (gridLayout.cellSize.x + gridLayout.spacing.x), parent.position.y + gridLayout.cellSize.y / 2f + gridLayout.spacing.y, 0);
    }
}
