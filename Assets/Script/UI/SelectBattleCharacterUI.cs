using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectBattleCharacterUI : MonoBehaviour
{
    public DragCharacterImage DragCharacterImage;
    public Image DragCharacterBG;
    public Transform CharacterListGroup;
    public DragCameraUI DragCameraUI;

    private List<CharacterInfo> _tempCharacterList = new List<CharacterInfo>();
    private Dictionary<CharacterInfo, DragCharacterImage> _dragCharacterImageDic = new Dictionary<CharacterInfo, DragCharacterImage>();
    private Dictionary<CharacterInfo, Image> _dragCharacterBGDic = new Dictionary<CharacterInfo, Image>();

    // Start is called before the first frame update
    void Start()
    {
        Image dragCharacterBG;
        DragCharacterImage dragCharacterImage;
        _tempCharacterList = new List<CharacterInfo>(CharacterManager.Instance.CharacterInfoGroup.CharacterList);
        for (int i=0; i<_tempCharacterList.Count; i++) 
        {
            dragCharacterBG = Instantiate(DragCharacterBG);
            dragCharacterBG.transform.SetParent(CharacterListGroup);
            _dragCharacterBGDic.Add(_tempCharacterList[i], dragCharacterBG);

            dragCharacterImage = Instantiate(DragCharacterImage);
            dragCharacterImage.transform.SetParent(dragCharacterBG.transform);
            dragCharacterImage.SetData(_tempCharacterList[i]);
            dragCharacterImage.DragEndHandler += OnDragEnd;
            _dragCharacterImageDic.Add(_tempCharacterList[i], dragCharacterImage);
        }
    }
    private void OnStartDrag(CharacterInfo character) 
    {
    
    }

    private void OnDragEnd(CharacterInfo character) 
    {
        _tempCharacterList.Remove(character);
        _dragCharacterImageDic[character].transform.SetParent(this.transform);
        _dragCharacterBGDic[character].gameObject.SetActive(false);
    }
}
