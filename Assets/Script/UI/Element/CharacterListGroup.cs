using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterListGroup : MonoBehaviour
{
    private static readonly int _max = 8;

    public Image Image;

    private List<BattleCharacterInfo> _characterList = new List<BattleCharacterInfo>();
    private Dictionary<BattleCharacterInfo, Image> _imageDic = new Dictionary<BattleCharacterInfo, Image>();
    private Dictionary<int, Sprite> _spriteDic = new Dictionary<int, Sprite>();

    public void Init(List<BattleCharacterInfo> characterList)
    {
        _characterList = characterList;
        Image image;
        Sprite sprite;
        for (int i=0; i<_characterList.Count; i++) 
        {
            image = Instantiate(Image);
            image.transform.SetParent(transform);
            sprite = Resources.Load<Sprite>("Image/" + _characterList[i].Controller + "_F");
            image.sprite = sprite;
            _spriteDic.Add(_characterList[i].ID, sprite);

            _imageDic.Add(_characterList[i], image);
            if (i > _max) 
            {
                image.gameObject.SetActive(false);
            }
        }
    }

    public void Refresh() 
    {
        int index = 0;
        foreach(KeyValuePair<BattleCharacterInfo, Image> pair in _imageDic) 
        {
            if (index != -1 && index < _max) 
            {
                pair.Value.gameObject.SetActive(true);
                pair.Value.sprite = _spriteDic[_characterList[index].ID];
                index++;
            }
            else
            {
                pair.Value.gameObject.SetActive(false);
            }
        }
    }
}
