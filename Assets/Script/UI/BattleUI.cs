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
    public SkillButtonGroup SkillButtonGroup;
    public AnchorValueBar LittleHpBar;
    public FloatingNumberPool FloatingNumberPool;
    public CharacterListGroup CharacterListGroup;
    public ScrollView ItemScrollView;

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

    public void SetSkillVisible(bool isVisible) 
    {
        SkillButtonGroup.gameObject.SetActive(isVisible);
    }

    public void SetSkillData(List<Skill> skillList) 
    {
        SkillButtonGroup.SetData(skillList);
    }

    public void SetSupportData(List<Support> supportList, int currentSP) 
    {
        SkillButtonGroup.SetData(supportList, currentSP);
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
        hpBar.transform.parent = transform;
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

    public void SetItemScrollView() 
    {
        ItemScrollView.transform.parent.gameObject.SetActive(true);
        ItemScrollView.SetData(new List<object>(ItemManager.Instance.GetItemList(ItemModel.CategoryEnum.Medicine)));
    }

    private void BackgroundOnClick(ButtonPlus buttonPlus) 
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

    private void BackgroundDown(ButtonPlus button) 
    {
        _dragOrigin = Input.mousePosition;
    }

    private void BackgroundUp(ButtonPlus button) 
    {
        if (Vector2.Distance(_dragOrigin, Input.mousePosition) > _distance)
        {
            Vector3 v1 = Camera.main.ScreenToViewportPoint(_dragOrigin - Input.mousePosition);
            Vector3 v2 = new Vector3(v1.x * Mathf.Sin(45) + v1.y * Mathf.Cos(45), 0, v1.x * Mathf.Sin(-45) + v1.y * Mathf.Cos(-45));
            Vector3 move = new Vector3(v2.x * CameraDragSpeed, 0, v2.z * CameraDragSpeed);
            float x = Mathf.Clamp(Camera.main.transform.position.x + move.x, -10, _width - 10);
            float z = Mathf.Clamp(Camera.main.transform.position.z + move.z, -10, _height - 10);
            Camera.main.transform.DOMove(new Vector3(x, Camera.main.transform.position.y, z), 1f);

            //if (Camera.main.transform.position.x + move.x > -10 && Camera.main.transform.position.x + move.x < _width - 10 && Camera.main.transform.position.z + move.z > -10 && Camera.main.transform.position.z + move.z < _height - 10)
            //{
            //    Camera.main.transform.DOMove(Camera.main.transform.position + move, 1f);
            //}
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

    private void IdleOnClick() 
    {
        BattleController.Instance.Idle();
    }

    private void Awake()
    {
        _graphicRaycaster = transform.parent.GetComponent<GraphicRaycaster>();

        ItemScrollView.Init();

        //IdleButton.onClick.AddListener(IdleOnClick);
        BackgroundButton.DownHandler += BackgroundDown;
        BackgroundButton.UpHandler += BackgroundUp;
    }

    private void Update()
    {
        //_pointerEventData.position = Input.mousePosition;
        //_graphicHitList.Clear();
        //_graphicRaycaster.Raycast(_pointerEventData, _graphicHitList);
        //Debug.Log(_graphicHitList.Count);
        //if (_graphicHitList.Count == 0)
        //{
        //    if (Input.GetMouseButtonDown(0))
        //    {
        //        _dragOrigin = Input.mousePosition;
        //    }
        //    else if (Input.GetMouseButtonUp(0))
        //    {
        //        if (Vector2.Distance(_dragOrigin, Input.mousePosition) > _distance)
        //        {
        //            Vector3 v1 = Camera.main.ScreenToViewportPoint(_dragOrigin - Input.mousePosition);
        //            Vector3 v2 = new Vector3(v1.x * Mathf.Sin(45) + v1.y * Mathf.Cos(45), 0, v1.x * Mathf.Sin(-45) + v1.y * Mathf.Cos(-45));
        //            Vector3 move = new Vector3(v2.x * CameraDragSpeed, 0, v2.z * CameraDragSpeed);

        //            if (Camera.main.transform.position.x + move.x > -10 && Camera.main.transform.position.x + move.x < _width - 10 && Camera.main.transform.position.z + move.z > -10 && Camera.main.transform.position.z + move.z < _height - 10)
        //            {
        //                Camera.main.transform.DOMove(Camera.main.transform.position + move, 1f);
        //            }
        //        }
        //        else
        //        {
        //            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //            RaycastHit hit;
        //            if (Physics.Raycast(ray, out hit, 100))
        //            {
        //                BattleController.Instance.Click(new Vector2(hit.transform.position.x, hit.transform.position.z));
        //            }
        //            else //代表按到沒有按鍵的地方
        //            {
        //                BattleController.Instance.Click(new Vector2(int.MinValue, int.MinValue));
        //            }
        //        }
        //    }
        //}
    }
}
