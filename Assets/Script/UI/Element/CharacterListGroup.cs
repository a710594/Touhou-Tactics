using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Battle;

public class CharacterListGroup : MonoBehaviour
{
    private static readonly int _max = 8;

    public Image Image;

    private List<BattleCharacterInfo> _characterList = new List<BattleCharacterInfo>();
    private List<Image> _imageList = new List<Image>();
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
            if (i > _max)
            {
                image.gameObject.SetActive(false);
            }
            _imageList.Add(image);
            sprite = Resources.Load<Sprite>("Image/" + _characterList[i].Sprite + "_F");
            _spriteDic.Add(_characterList[i].Index, sprite);
        }
    }

    public void Add(BattleCharacterInfo character)
    {
        Image image;
        Sprite sprite;
        image = Instantiate(Image);
        image.transform.SetParent(transform);
        image.gameObject.SetActive(false);
        _imageList.Add(image);
        sprite = Resources.Load<Sprite>("Image/" + character.Sprite + "_F");
        _spriteDic.Add(character.Index, sprite);
        
    }

    public void ChangeSprite(int index, string sprite) 
    {
        _spriteDic[index] = Resources.Load<Sprite>("Image/" + sprite + "_F");
        Refresh();
    }

    public void Refresh() 
    {
        for (int i=0; i<_characterList.Count; i++) 
        {
            if (i < _max)
            {
                _imageList[i].gameObject.SetActive(true);
                _imageList[i].sprite = _spriteDic[_characterList[i].Index];
            }
            else
            {
                _imageList[i].gameObject.SetActive(false);
            }
        }

        //int index = 0;
        //foreach(KeyValuePair<BattleCharacterInfo, Image> pair in _imageDic) 
        //{
        //    if (index != -1 && index < _max && index < _characterList.Count) 
        //    {
        //        pair.Value.gameObject.SetActive(true);
        //        pair.Value.sprite = _spriteDic[_characterList[index].Index];
        //        index++;
        //    }
        //    else
        //    {
        //        pair.Value.gameObject.SetActive(false);
        //    }
        //}
    }
}
