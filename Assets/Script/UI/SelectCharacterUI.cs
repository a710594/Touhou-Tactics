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
        public DragCameraUI DragCameraUI;
        public Button ConfirmButton;
        public TipLabel TipLabel;
        public CharacterInfoUI CharacterInfoUI;
        public Text NeedCountLabel;

        private int _needCount;
        private bool _mustBeEqualToNeedCount;
        
        private Timer _timer = new Timer();
        private List<CharacterInfo> _tempCharacterList = new List<CharacterInfo>();
        private List<CharacterInfo> _selectedCharacterList = new List<CharacterInfo>();

        public void Init(int needCount, bool mustBeEqualToNeedCount, List<CharacterInfo> list)
        {
            _needCount = needCount;
            _mustBeEqualToNeedCount = mustBeEqualToNeedCount;
            _tempCharacterList = list;

            if (mustBeEqualToNeedCount) 
            {
                NeedCountLabel.text = "可放置的角色數量(最少/最多):" + needCount.ToString() + " / " + needCount.ToString();
            }
            else 
            {
                NeedCountLabel.text = "可放置的角色數量(最少/最多):" + "1 / " + needCount.ToString();
            }

            ScrollView.SetData(new List<object>(_tempCharacterList));
            ScrollView.DragBegingHandler = OnDragBegin;
            ScrollView.DragHandler = OnDrag;
            ScrollView.DragEndHandler = OnDragEnd;
            //ScrollView.ClickHandler = OnClick;
            ScrollView.EnterHandler = OnEnter;
            ScrollView.ExitHandler = OnExit;
        }


        private void OnDragBegin(ButtonPlus buttonPlus) 
        {
            BattleController.Instance.SetCharacterSpriteVisible((CharacterInfo)buttonPlus.Data, false);
            buttonPlus.Image.transform.SetParent(transform);
        }

        private void OnDrag(PointerEventData eventData,  ButtonPlus buttonPlus) 
        {
            buttonPlus.Image.rectTransform.anchoredPosition += eventData.delta;
        }

        private void OnDragEnd(ButtonPlus buttonPlus)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Vector2Int position;
            CharacterInfo character = (CharacterInfo)buttonPlus.Data;
            if (Physics.Raycast(ray, out hit, 100))
            {
                position = Utility.ConvertToVector2Int(hit.point);
                BattleCharacterController controller = BattleController.Instance.PlaceCharacter(position, character);
                if (controller != null)
                {
                    controller.SetCharacterInfo(character);
                    controller.RightClickHandler = OnRightClick;
                    _tempCharacterList.Remove(character);
                    if (!_selectedCharacterList.Contains(character))
                    {
                        _selectedCharacterList.Add(character);
                    }
                    ScrollView.SetData(new List<object>(_tempCharacterList));
                }
                else
                {
                    BattleController.Instance.SetCharacterSpriteVisible(character, true);
                }
            }
            else
            {
                BattleController.Instance.SetCharacterSpriteVisible(character, true);
            }
            buttonPlus.Image.transform.SetParent(buttonPlus.transform);
            buttonPlus.Image.transform.localPosition = Vector3.zero;
        }

        /*private void OnClick(PointerEventData eventData, ButtonPlus buttonPlus)
        {
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                BattleController.Instance.RemoveCharacterSprite((CharacterInfo)buttonPlus.Data);
                _tempCharacterList.Add((CharacterInfo)buttonPlus.Data);
                _selectedCharacterList.Remove((CharacterInfo)buttonPlus.Data);
            }
        }*/

        private void OnRightClick(CharacterInfo info) 
        {
            BattleController.Instance.RemoveCharacterSprite(info);
            _tempCharacterList.Add(info);
            _selectedCharacterList.Remove(info);
            ScrollView.SetData(new List<object>(_tempCharacterList));
        }

        private void OnEnter(ButtonPlus buttonPlus)
        {
            CharacterInfoUI.SetData((CharacterInfo)buttonPlus.Data);
            CharacterInfoUI.gameObject.SetActive(true);
        }

        private void OnExit(ButtonPlus buttonPlus)
        {
            CharacterInfoUI.gameObject.SetActive(false);
        }

        private void ConfirmOnClick()
        {
            if (_selectedCharacterList.Count == _needCount)
            {
                BattleController.Instance.SetState<BattleController.CharacterState>();
            }
            else
            {
                if(_mustBeEqualToNeedCount)
                {
                    ConfirmUI.Open("必需放置" + _needCount + "個角色才能開始戰鬥", "確定", null);
                }
                else
                {
                    if(_selectedCharacterList.Count == 0) 
                    {
                        ConfirmUI.Open("至少要放置 1 個角色", "確定", null);
                    }
                    else if (_selectedCharacterList.Count < _needCount)
                    {
                        ConfirmUI.Open("還可以再放置" + (_needCount - _selectedCharacterList.Count) + "個角色，確定要開始戰鬥嗎？", "確定", "取消", () =>
                        {
                            BattleController.Instance.SetState<BattleController.CharacterState>();
                        }, null);
                    }
                    else
                    {
                        ConfirmUI.Open("不能放置超過" + _needCount + "個角色，多出了" + (_selectedCharacterList.Count - _needCount) + "個", "確定", null);
                    }
                }
            }
        }

        private void Update()
        {
        }

        void Awake()
        {
            ConfirmButton.onClick.AddListener(ConfirmOnClick);
            CharacterInfoUI.gameObject.SetActive(false);
        }
    }
}