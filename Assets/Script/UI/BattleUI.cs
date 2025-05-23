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

    public Button BackgroundButton;
    public CommandGroup CommandGroup;
    public LittleHpBarWithStatus LittleHpBarWithStatus;
    public Transform HPGroup;
    public FloatingNumberPool FloatingNumberPool;
    public CharacterListGroup CharacterListGroup;
    public DirectionGroup DirectionGroup;
    public LogGroup LogGroup;
    public ArrowImage Arrow;

    private Vector3 _directionPosition = new Vector3();    
   // private Dictionary<BattleCharacterController, LittleHpBarWithStatus> _littleHpBarDic = new Dictionary<BattleCharacterController, LittleHpBarWithStatus>();
    private Dictionary<BattleCharacterController, FloatingNumberPool> _floatingNumberPoolDic  = new Dictionary<BattleCharacterController, FloatingNumberPool>();

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
        FloatingNumberPool floatingNumberPool = Instantiate(FloatingNumberPool);
        floatingNumberPool.transform.SetParent(transform);
        floatingNumberPool.SetAnchor(controller.transform);
        _floatingNumberPoolDic.Add(controller, floatingNumberPool);
    }

    public void PlayFloatingNumberPool(BattleCharacterController info, Log log)
    {
        FloatingNumberPool floatingNumberPool = _floatingNumberPoolDic[info];
        floatingNumberPool.Play(log);
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

    private void DirectionButtonOnClick(Vector2Int direction)
    {
        BattleController.Instance.SetDirection(direction);
    }

    private void BackgroundOnClick() 
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100))
        {
            Vector2Int v2 = Utility.ConvertToVector2Int(hit.point);
            if (BattleController.Instance.IsTutorialActive && !BattleController.Instance.Tutorial.CheckClick(v2))
            {
                return;
            }
            BattleController.Instance.Click(v2);
        }
        else //?N??????S???????a??
        {
            if (BattleController.Instance.IsTutorialActive)
            {
                return;
            }
            BattleController.Instance.Click(new Vector2Int(int.MinValue, int.MinValue));
        }
    }

    private void Awake()
    {
        Instance = this;
        SetDirectionGroupVisible(false);
        DirectionGroup.ClickHandler += DirectionButtonOnClick;
        BackgroundButton.onClick.AddListener(BackgroundOnClick);
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
