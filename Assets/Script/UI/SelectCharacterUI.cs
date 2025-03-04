using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Battle;
using UnityEngine.EventSystems;

namespace Battle
{
    public class SelectCharacterUI : MonoBehaviour
    {
        public ScrollView ScrollView;
        public Button ConfirmButton;
        public TipLabel TipLabel;
        public Text NeedCountLabel;

        private bool _isDrag = false;
        private int _minCount;
        private int _maxCount;
        private CameraController _camera;

        private BattleCharacterController _characterController;
        private List<BattleCharacterInfo> _candidateList = new List<BattleCharacterInfo>();

        public void Init(int minCount, int maxCount, List<CharacterInfo> list)
        {
            _minCount = minCount;
            _maxCount = maxCount;
            _candidateList.Clear();
            for (int i=0; i<list.Count; i++) 
            {
                _candidateList.Add(new BattlePlayerInfo(CharacterManager.Instance.Info.Lv, list[i]));
            }

            NeedCountLabel.text = "可放置的角色數量(最少/最多):" + _minCount + " / " + _maxCount;
            ScrollView.SetData(new List<object>(_candidateList));
            ScrollView.DragBegingHandler = OnDragBegin;
            ScrollView.DragHandler = OnDrag;
            ScrollView.DragEndHandler = OnDragEnd;
            ScrollView.EnterHandler = OnEnter;
            ScrollView.ExitHandler = OnExit;

            BattleController.Instance.CharacterInfoUIGroup.SetCharacterInfoUI_1(null);
            BattleController.Instance.CharacterInfoUIGroup.SetCharacterInfoUI_2(null);
        }


        private void OnDragBegin(ButtonPlus buttonPlus) 
        {
            BattleCharacterInfo info = (BattleCharacterInfo)buttonPlus.Data;
            _characterController = ((GameObject)GameObject.Instantiate(Resources.Load("Prefab/Battle/" + info.FileName), Vector3.zero, Quaternion.identity)).GetComponent<BattleCharacterController>();
            _characterController.Info = info;
            Vector3 screenPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
            _characterController.transform.position = screenPos;
            _isDrag = true;
        }

        private void OnDrag(PointerEventData eventData,  ButtonPlus buttonPlus) 
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100))
            {
                Debug.Log(hit.collider.name + " " + hit.collider.transform.position);
                if (hit.collider.tag == "BattleTile" && BattleController.Instance.PlayerPositionList.Contains(Utility.ConvertToVector2Int(hit.collider.transform.position)))
                {
                    BattleInfoTile tile = BattleController.Instance.TileDic[Utility.ConvertToVector2Int(hit.collider.transform.position)];
                    _characterController.transform.position = hit.collider.transform.position + hit.collider.transform.up * tile.TileData.Height;
                }
                else
                {
                    Vector3 screenPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
                    _characterController.transform.position = screenPos;
                }
            }
            else
            {
                Vector3 screenPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
                _characterController.transform.position = screenPos;
            }
        }

        private void OnDragEnd(ButtonPlus buttonPlus)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Vector2Int position;
            if (Physics.Raycast(ray, out hit, 100))
            {
                position = Utility.ConvertToVector2Int(hit.point);
                if (BattleController.Instance.PlaceCharacter(_characterController))
                {
                    _characterController.RightClickHandler = OnRightClick;
                    _candidateList.Remove(_characterController.Info);
                    ScrollView.SetData(new List<object>(_candidateList));
                }
                else
                {
                    //BattleController.Instance.SetCharacterSpriteVisible(character, true);
                }
            }
            else
            {
                //BattleController.Instance.SetCharacterSpriteVisible(character, true);
                GameObject.Destroy(_characterController.gameObject);
            }
            //buttonPlus.Image.transform.SetParent(buttonPlus.transform);
            //buttonPlus.Image.transform.localPosition = Vector3.zero;
            _camera.enabled = true;
            _isDrag = false;
        }

        private void OnRightClick(int jobId) 
        {
            //BattleController.Instance.RemoveCharacterSprite(jobId);
            //CharacterInfo info = CharacterManager.Instance.GetCharacterInfoById(jobId);
            //_tempCharacterList.Add(info);
            //_selectedCharacterList.Remove(info);
            //ScrollView.SetData(new List<object>(_tempCharacterList));
        }

        private void OnEnter(ButtonPlus buttonPlus)
        {
            BattleController.Instance.CharacterInfoUIGroup.SetCharacterInfoUI_1((BattleCharacterInfo)buttonPlus.Data);
            _camera.enabled = false;
        }

        private void OnExit(ButtonPlus buttonPlus)
        {
            BattleController.Instance.CharacterInfoUIGroup.SetCharacterInfoUI_1(null);
            if (!_isDrag) 
            {
                _camera.enabled = true;
            }
        }

        private void ConfirmOnClick()
        {
            if (BattleController.Instance.TempList.Count == _maxCount)
            {
                BattleController.Instance.SetState<BattleController.CharacterState>();
            }
            else if(BattleController.Instance.TempList.Count < _minCount)
            {
                ConfirmUI.Open("至少要放置" +_minCount + "個角色", "確定", null);
            }
            else if(BattleController.Instance.TempList.Count < _maxCount) 
            {
                ConfirmUI.Open("還可以再放置" + (_maxCount - BattleController.Instance.TempList.Count) + "個角色，確定要開始戰鬥嗎？", "確定", "取消", () =>
                {
                    BattleController.Instance.SetState<BattleController.CharacterState>();
                }, null);
            }
            else if(BattleController.Instance.TempList.Count > _maxCount) 
            {
                ConfirmUI.Open("不能放置超過" + _maxCount + "個角色，多出了" + (BattleController.Instance.TempList.Count - _maxCount) + "個", "確定", null);
            }
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(1))
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit, 100))
                {
                    if (hit.collider.tag == "BattleTile")
                    {
                        Vector2Int position = Utility.ConvertToVector2Int(hit.collider.transform.position);
                        for (int i=0; i< BattleController.Instance.TempList.Count; i++) 
                        {
                            if(position == Utility.ConvertToVector2Int(BattleController.Instance.TempList[i].transform.position)) 
                            {
                                _candidateList.Add(BattleController.Instance.TempList[i].Info);
                                GameObject.Destroy(BattleController.Instance.TempList[i].gameObject);
                                BattleController.Instance.TempList.RemoveAt(i);
                                ScrollView.SetData(new List<object>(_candidateList));
                                break;
                            }
                        }
                    }
                    else
                    {
                        Debug.Log(hit.collider.name);
                    }
                }
            }
        }

        void Awake()
        {
            _camera = Camera.main.GetComponent<CameraController>();
            ConfirmButton.onClick.AddListener(ConfirmOnClick);
        }
    }
}