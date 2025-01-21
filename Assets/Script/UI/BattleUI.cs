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

    //public Button IdleButton;
    public ButtonPlus BackgroundButton;
    public CharacterInfoUI CharacterInfoUI_1;
    public CharacterInfoUI CharacterInfoUI_2;
    public ActionButtonGroup ActionButtonGroup;
    //public SkillGroup SkillGroup;
    public LittleHpBarWithStatus LittleHpBarWithStatus;
    public Transform HPGroup;
    public FloatingNumberPool FloatingNumberPool;
    public CharacterListGroup CharacterListGroup;
    //public GameObject PowerPoint;
    public TipLabel TipLabel;
    public DirectionGroup DirectionGroup;
    public CameraRotate CameraRotate;
    public LogGroup LogGroup;
    public Image Arrow;

    private Vector3 _arrowOffset = new Vector3(0, 1.5f, 0);
    private Vector3 _directionPosition = new Vector3();    
    private Dictionary<BattleCharacterController, LittleHpBarWithStatus> _littleHpBarDic = new Dictionary<BattleCharacterController, LittleHpBarWithStatus>();
    private Transform _arrowTransform = null;
    private Dictionary<BattleCharacterController, FloatingNumberPool> _floatingNumberPoolDic  = new Dictionary<BattleCharacterController, FloatingNumberPool>();

    public void SetVisible(bool isVisible) 
    {
        gameObject.SetActive(isVisible);
    }

    public void SetCharacterInfoUI_1(BattleCharacterControllerData info) 
    {
        if (info != null)
        {
            CharacterInfoUI_1.SetVisible(true);
            CharacterInfoUI_1.SetData(info);
        }
        else 
        {
            CharacterInfoUI_1.SetVisible(false);
        }
    }

    public void SetCharacterInfoUI_2(BattleCharacterControllerData info)
    {
        if (info != null)
        {
            CharacterInfoUI_2.SetVisible(true);
            CharacterInfoUI_2.SetData(info);
        }
        else
        {
            CharacterInfoUI_2.SetVisible(false);
        }
    }

    public void SetActionVisible(bool isVisible)
    {
        ActionButtonGroup.gameObject.SetActive(isVisible);
        LogGroup.OpenButton.gameObject.SetActive(isVisible);
    }

    public void SetSkillVisible(bool isVisible) 
    {
        ActionButtonGroup.ScrollView.transform.parent.gameObject.SetActive(isVisible);
    }

    public void SetActionScrollView(List<object> list) 
    {
        ActionButtonGroup.SetScrollView(list);
    }

    public void SetPredictionInfo_1(BattleCharacterControllerData info, int predictionHp)
    {
        CharacterInfoUI_1.SetHpPrediction(info.CurrentHP, predictionHp, info.MaxHP);
    }

    public void SetPredictionInfo_2(BattleCharacterControllerData info, int predictionHp)
    {
        CharacterInfoUI_2.SetHpPrediction(info.CurrentHP, predictionHp, info.MaxHP);
    }

    public void SetPredictionLittleHpBar(BattleCharacterController controller, int predictionHp)
    {
        LittleHpBarWithStatus hpBar = _littleHpBarDic[controller];
        hpBar.SetPrediction(controller.Info.CurrentHP, predictionHp, controller.Info.MaxHP);
    }

    public void StopPredictionInfo()
    {
        CharacterInfoUI_2.StopHpPrediction();
    }

    public void StopPredictionLittleHpBar(BattleCharacterController controller)
    {
        LittleHpBarWithStatus hpBar = _littleHpBarDic[controller];
        hpBar.StopPrediction();
    }

    public void SetHitRate(int hitRate)
    {
        CharacterInfoUI_2.SetHitRate(hitRate);
    }

    public void HideHitRate()
    {
        CharacterInfoUI_2.HideHitRate();
    }

    public void SetLittleHpBarAnchor(BattleCharacterController controller) 
    {
        LittleHpBarWithStatus hpBar = Instantiate(LittleHpBarWithStatus);
        hpBar.transform.SetParent(HPGroup);
        hpBar.SetAnchor(controller.HpAnchor);
        _littleHpBarDic.Add(controller, hpBar);
    }

    public void SetLittleHpBarValue(BattleCharacterController controller)
    {
        LittleHpBarWithStatus hpBar = _littleHpBarDic[controller];
        if (controller.Info.CurrentHP > 0)
        {
            hpBar.gameObject.SetActive(true);
            hpBar.SetData(controller.Info);
        }
        else
        {
            hpBar.gameObject.SetActive(false);
        }
    }

    public void SetFloatingNumberPoolAnchor(BattleCharacterController controller)
    {
        FloatingNumberPool floatingNumberPool = Instantiate(FloatingNumberPool);
        floatingNumberPool.transform.SetParent(transform);
        floatingNumberPool.SetAnchor(controller.HpAnchor);
        _floatingNumberPoolDic.Add(controller, floatingNumberPool);
    }

    public void PlayFloatingNumberPool(BattleCharacterController info, List<Log> logList)
    {
        FloatingNumberPool floatingNumberPool = _floatingNumberPoolDic[info];
        floatingNumberPool.Play(new Queue<Log>(logList));
    }

    public void CharacterListGroupInit(List<BattleCharacterController> characterList)
    {
        CharacterListGroup.Init(characterList);
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

    public void SetArrowVisible(bool isVisible)
    {
        Arrow.gameObject.SetActive(isVisible);
    }

    public void SetArrowTransform(Transform transform)
    {
        _arrowTransform = transform;
    }

    public void AddLog(string text)
    {
        LogGroup.AddLog(text);
    }

    private void Rotate(int angle)
    {
        if (CameraRotate.CurrentState == CameraRotate.StateEnum.Slope)
        {
            DirectionGroup.transform.eulerAngles = new Vector3(60, 0, 45);
            _arrowOffset = new Vector3(0, 1.5f, 0);
        }
        else
        {
            DirectionGroup.transform.eulerAngles = new Vector3(0, 0, 0);
            angle = angle % 360;;
            if (angle == 0)
            {
                _arrowOffset = new Vector3(0, 0, 1.5f);
            }
            else if(angle == 90) 
            {
                _arrowOffset = new Vector3(1.5f, 0, 0);
            }
            else if (angle == 180)
            {
                _arrowOffset = new Vector3(0, 0, -1.5f);
            }
            else if (angle == 270 || angle == -90)
            {
                _arrowOffset = new Vector3(-1.5f, 0, 0);
            }
        }
    }

    private void DirectionButtonOnClick(Vector2Int direction)
    {
        if (BattleController.Instance.IsTutorialActive && !BattleController.Instance.Tutorial.CheckClick(direction))
        {
            return;
        }

        BattleController.Instance.SetDirection(direction);
    }

    private void Awake()
    {
        Instance = this;
        SetDirectionGroupVisible(false);
        CameraRotate.RotateHandler += Rotate;
        DirectionGroup.ClickHandler += DirectionButtonOnClick;
    }

    private void Update()
    {
        DirectionGroup.transform.position = Camera.main.WorldToScreenPoint(_directionPosition + Vector3.down * 0.4f);
        if (_arrowTransform != null) 
        {
            Arrow.transform.position = Camera.main.WorldToScreenPoint(_arrowTransform.position + _arrowOffset);
        }
    }

    void Destroy()
    {
        Instance = null;
    }
}
