using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Battle;

public class SelectBattleCharacterUI : MonoBehaviour
{
    public DragCharacterImage DragCharacterImage;
    public Image DragCharacterBG;
    public Transform CharacterListGroup;
    public DragCameraUI DragCameraUI;
    public Button ConfirmButton;
    public TipLabel TipLabel;
    public CharacterInfoUI CharacterInfoUI;

    private List<CharacterInfo> _tempCharacterList = new List<CharacterInfo>();
    private List<CharacterInfo> _selectedCharacterList = new List<CharacterInfo>();
    private Dictionary<CharacterInfo, DragCharacterImage> _dragCharacterImageDic = new Dictionary<CharacterInfo, DragCharacterImage>();
    private Dictionary<CharacterInfo, Image> _dragCharacterBGDic = new Dictionary<CharacterInfo, Image>();

    // Start is called before the first frame update
    void Awake()
    {
        Image dragCharacterBG;
        DragCharacterImage dragCharacterImage;
        _tempCharacterList = new List<CharacterInfo>(CharacterManager.Instance.Info.CharacterList);
        for (int i=0; i<_tempCharacterList.Count; i++) 
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
        if(_selectedCharacterList.Count == 5) 
        {
            BattleController.Instance.SetCharacterState();
        }
        else
        {
            TipLabel.SetLabel("要有五個角色參戰才能開始戰鬥");
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
