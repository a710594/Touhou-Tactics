using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectBattleCharacterUI : MonoBehaviour
{
    public DragCharacter DragCharacter;
    public Transform CharacterListGroup;

    private List<CharacterInfo> _tempCharacterList = new List<CharacterInfo>();
    private Dictionary<CharacterInfo, DragCharacter> _dragCharacterDic = new Dictionary<CharacterInfo, DragCharacter>();

    // Start is called before the first frame update
    void Start()
    {
        DragCharacter dragCharacter;
        _tempCharacterList = new List<CharacterInfo>(CharacterManager.Instance.CharacterInfoGroup.CharacterList);
        for (int i=0; i<_tempCharacterList.Count; i++) 
        {
            dragCharacter = Instantiate(DragCharacter);
            dragCharacter.transform.SetParent(CharacterListGroup);
            dragCharacter.SetData(_tempCharacterList[i]);
            dragCharacter.DragEndHandler += OnDragEnd;
            _dragCharacterDic.Add(_tempCharacterList[i], dragCharacter);
        }
    }

    private void OnDragEnd(CharacterInfo character) 
    {
        _tempCharacterList.Remove(character);
        _dragCharacterDic[character].gameObject.SetActive(false);
    }
}
