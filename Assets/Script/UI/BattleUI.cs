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
    public SkillGroup SkillGroup;
    public AnchorValueBar LittleHpBar;
    public Transform HPGroup;
    public FloatingNumberPool FloatingNumberPool;
    public CharacterListGroup CharacterListGroup;
    //public GameObject PowerPoint;
    public TipLabel TipLabel;

    private GraphicRaycaster _graphicRaycaster;
    private PointerEventData _pointerEventData = new PointerEventData(null);
    private List<RaycastResult> _graphicHitList = new List<RaycastResult>();
    private Dictionary<int, AnchorValueBar> _littleHpBarDic = new Dictionary<int, AnchorValueBar>();
    private Dictionary<int, FloatingNumberPool> _floatingNumberPoolDic  = new Dictionary<int, FloatingNumberPool>();

    //camera drag
    public float CameraDragSpeed = 100;

    private int _width;
    private int _height;
    private float _distance = 20;
    private Vector3 _dragOrigin;
    private CameraRotate _cameraRotate;

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
        SkillGroup.gameObject.SetActive(isVisible);
    }

    public void SetSkillData(List<Skill> skillList) 
    {
        SkillGroup.SetData(skillList);
    }

    public void SetSupportData(List<Support> supportList, int currentSP) 
    {
        SkillGroup.SetData(supportList, currentSP);
    }

    public void SetItemData(BattleCharacterInfo character)
    {
        SkillGroup.SetData(character);
    }

    public void SetHpPrediction(int origin, int prediction, int max)
    {
        CharacterInfoUI_2.SetHpPrediction(origin, prediction, max);
    }

    public void StopHpPrediction()
    {
        CharacterInfoUI_2.StopHpPrediction();
    }

    public void SetLittleHpBarAnchor(int id, BattleCharacterController characterController) 
    {
        AnchorValueBar hpBar = Instantiate(LittleHpBar);
        hpBar.transform.SetParent(HPGroup);
        hpBar.SetAnchor(characterController.HpAnchor);
        _littleHpBarDic.Add(id, hpBar);
    }

    public void SetLittleHpBarValue(int id, BattleCharacterInfo characterInfo)
    {
        AnchorValueBar hpBar = _littleHpBarDic[id];
        if (characterInfo.CurrentHP > 0)
        {
            hpBar.SetValue(characterInfo.CurrentHP, characterInfo.MaxHP);
        }
        else 
        {
            hpBar.gameObject.SetActive(false);
        }
    }

    public void SetFloatingNumberPoolAnchor(int id, BattleCharacterController characterController)
    {
        FloatingNumberPool floatingNumberPool = Instantiate(FloatingNumberPool);
        floatingNumberPool.transform.parent = transform;
        floatingNumberPool.SetAnchor(characterController.HpAnchor);
        _floatingNumberPoolDic.Add(id, floatingNumberPool);
    }

    public void PlayFloatingNumberPool(int id, FloatingNumberData.TypeEnum type, string text)
    {
        FloatingNumberPool floatingNumberPool = _floatingNumberPoolDic[id];
        floatingNumberPool.Play(text, type);
    }

    public void SetMapInfo(int width, int height)
    {
        _width = width;
        _height = height;
    }

    public void CharacterListGroupInit(List<BattleCharacterInfo> characterList)
    {
        CharacterListGroup.Init(characterList);
    }

    public void CharacterListGroupRefresh()
    {
        CharacterListGroup.Refresh();
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

    private void BackgroundDown(ButtonPlus button) 
    {
        _dragOrigin = Input.mousePosition;
    }

    private void BackgroundUp(ButtonPlus button) 
    {
        if (Vector2.Distance(_dragOrigin, Input.mousePosition) > _distance)
        {
            float x;
            float z;
            Vector3 move;
            Vector3 v1 = Camera.main.ScreenToViewportPoint(_dragOrigin - Input.mousePosition);
            if (_cameraRotate.CurrentState == CameraRotate.StateEnum.Slope) 
            {
                Vector3 v2 = new Vector3(v1.x * Mathf.Sin(45) + v1.y * Mathf.Cos(45), 0, v1.x * Mathf.Sin(-45) + v1.y * Mathf.Cos(-45));
                move = new Vector3(v2.x * CameraDragSpeed, 0, v2.z * CameraDragSpeed);
                x = Mathf.Clamp(Camera.main.transform.position.x + move.x, -10, _width - 10);
                z = Mathf.Clamp(Camera.main.transform.position.z + move.z, -10, _height - 10);
            }
            else 
            {
                move = new Vector3(v1.x * CameraDragSpeed, 0, v1.y * CameraDragSpeed);
                x = Mathf.Clamp(Camera.main.transform.position.x + move.x, 0, _width);
                z = Mathf.Clamp(Camera.main.transform.position.z + move.z, 0, _height);
            }

            Camera.main.transform.DOMove(new Vector3(x, Camera.main.transform.position.y, z), 1f);
        }
        else
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100))
            {
                BattleController.Instance.Click(Utility.ConvertToVector2Int(hit.transform.position));
            }
            else //代表按到沒有按鍵的地方
            {
                BattleController.Instance.Click(new Vector2Int(int.MinValue, int.MinValue));
            }
        }
    }

    private void Awake()
    {
        _graphicRaycaster = transform.parent.GetComponent<GraphicRaycaster>();
        _cameraRotate = Camera.main.GetComponent<CameraRotate>();

        //IdleButton.onClick.AddListener(IdleOnClick);
        BackgroundButton.DownHandler += BackgroundDown;
        BackgroundButton.UpHandler += BackgroundUp;
    }

    private void Update()
    {
    }
}
