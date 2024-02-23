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
        [System.NonSerialized]
        public int PlayerCount;

        private Timer _timer = new Timer();
        private List<CharacterInfo> _tempCharacterList = new List<CharacterInfo>();
        private List<CharacterInfo> _selectedCharacterList = new List<CharacterInfo>();
        private Dictionary<CharacterInfo, DragCharacterImage> _dragCharacterImageDic = new Dictionary<CharacterInfo, DragCharacterImage>();
        private Dictionary<CharacterInfo, Image> _dragCharacterBGDic = new Dictionary<CharacterInfo, Image>();

        public void Init(BattleInfo info)
        {
            PlayerCount = info.PlayerCount;
            if (info.IsTutorial)
            {
                _tempCharacterList.Clear();
                _tempCharacterList.Add(CharacterManager.Instance.Info.CharacterList[3]); //妖夢
                _timer.Start(0.5f, ()=> 
                {
                    TutorialUI.Open("將角色從下方拖曳至場景的白色區域中。\n白色的區域代表可放置角色的位置。\n將角色配置完後按開始戰鬥。", "Tutorial_1", null);
                });
            }
            else
            {
                _tempCharacterList = new List<CharacterInfo>(CharacterManager.Instance.Info.CharacterList);
                for (int i=0; i<_tempCharacterList.Count; i++) 
                {
                    if (_tempCharacterList[i].CurrentHP == 0) 
                    {
                        _tempCharacterList.RemoveAt(i);
                        i--;
                    }
                }
            }

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
            if (_selectedCharacterList.Count == PlayerCount)
            {
                BattleController.Instance.SetCharacterState();
            }
            else
            {
                TipLabel.SetLabel("要有" + PlayerCount + "個角色參戰才能開始戰鬥");
            }
        }

        private void Update()
        {
        }
    }
}