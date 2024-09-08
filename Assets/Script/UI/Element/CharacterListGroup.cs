using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Battle;

public class CharacterListGroup : MonoBehaviour
{
    private static readonly int _max = 8;

    public Image Image;

    private List<Image> _imageList = new List<Image>();
    private List<BattleCharacterInfo> _characterList = new List<BattleCharacterInfo>();
    private Dictionary<BattleCharacterInfo, Sprite> _spriteDic = new Dictionary<BattleCharacterInfo, Sprite>();

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
            _spriteDic.Add(_characterList[i], sprite);
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
        _spriteDic.Add(character, sprite);
        
    }

    public void ChangeSprite(BattleCharacterInfo info, string sprite) 
    {
        _spriteDic[info] = Resources.Load<Sprite>("Image/" + sprite + "_F");
        Refresh();
    }

    public void Refresh() 
    {
        for (int i=0; i<_imageList.Count; i++) 
        {
            if (i < _characterList.Count)
            {
                _imageList[i].gameObject.SetActive(true);
                _imageList[i].sprite = _spriteDic[_characterList[i]];
            }
            else
            {
                _imageList[i].gameObject.SetActive(false);
            }
        }

        //for (int i=0; i<_characterList.Count; i++) 
        //{
        //    if (i < _max)
        //    {
        //        _imageList[i].gameObject.SetActive(true);
        //        _imageList[i].sprite = _spriteDic[_characterList[i]];
        //    }
        //    else
        //    {
        //        _imageList[i].gameObject.SetActive(false);
        //    }
        //}
    }
}
