using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ScrollView : MonoBehaviour
{
    public enum TypeEnum
    {
        Horizontal,
        Vertical,
    }

    public Action<ScrollItem> ClickHandler;
    public Action<ScrollItem> DownHandler;
    public Action<ScrollItem> PressHandler;
    public Action<ScrollItem> UpHandler;
    public Action<ScrollItem> EnterHandler;
    public Action<ScrollItem> ExitHandler;

    public TypeEnum Type;
    public float CellSizeX;
    public float CellSizeY;
    public float SpacingX;
    public float SpacingY;
    public float SubSpacing;
    public ScrollRect ScrollRect;
    public RectTransform Background;
    public RectTransform Content;
    public GridLayoutGroup MainGrid;
    public RectTransform MainGridRect;
    public ScrollGrid SubGrid;
    public ScrollItem ScrollItem;

    private bool _isLock = false;
    private int _currentIndex = 0;
    private int _subGridAmount = 0;
    private int _scrollItemAmount = 0;
    private Vector2 _originGridAnchoredPosition;
    private Vector2 _originContentAnchoredPosition = new Vector2();
    private List<ScrollGrid> _gridList = new List<ScrollGrid>();
    private List<object> _dataList = new List<object>();

    public void Init()
    {
        float length = 0;
        if (Type == TypeEnum.Horizontal)
        {
            _subGridAmount = Mathf.FloorToInt(Background.rect.width / CellSizeX) + 1;
            MainGridRect.sizeDelta = new Vector2((CellSizeX + SpacingX) * _subGridAmount, Background.rect.height);
            MainGrid.cellSize = new Vector2(CellSizeX, Background.rect.height);
            MainGrid.spacing = new Vector2(SpacingX, 0);
            length = Background.rect.height;
            Content.sizeDelta = new Vector2(Content.sizeDelta.x, length);
            _scrollItemAmount = Mathf.FloorToInt(length / (CellSizeY + SubGrid.Grid.spacing.y));
        }
        else if (Type == TypeEnum.Vertical)
        {
            _subGridAmount = Mathf.FloorToInt(Background.rect.height / CellSizeY) + 1;
            MainGridRect.sizeDelta = new Vector2(Background.rect.width, (CellSizeY + SpacingY) * _subGridAmount);
            MainGrid.cellSize = new Vector2(Background.rect.width, CellSizeY);
            MainGrid.spacing = new Vector2(0, SpacingY);
            length = Background.rect.width;
            Content.sizeDelta = new Vector2(length, Content.sizeDelta.y);
            _scrollItemAmount = Mathf.FloorToInt(length / (CellSizeX + SubGrid.Grid.spacing.x));
        }


        ScrollGrid grid;
        for (int i = 0; i < _subGridAmount; i++)
        {
            grid = Instantiate(SubGrid);
            grid.transform.SetParent(MainGrid.transform);
            grid.ClickHandler = OnClick;
            grid.DownHandler = OnDown;
            grid.PressHandler = OnPress;
            grid.UpHandler = OnUp;
            grid.EnterHandler = OnEnter;
            grid.ExitHandler = OnExit;
            grid.Init(ScrollItem, Type, length, CellSizeX, CellSizeY, SpacingX, SpacingY, _scrollItemAmount);
            _gridList.Add(grid);
        }
        _originContentAnchoredPosition = Content.anchoredPosition;
    }

    public void SetData(List<object> list) 
    {
        _dataList = list;
        Refresh(0);

        int count = list.Count / _scrollItemAmount;
        if (Type == TypeEnum.Horizontal)
        {
           Content.sizeDelta = new Vector2(count * CellSizeX + count * SpacingX, Content.sizeDelta.y);
           MainGridRect.anchoredPosition = new Vector2((Content.sizeDelta.x - MainGridRect.sizeDelta.x) / 2, 0);
        }
        else if (Type == TypeEnum.Vertical)
        {
           Content.sizeDelta = new Vector2(Content.sizeDelta.x, count * CellSizeY + count * SpacingY);
            MainGridRect.anchoredPosition = new Vector2(0, (Content.sizeDelta.y - MainGridRect.sizeDelta.y) / 2);
        }
        _originGridAnchoredPosition = MainGridRect.anchoredPosition;
    }

    public void CancelSelect() 
    {
        for (int i = 0; i < _gridList.Count; i++)
        {
            _gridList[i].SetSelect(null);
        }
    }

    public void SetIndex(int index)
    {
        StartCoroutine(WaitOneFrame());
        if(_dataList.Count<_subGridAmount - 1)
        {
            return;
        }
        else if(index <= _dataList.Count - _subGridAmount + 1)
        {
            _currentIndex = index;
            if (Type == TypeEnum.Horizontal)
            {
                Content.anchoredPosition = new Vector2(_originContentAnchoredPosition.x + _currentIndex * (CellSizeX + SpacingX), _originContentAnchoredPosition.y);
                MainGridRect.anchoredPosition =new Vector2(_originGridAnchoredPosition.x - _currentIndex * (CellSizeX + SpacingX), _originGridAnchoredPosition.y);
                Refresh(_currentIndex);
            }
            else if (Type == TypeEnum.Vertical)
            {
                Content.anchoredPosition = new Vector2(_originContentAnchoredPosition.x, _originContentAnchoredPosition.y + _currentIndex * (CellSizeY + SpacingY));
                MainGridRect.anchoredPosition =new Vector2(_originGridAnchoredPosition.x, _originGridAnchoredPosition.y - _currentIndex * (CellSizeX + SpacingX));
                Refresh(_currentIndex);
            }
        }
        else
        {
            _currentIndex = _dataList.Count - _subGridAmount + 1;
            if (Type == TypeEnum.Horizontal)
            {
                Content.anchoredPosition = new Vector2(_originContentAnchoredPosition.x + (_currentIndex + 1) * (CellSizeX + SpacingX), _originContentAnchoredPosition.y);
                MainGridRect.anchoredPosition = new Vector2(_originGridAnchoredPosition.x - _currentIndex * (CellSizeX + SpacingX), _originGridAnchoredPosition.y);
                Refresh(_currentIndex);
            }
            else if (Type == TypeEnum.Vertical)
            {
                Content.anchoredPosition = new Vector2(_originContentAnchoredPosition.x, _originContentAnchoredPosition.y + (_currentIndex + 1) * (CellSizeY + SpacingY));
                MainGridRect.anchoredPosition = new Vector2(_originGridAnchoredPosition.x, _originGridAnchoredPosition.y - _currentIndex * (CellSizeY + SpacingY));
                Refresh(_currentIndex);
            }
        }
        ScrollRect.StopMovement();
    }

    private IEnumerator WaitOneFrame()
    {
        _isLock = true;
        yield return new WaitForEndOfFrame();
        _isLock = false;
    }

    private void Refresh(int index) 
    {
        for (int i = 0; i < _gridList.Count; i++)
        {
            _gridList[i].RefreshData(index + i, _dataList);
        }
    }

    private void OnClick(ScrollItem scrollItem)
    {
        for (int i = 0; i < _gridList.Count; i++)
        {
            _gridList[i].SetSelect(scrollItem);
        }

        if (ClickHandler != null) 
        {
            ClickHandler(scrollItem);
        }
    }

    private void OnDown(ScrollItem scrollItem)
    {
        if (DownHandler != null)
        {
            DownHandler(scrollItem);
        }
    }

    private void OnPress(ScrollItem scrollItem)
    {
        if (PressHandler != null)
        {
            PressHandler(scrollItem);
        }
    }

    private void OnUp(ScrollItem scrollItem)
    {
        if (UpHandler != null)
        {
            UpHandler(scrollItem);
        }
    }

    private void OnEnter(ScrollItem scrollItem)
    {
        if (EnterHandler != null)
        {
            EnterHandler(scrollItem);
        }
    }

    private void OnExit(ScrollItem scrollItem)
    {
        if (ExitHandler != null)
        {
            ExitHandler(scrollItem);
        }
    }

    private void Awake()
    {
        if (Type == TypeEnum.Horizontal)
        {
            SubGrid.Grid.spacing = new Vector3(0, SubSpacing);
        }
        else if (Type == TypeEnum.Vertical)
        {
            SubGrid.Grid.spacing = new Vector3(SubSpacing, 0);
        }
        Init();
    }

    private void Update()
    {
        if(_isLock)
        {
            return;
        }

        if (Type == TypeEnum.Horizontal)
        {
            if(Mathf.RoundToInt(Content.anchoredPosition.x - _originContentAnchoredPosition.x) > (CellSizeX + SpacingX) * (_currentIndex + 1))
            {
                MainGridRect.anchoredPosition = new Vector2(MainGridRect.anchoredPosition.x - CellSizeX - SpacingX, MainGridRect.anchoredPosition.y);
                _currentIndex++;
                Refresh(_currentIndex);
            }
            else if(Mathf.RoundToInt(Content.anchoredPosition.x - _originContentAnchoredPosition.x) < (CellSizeX + SpacingX) * _currentIndex)
            {                   
                MainGridRect.anchoredPosition = new Vector2(MainGridRect.anchoredPosition.x + CellSizeX + SpacingX, MainGridRect.anchoredPosition.y);
                _currentIndex--;
                Refresh(_currentIndex);
            }
        }
        else if (Type == TypeEnum.Vertical)
        {
            if(Mathf.RoundToInt(Content.anchoredPosition.y - _originContentAnchoredPosition.y) > (CellSizeY + SpacingY) * (_currentIndex + 1))
            {
                MainGridRect.anchoredPosition = new Vector2(MainGridRect.anchoredPosition.x, MainGridRect.anchoredPosition.y - CellSizeY - SpacingY);
                _currentIndex++;
                Refresh(_currentIndex);
            }
            else if(Mathf.RoundToInt(Content.anchoredPosition.y - _originContentAnchoredPosition.y) < (CellSizeY + SpacingY) * _currentIndex)
            {                   
                MainGridRect.anchoredPosition = new Vector2(MainGridRect.anchoredPosition.x, MainGridRect.anchoredPosition.y + CellSizeY + SpacingY);
                _currentIndex--;
                Refresh(_currentIndex);
            }
        }
    }
}
