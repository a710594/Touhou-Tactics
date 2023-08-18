using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollView : MonoBehaviour
{
    public enum TypeEnum
    {
        Horizontal,
        Vertical,
    }

    public Action<object, ScrollItem> ItemOnClickHandler;

    public TypeEnum Type;
    public float CellSizeX;
    public float CellSizeY;
    public float SpacingX;
    public float SpacingY;
    public RectTransform Background;
    public RectTransform Content;
    public GridLayoutGroup MainGrid;
    public RectTransform MainGridRect;
    public ScrollGrid SubGrid;
    public ScrollItem ScrollItem;

    private int _currentIndex = 0;
    private int _subGridAmount = 0;
    private int _scrollItemAmount = 0;
    private Vector2 _gridOrign = new Vector2();
    private List<ScrollGrid> _gridList = new List<ScrollGrid>();
    private List<object> _dataList = new List<object>();

    public void Init()
    {
        float length = 0;
        if (Type == TypeEnum.Horizontal)
        {
            _subGridAmount = Mathf.FloorToInt(Background.sizeDelta.x / ScrollItem.Background.rectTransform.sizeDelta.x) + 1;
            MainGridRect.sizeDelta = new Vector2((ScrollItem.Background.rectTransform.sizeDelta.x + SpacingX) * _subGridAmount, Background.sizeDelta.y);
            MainGrid.cellSize = new Vector2(CellSizeX, Background.sizeDelta.y);
            MainGrid.spacing = new Vector2(SpacingX, 0);
            length = Content.sizeDelta.y;
            _scrollItemAmount = Mathf.FloorToInt(length / (ScrollItem.Background.rectTransform.sizeDelta.y + SubGrid.Grid.spacing.y));
        }
        else if (Type == TypeEnum.Vertical)
        {
            _subGridAmount = Mathf.FloorToInt(Background.sizeDelta.y / ScrollItem.Background.rectTransform.sizeDelta.y) + 1;
            MainGridRect.sizeDelta = new Vector2(Background.sizeDelta.x, (ScrollItem.Background.rectTransform.sizeDelta.y + SpacingY) * _subGridAmount);
            MainGrid.cellSize = new Vector2(Background.sizeDelta.x, CellSizeY);
            MainGrid.spacing = new Vector2(0, SpacingY);
            length = Content.sizeDelta.x;
            _scrollItemAmount = Mathf.FloorToInt(length / (ScrollItem.Background.rectTransform.sizeDelta.x + SubGrid.Grid.spacing.x));
        }


        ScrollGrid grid;
        for (int i = 0; i < _subGridAmount; i++)
        {
            grid = Instantiate(SubGrid);
            grid.transform.SetParent(MainGrid.transform);
            //if (Type == TypeEnum.Horizontal)
            //{
            //    grid.transform.localPosition = new Vector3((i + 0.5f) * CellSizeY + i * SpacingY, Background.sizeDelta.y / 2f);
            //}
            //else if (Type == TypeEnum.Vertical)
            //{
            //    grid.transform.localPosition = new Vector3(Background.sizeDelta.x / 2f, -(i + 0.5f) * CellSizeY - i * SpacingY);
            //}
            grid.ItemOnClickHandler += ItemOnClick;
            grid.Init(ScrollItem, Type, length, CellSizeX, CellSizeY, SpacingX, SpacingY, _scrollItemAmount);
            _gridList.Add(grid);
        }
    }

    public void SetData(List<object> list) 
    {
        _dataList = list;
        Refresh(0);

        int count = list.Count / _scrollItemAmount;
        if (Type == TypeEnum.Horizontal)
        {
            Content.sizeDelta = new Vector2((count + 1) * CellSizeX + count * SpacingX, Content.sizeDelta.y);
            MainGrid.transform.localPosition = new Vector2(-MainGridRect.sizeDelta.x / 2f, MainGridRect.sizeDelta.x / 2f);
        }
        else if (Type == TypeEnum.Vertical)
        {
            Content.sizeDelta = new Vector2(Content.sizeDelta.x, (count + 1) * CellSizeY + count * SpacingY);
            MainGrid.transform.localPosition = new Vector2(MainGridRect.sizeDelta.x / 2f, -MainGridRect.sizeDelta.y / 2f);
        }
        _gridOrign = MainGrid.transform.localPosition;
    }

    private void Refresh(int index) 
    {
        for (int i = 0; i < _gridList.Count; i++)
        {
            _gridList[i].RefreshData(index + i, _dataList);
        }
    }

    private void ItemOnClick(object obj, ScrollItem item) 
    {
        if (ItemOnClickHandler != null) 
        {
            ItemOnClickHandler(obj, item);
        }
    }

    private void Awake()
    {
        //Init();

        //List<object> list = new List<object>();
        //for (int i=1; i<= 50; i++) 
        //{
        //    list.Add(i.ToString());
        //}
        //SetData(list);
        MainGrid.transform.localPosition = Vector3.zero;
    }

    private void Update()
    {
        if (Type == TypeEnum.Horizontal)
        {
            if (MainGrid.transform.localPosition.x - _gridOrign.x < (CellSizeY + SpacingY))
            {
                MainGrid.transform.localPosition = new Vector2(MainGrid.transform.localPosition.y + CellSizeY + SpacingY, MainGrid.transform.localPosition.y);
                _currentIndex++;
                Refresh(_currentIndex);
            }
            else if (MainGrid.transform.localPosition.y - _gridOrign.y > -0.1f)
            {
                MainGrid.transform.localPosition = new Vector2(MainGrid.transform.localPosition.y - CellSizeY - SpacingY, MainGrid.transform.localPosition.x);
                _currentIndex--;
                Refresh(_currentIndex);
            }
        }
        else if (Type == TypeEnum.Vertical)
        {
            if (MainGrid.transform.localPosition.y - _gridOrign.y >  (CellSizeY + SpacingY))
            {
                MainGrid.transform.localPosition = new Vector2(MainGrid.transform.localPosition.x, MainGrid.transform.localPosition.y - CellSizeY - SpacingY);
                _currentIndex++;
                Refresh(_currentIndex);
            }
            else if (MainGrid.transform.localPosition.y - _gridOrign.y < -0.1f)
            {
                MainGrid.transform.localPosition = new Vector2(MainGrid.transform.localPosition.x, MainGrid.transform.localPosition.y + CellSizeY + SpacingY);
                _currentIndex--;
                Refresh(_currentIndex);
            }
        }
        //Debug.Log(Content.transform.localPosition.y);
    }
}
