using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Battle;

namespace Battle
{
    public class SelectBattleCharacterUI : MonoBehaviour
    {
        public DragCharacterImage DragCharacterImage;
        public Image DragCharacterBG;
        public Transform CharacterListGroup;
        public DragCameraUI DragCameraUI;
        public Button ConfirmButton;
        public TipLabel TipLabel;
        public CharacterInfoUI CharacterInfoUI;

        private int _needCount;
        private bool _mustBeEqualToNeedCount;
        
        private Timer _timer = new Timer();
        private List<CharacterInfo> _tempCharacterList = new List<CharacterInfo>();
        private List<CharacterInfo> _selectedCharacterList = new List<CharacterInfo>();
        private Dictionary<CharacterInfo, DragCharacterImage> _dragCharacterImageDic = new Dictionary<CharacterInfo, DragCharacterImage>();
        private Dictionary<CharacterInfo, Image> _dragCharacterBGDic = new Dictionary<CharacterInfo, Image>();

        public void Init(int needCount, bool mustBeEqualToNeedCount, List<CharacterInfo> list)
        {
            _needCount = needCount;
            _mustBeEqualToNeedCount = mustBeEqualToNeedCount;
            _tempCharacterList = list;

            Image dragCharacterBG;
            DragCharacterImage dragCharacterImage;
            for (int i = 0; i < _tempCharacterList.Count; i++)
            {
                dragCharacterBG = Instantiate(DragCharacterBG);
                dragCharacterBG.transform.SetParent(CharacterListGroup);
                _dragCharacterBGDic.Add(_tempCharacterList[i], dragCharacterBG);

                dragCharacterImage = Instantiate(DragCharacterImage);
                dragCharacterImage.transform.SetParent(dragCharacterBG.transform);
                dragCharacterImage.SetData(_tempCharacterList[i]);
                dragCharacterImage.DragEndHandler += OnDragEnd;
                dragCharacterImage.EnterHandler += OnEnter;
                dragCharacterImage.ExitHandler += OnExit;
                dragCharacterImage.RightClickHandler += OnRightClick;
                _dragCharacterImageDic.Add(_tempCharacterList[i], dragCharacterImage);
            }
        }

        // Start is called before the first frame update
        void Awake()
        {
            ConfirmButton.onClick.AddListener(ConfirmOnClick);
            CharacterInfoUI.gameObject.SetActive(false);
        }
        private void OnStartDrag(CharacterInfo character)
        {

        }

        private void OnDragEnd(CharacterInfo character)
        {
            _tempCharacterList.Remove(character);
            if (!_selectedCharacterList.Contains(character))
            {
                _selectedCharacterList.Add(character);
            }
            _dragCharacterImageDic[character].transform.SetParent(this.transform);
            _dragCharacterBGDic[character].gameObject.SetActive(false);
        }

        private void OnRightClick(CharacterInfo character)
        {
            _tempCharacterList.Add(character);
            _selectedCharacterList.Remove(character);
            _dragCharacterImageDic[character].transform.SetParent(_dragCharacterBGDic[character].transform);
            _dragCharacterBGDic[character].gameObject.SetActive(true);
        }

        private void OnEnter(CharacterInfo character)
        {
            CharacterInfoUI.SetData(character);
            CharacterInfoUI.gameObject.SetActive(true);
        }

        private void OnExit(CharacterInfo character)
        {
            CharacterInfoUI.gameObject.SetActive(false);
        }

        private void ConfirmOnClick()
        {
            if (_selectedCharacterList.Count == _needCount)
            {
                BattleController.Instance.SetCharacterState();
            }
            else
            {
                if(_mustBeEqualToNeedCount)
                {
                    ConfirmUI.Open("必需放置" + _needCount + "個角色才能開始戰鬥", "確定", null);
                }
                else
                {
                    ConfirmUI.Open("還可以再放置" + (_needCount - _selectedCharacterList.Count) + "個角色，確定要開始戰鬥嗎？", "確定", "取消", ()=> 
                    {
                        BattleController.Instance.SetCharacterState();
                    }, null);
                }
            }
        }

        private void Update()
        {
        }
    }
}