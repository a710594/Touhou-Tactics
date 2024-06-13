using System;
using System.Collections;
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
    private Vector2 _originGridPosition = new Vector2();
    private Vector2 _originGridLocalPosition = new Vector2();
    private Vector2 _originContentPosition = new Vector2();
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
    }

    public void SetData(List<object> list) 
    {
        _dataList = list;
        Refresh(0);

        int count = list.Count / _scrollItemAmount;
        if (Type == TypeEnum.Horizontal)
        {
            Content.sizeDelta = new Vector2(count * CellSizeX + count * SpacingX, Content.sizeDelta.y);
            MainGrid.transform.localPosition = new Vector2(-MainGridRect.sizeDelta.x / 2f, MainGridRect.sizeDelta.x / 2f);
        }
        else if (Type == TypeEnum.Vertical)
        {
            Content.sizeDelta = new Vector2(Content.sizeDelta.x, count * CellSizeY + count * SpacingY);
            MainGrid.transform.localPosition = new Vector2(MainGridRect.sizeDelta.x / 2f, -MainGridRect.sizeDelta.y / 2f);
        }
        _originGridPosition = MainGrid.transform.position;
        _originGridLocalPosition = MainGrid.transform.localPosition;
        _originContentPosition = Content.transform.position;
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
        WaitOneFrame();
        if(_dataList.Count<_subGridAmount)
        {
            return;
        }
        else if(index <= _dataList.Count - _subGridAmount)
        {
            _currentIndex = index;
            if (Type == TypeEnum.Horizontal)
            {
                Content.transform.position = new Vector2(_originContentPosition.x + _currentIndex * (CellSizeX + SpacingX), _originContentPosition.y);
                MainGrid.transform.localPosition = new Vector2(_originGridLocalPosition.x - _currentIndex * (CellSizeX + SpacingX), _originGridLocalPosition.y);
                Refresh(_currentIndex);
            }
            else if (Type == TypeEnum.Vertical)
            {
                Content.transform.position = new Vector2(_originContentPosition.x, _originContentPosition.y + _currentIndex * (CellSizeY + SpacingY));
                MainGrid.transform.localPosition = new Vector2(_originGridLocalPosition.x, _originGridLocalPosition.y - _currentIndex * (CellSizeY + SpacingY));
                Refresh(_currentIndex);
            }
        }
        else
        {
            _currentIndex = _dataList.Count - _subGridAmount;
            if (Type == TypeEnum.Horizontal)
            {
                Content.transform.position = new Vector2(_originContentPosition.x + (_currentIndex + 1) * (CellSizeX + SpacingX), _originContentPosition.y);
                MainGrid.transform.localPosition = new Vector2(_originGridLocalPosition.x - _currentIndex * (CellSizeX + SpacingX), _originGridLocalPosition.y);
                Refresh(_currentIndex);
            }
            else if (Type == TypeEnum.Vertical)
            {
                Content.transform.position = new Vector2(_originContentPosition.x, _originContentPosition.y + (_currentIndex + 1) * (CellSizeY + SpacingY));
                MainGrid.transform.localPosition = new Vector2(_originGridLocalPosition.x, _originGridLocalPosition.y - _currentIndex * (CellSizeY + SpacingY));
                Refresh(_currentIndex);
            }
        }
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
        MainGrid.transform.localPosition = Vector3.zero;
    }

    private void Update()
    {
        if(_isLock)
        {
            return;
        }

        if (Type == TypeEnum.Horizontal)
        {
            if (MainGrid.transform.position.x - _originGridPosition.x < (CellSizeX + SpacingX))
            {
                MainGrid.transform.localPosition = new Vector2(MainGrid.transform.localPosition.x + CellSizeX + SpacingX, MainGrid.transform.localPosition.y);
                _currentIndex++;
                Refresh(_currentIndex);
            }
            else if (MainGrid.transform.position.x - _originGridPosition.x > -0.1f)
            {
                MainGrid.transform.localPosition = new Vector2(MainGrid.transform.localPosition.x - CellSizeX - SpacingX, MainGrid.transform.localPosition.y);
                _currentIndex--;
                Refresh(_currentIndex);
            }
        }
        else if (Type == TypeEnum.Vertical)
        {
            if (MainGrid.transform.position.y - _originGridPosition.y >  (CellSizeY + SpacingY))
            {
                MainGrid.transform.localPosition = new Vector2(MainGrid.transform.localPosition.x, MainGrid.transform.localPosition.y - CellSizeY - SpacingY);
                _currentIndex++;
                Refresh(_currentIndex);
            }
            else if (MainGrid.transform.position.y - _originGridPosition.y < -0.1f)
            {
                Debug.Log(MainGrid.transform.position.y + " " + _originGridPosition.y);
                MainGrid.transform.localPosition = new Vector2(MainGrid.transform.localPosition.x, MainGrid.transform.localPosition.y + CellSizeY + SpacingY);
                _currentIndex--;
                Refresh(_currentIndex);
            }
        }
    }
}
