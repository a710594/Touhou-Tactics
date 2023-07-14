using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterListGroup : MonoBehaviour
{
    public Image Image;

    private List<BattleCharacterInfo> _characterList = new List<BattleCharacterInfo>();
    private Dictionary<BattleCharacterInfo, Image> _imageDic = new Dictionary<BattleCharacterInfo, Image>();

    public void Init(List<BattleCharacterInfo> characterList)
    {
        _characterList = characterList;
        Image image;
        for (int i=0; i<_characterList.Count; i++) 
        {
            image = Instantiate(Image);
            image.transform.SetParent(transform);
            image.sprite = Resources.Load<Sprite>("Prefab/Image/" + _characterList[i].Controller + "_F"); 
            _imageDic.Add(_characterList[i], image);
        }
    }

    public void Refresh() 
    {
        int index = 0;
        foreach(KeyValuePair<BattleCharacterInfo, Image> pair in _imageDic) 
        {
            if (_characterList.Contains(pair.Key)) 
            {
                pair.Value.gameObject.SetActive(true);
                pair.Value.transform.SetSiblingIndex(_characterList.IndexOf(pair.Key));
                index++;
                //pair.Value.transform.localPosition = new Vector3(pair.Key.CurrentWT * Image.rectTransform.rect.width, 0, 0);
            }
            else
            {
                pair.Value.gameObject.SetActive(false);
            }
        }
    }
}
