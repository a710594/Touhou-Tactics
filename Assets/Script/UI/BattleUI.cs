using Battle;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BattleUI : MonoBehaviour
{
    public Action<Vector2Int> DirectionButtonHandler;

    public static BattleUI Instance = null;

    public CommandGroup CommandGroup;
    public LittleHpBarWithStatus LittleHpBarWithStatus;
    public Transform HPGroup;
    public NewFloatingNumberPool FloatingNumberPool;
    public CharacterListGroup CharacterListGroup;
    public DirectionGroup DirectionGroup;
    public LogGroup LogGroup;
    public ArrowImage Arrow;
    public Text TileLabel;

    private Vector3 _directionPosition = new Vector3();    
    private Dictionary<BattleCharacterController, NewFloatingNumberPool> _floatingNumberPoolDic  = new Dictionary<BattleCharacterController, NewFloatingNumberPool>();

    public void SetVisible(bool isVisible) 
    {
        gameObject.SetActive(isVisible);
    }

    public void SetCommandVisible(bool isVisible)
    {
        CommandGroup.gameObject.SetActive(isVisible);
        LogGroup.OpenButton.gameObject.SetActive(isVisible);
    }

    public void SetFloatingNumberPoolAnchor(BattleCharacterController controller)
    {
        NewFloatingNumberPool floatingNumberPool = Instantiate(FloatingNumberPool);
        floatingNumberPool.transform.SetParent(transform);
        floatingNumberPool.SetAnchor(controller.transform);
        _floatingNumberPoolDic.Add(controller, floatingNumberPool);
    }

    public void PlayFloatingNumberPool(BattleCharacterController info, List<FloatingNumberData> list)
    {
        NewFloatingNumberPool floatingNumberPool = _floatingNumberPoolDic[info];
        floatingNumberPool.Play(list);
    }


    public void CharacterListGroupRefresh()
    {
        CharacterListGroup.Refresh();
    }

    public void SetDirectionGroupVisible(bool isVisible)
    {
        DirectionGroup.gameObject.SetActive(isVisible);
    }

    public void SetDirectionGroupPosition(Vector3 position) 
    {
        _directionPosition = position;
    }

    public void AddLog(string text)
    {
        LogGroup.AddLog(text);
    }

    public void ShowArrow(Transform transform) 
    {
        Arrow.Show(transform);
    }

    public void HideArrow()
    {
        Arrow.Hide();
    }

    public void SetTileLabel(BattleInfoTile tile) 
    {
        if (tile == null)
        {
            TileLabel.text = "";
        }
        else
        {
            TileLabel.text = "°ª«×¡G" + tile.TileData.Height.ToString();
        }
    }

    private void DirectionButtonOnClick(Vector2Int direction)
    {
        BattleController.Instance.SetDirection(direction);
    }

    private void Awake()
    {
        Instance = this;
        SetDirectionGroupVisible(false);
        DirectionGroup.ClickHandler += DirectionButtonOnClick;
    }

    private void Update()
    {
        DirectionGroup.transform.position = Camera.main.WorldToScreenPoint(_directionPosition + Vector3.down * 0.4f);
        DirectionGroup.transform.eulerAngles = new Vector3(90 - Camera.main.transform.eulerAngles.x, 0, Camera.main.transform.eulerAngles.y);
    }

    void Destroy()
    {
        Instance = null;
    }
}
