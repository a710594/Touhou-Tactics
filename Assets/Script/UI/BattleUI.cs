using Battle;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BattleUI : MonoBehaviour
{
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
    public GameObject DirectionGroup;

    private Vector3 _directionPosition = new Vector3();
private CameraRotate _cameraRotate;
    private Dictionary<int, LittleHpBarWithStatus> _littleHpBarDic = new Dictionary<int, LittleHpBarWithStatus>();
    private Dictionary<int, FloatingNumberPool> _floatingNumberPoolDic  = new Dictionary<int, FloatingNumberPool>();

    public void SetVisible(bool isVisible) 
    {
        gameObject.SetActive(isVisible);
    }

    public void SetCharacterInfoUI_1(BattleCharacterInfo info) 
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

    public void SetCharacterInfoUI_2(BattleCharacterInfo info)
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
    }

    public void SetSkillVisible(bool isVisible) 
    {
        ActionButtonGroup.ScrollView.transform.parent.gameObject.SetActive(isVisible);
    }

    public void SetActionScrollView(List<object> list) 
    {
        ActionButtonGroup.SetScrollView(list);
    }

    public void SetPredictionInfo(BattleCharacterInfo info, int predictionHp)
    {
        CharacterInfoUI_2.SetHpPrediction(info.CurrentHP, predictionHp, info.MaxHP);
    }

    public void SetPredictionLittleHpBar(BattleCharacterInfo info, int predictionHp)
    {
        LittleHpBarWithStatus hpBar = _littleHpBarDic[info.Index];
        hpBar.SetPrediction(info.CurrentHP, predictionHp, info.MaxHP);
    }

    public void StopPredictionInfo()
    {
        CharacterInfoUI_2.StopHpPrediction();
    }

    public void StopPredictionLittleHpBar(BattleCharacterInfo info)
    {
        LittleHpBarWithStatus hpBar = _littleHpBarDic[info.Index];
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

    public void SetLittleHpBarAnchor(int id, BattleCharacterController characterController) 
    {
        LittleHpBarWithStatus hpBar = Instantiate(LittleHpBarWithStatus);
        hpBar.transform.SetParent(HPGroup);
        hpBar.SetAnchor(characterController.HpAnchor);
        _littleHpBarDic.Add(id, hpBar);
    }

    public void SetLittleHpBarValue(int id, BattleCharacterInfo characterInfo)
    {
        LittleHpBarWithStatus hpBar = _littleHpBarDic[id];
        if (characterInfo.CurrentHP > 0)
        {
            hpBar.gameObject.SetActive(true);
            hpBar.SetData(characterInfo);
        }
        else 
        {
            hpBar.gameObject.SetActive(false);
        }
    }

    public void SetFloatingNumberPoolAnchor(int id, BattleCharacterController characterController)
    {
        FloatingNumberPool floatingNumberPool = Instantiate(FloatingNumberPool);
        floatingNumberPool.transform.SetParent(transform);
        floatingNumberPool.SetAnchor(characterController.HpAnchor);
        _floatingNumberPoolDic.Add(id, floatingNumberPool);
    }

    public void PlayFloatingNumberPool(int id, FloatingNumberData.TypeEnum type, string text)
    {
        FloatingNumberPool floatingNumberPool = _floatingNumberPoolDic[id];
        floatingNumberPool.Play(text, type);
    }

    public void CharacterListGroupInit(List<BattleCharacterInfo> characterList)
    {
        CharacterListGroup.Init(characterList);
    }

    public void CharacterListGroupRefresh()
    {
        CharacterListGroup.Refresh();
    }

    public void SetDirectionGroupVisible(bool isVisible)
    {
        DirectionGroup.SetActive(isVisible);
    }

    public void SetDirectionGroupPosition(Vector3 position) 
    {
        _directionPosition = position;
    }

    //public void DropPowerPoint(List<BattleCharacterInfo> targetList)
    //{
    //    GameObject obj;
    //    for (int i = 0; i < targetList.Count; i++)
    //    {
    //        obj = Instantiate(PowerPoint, Vector3.zero, Quaternion.identity);
    //        obj.transform.SetParent(PowerPoint.transform.parent);
    //        obj.transform.position = Camera.main.WorldToScreenPoint(targetList[i].Position + Vector3.up * 0.5f);
    //        JumpPowerPoint(obj);
    //    }
    //}

    //private void JumpPowerPoint(GameObject obj)
    //{
    //    obj.transform.DOLocalJump(obj.transform.localPosition + Vector3.right * UnityEngine.Random.Range(-50, 50), 50, 1, 0.5f).OnComplete(() =>
    //    {
    //        obj.transform.DOMove(PPGroup.transform.position, 0.5f).OnComplete(() =>
    //        {
    //            Destroy(obj);
    //        });
    //    });
    //}

    private void Rotate()
    {
        if (_cameraRotate.CurrentState == CameraRotate.StateEnum.Slope)
        {
            DirectionGroup.transform.eulerAngles = new Vector3(60, 0, 45);
        }
        else
        {
            DirectionGroup.transform.eulerAngles = new Vector3(0, 0, 0);
        }
    }

    private void Awake()
    {
        SetDirectionGroupVisible(false);
        _cameraRotate = Camera.main.GetComponent<CameraRotate>();
        _cameraRotate.RotateHandler += Rotate;
    }

    private void Update()
    {
        DirectionGroup.transform.position = Camera.main.WorldToScreenPoint(_directionPosition + Vector3.down * 0.4f);
    }
}
