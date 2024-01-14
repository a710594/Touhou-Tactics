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
                _tempCharacterList.Add(CharacterManager.Instance.Info.CharacterList[3]); //����
                _timer.Start(0.5f, ()=> 
                {
                    TutorialUI.Open("�N����q�U��즲�ܳ������զ�ϰ줤�C\n�զ⪺�ϰ�N��i��m���⪺��m�C\n�N����t�m������}�l�԰��C", "Tutorial_1", null);
                });
            }
            else
            {
                _tempCharacterList = new List<CharacterInfo>(CharacterManager.Instance.Info.CharacterList);
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
                TipLabel.SetLabel("�n��" + PlayerCount + "�Ө���ѾԤ~��}�l�԰�");
            }
        }

        private void Update()
        {
#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.V))
            {
                BattleController.Instance.SetWin();
            }
#endif
        }
    }
}